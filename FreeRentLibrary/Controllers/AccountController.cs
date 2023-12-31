﻿using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Data.Repositories.IRepositories;
using FreeRentLibrary.Helpers.IHelpers;
using FreeRentLibrary.Helpers.SimpleHelpers;
using FreeRentLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FreeRentLibrary.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly IMailHelper _mailHelper;
        private readonly IRoleRepository _roleRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        private readonly ICountryRepository _countryRepository;

        public AccountController(IUserHelper userHelper, ICountryRepository countryRepository, IConfiguration configuration, IMailHelper mailHelper,
            IRoleRepository roleRepository, RoleManager<IdentityRole>roleManager)
        {
            _configuration = configuration;
            _mailHelper = mailHelper;
            _roleRepository = roleRepository;
            _roleManager = roleManager;
           
            _userHelper = userHelper;
            _countryRepository = countryRepository;
        }

        [HttpPost][HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult>IsEmailInUse(string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {username} is already in use");
            }
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Dashboards");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Dashboards");
            }
            if (ModelState.IsValid)
            {                
                var user = await _userHelper.GetUserByEmailAsync(model.UserName);
                if (user != null)
                {
                        if (user.TwoFactorEnabled)
                        {
                            var token = await _userHelper.GenerateTwoFactorAuthenticationTokenAsync(user);
                            _mailHelper.SendEmail(user.Email, "Two Factor Authentication", $"This is your authentication code: {token}");
                        
                            return this.RedirectToAction("VerifyLoginToken", "Account", model);
                        }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User Not Found");
                    return View(model);
                }
                    var result = await _userHelper.LoginAsync(model);
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }
                
            }
                this.ModelState.AddModelError(string.Empty, "Failed  to Login");
                return View(model);
            
        }


        public IActionResult VerifyLoginToken(TwoFactorViewModel model)
        {
            return View(model);
        }


        
        [HttpPost]
        public async Task<IActionResult> VerifyLoginToken(TwoFactorViewModel model, LoginViewModel modelLogin)
        {
            model.RememberMe = modelLogin.RememberMe;
            model.UserName= modelLogin.UserName;
            model.Password= modelLogin.Password;
            var user = await _userHelper.GetUserByEmailAsync(modelLogin.UserName);
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    var validToken = await _userHelper.TwoFactorAuthenticationConfirmationAsync(user, model.TwoFactorCode);
                    if (validToken)
                    {
                        await _userHelper.LoginAsync(modelLogin);
                        return RedirectToAction("Dashboard", "Dashboards");
                    }
                    this.ModelState.AddModelError(string.Empty, "Failed  validation");
                    return View(model);
                }
                this.ModelState.AddModelError(string.Empty, "Failed  validation");
                return View(model);
            }
            this.ModelState.AddModelError(string.Empty, "Login Failed");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        //registar através da conta Admin
        [Authorize(Roles ="Admin")]
        public IActionResult RegisterAsAdmin()
        {
            var model = new RegisterAsAdminViewModel
            {
                Countries = _countryRepository.GetComboCountries(),
                Cities = _countryRepository.GetComboCities(0),
                Roles = _roleRepository.GetComboRoles()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>RegisterAsAdmin(RegisterAsAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.RoleId == "0")
                {
                    model.Countries = _countryRepository.GetComboCountries();
                    model.Cities = _countryRepository.GetComboCities(model.CountryId);
                    model.Roles = _roleRepository.GetComboRoles();
                    ModelState.AddModelError("RoleId", "Please select a role...");
                    return View(model);
                }
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user == null)
                {
                    var city = await _countryRepository.GetCityAsync(model.CityId);
                    user = new User
                    {
                        FirstName = model.FirtsName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        CityId = city.Id,
                        City = city
                    };
                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        model.Countries = _countryRepository.GetComboCountries();
                        model.Cities = _countryRepository.GetComboCities(0);
                        model.Roles = _roleRepository.GetComboRoles();
                        ModelState.AddModelError(string.Empty, "The user could not be created");
                        return View(model);
                    }
                    
                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    string tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        token = myToken,

                    }, protocol: HttpContext.Request.Scheme);
                    var role = await _roleManager.FindByIdAsync(model.RoleId);
                    await _userHelper.AddUserToRoleAsync(user, role.Name);
                    Response response = _mailHelper.SendEmail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                       $"An Account as {role.Name} was created for you, " +
                        $"plase click in this link to confirm:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");

                    if (response.IsSuccess)
                    {
                        ViewBag.Message = $"The instructions has been send to {model.Username}";
                        return View(model);
                    }

                    ModelState.AddModelError(string.Empty, "The user could not be logged");

                }
                model.Cities = _countryRepository.GetComboCities(model.CountryId);
                model.Countries = _countryRepository.GetComboCountries();
                model.Roles= _roleRepository.GetComboRoles();
            }
            return View(model);
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Dashboards");
            }
            var model = new RegisterNewUserViewModel
            {
                Countries = _countryRepository.GetComboCountries(),
                Cities = _countryRepository.GetComboCities(0),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Dashboards");
            }
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user == null)
                {
                    var city = await _countryRepository.GetCityAsync(model.CityId);
                    user = new User
                    {
                        FirstName = model.FirtsName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        CityId = city.Id,
                        City = city
                    };
                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user could not be created");
                        return View(model);
                    }
                     await _userHelper.AddUserToRoleAsync(user, "Reader");
                    string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    string tokenLink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        token = myToken,

                    }, protocol: HttpContext.Request.Scheme);
                    Response response = _mailHelper.SendEmail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                        $"To allow the user, " +
                        $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");

                    if (response.IsSuccess)
                    {
                        ViewBag.Message = "The instructions to allow you user has been send to email";
                        return View(model);
                    }
                    ModelState.AddModelError(string.Empty, "The user could not be logged");

                }
                model.Cities = _countryRepository.GetComboCities(model.CountryId);
                model.Countries = _countryRepository.GetComboCountries();
            }
            return View(model);
        }



        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();
            if (user != null)
            {
                model.FirtsName = user.FirstName;
                model.LastName = user.LastName;
                model.Address = user.Address;
                model.PhoneNumber = user.PhoneNumber;
                model.TwoFactorAuthentication = user.TwoFactorEnabled;
                var city = await _countryRepository.GetCityAsync(user.CityId);
                if (city != null)
                {
                    var country = await _countryRepository.GetCountryAsync(city);
                    if (country != null)
                    {
                        model.CountryId = country.Id;
                        model.Cities = _countryRepository.GetComboCities(country.Id);
                        model.Countries = _countryRepository.GetComboCountries();
                        model.CityId = user.CityId;
                    }
                }
            }
            model.Cities = _countryRepository.GetComboCities(model.CountryId);
            model.Countries = _countryRepository.GetComboCountries();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var city = await _countryRepository.GetCityAsync(model.CityId);
                    user.FirstName = model.FirtsName;
                    user.LastName = model.LastName;
                    user.Address = model.Address;
                    user.PhoneNumber = model.PhoneNumber;
                    user.CityId = model.CityId;
                    user.City = city;
                    user.TwoFactorEnabled = model.TwoFactorAuthentication;

                    var response = await _userHelper.UpdateUserAsync(user);
                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "User Updated";
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
            }

            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found.");
                }
            }
            return this.View(model);
        }


        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.UserName);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);

                    }
                }
            }

            return BadRequest();
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }
            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }
            return View();
        }


        public IActionResult RecoverPassword()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                var link = this.Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendEmail(model.Email, "Free Rent Library Password Reset", $"<h1>Free Rent Library Password Reset</h1>" +
                $"To reset the password click in this link:</br></br>" +
                $"<a href = \"{link}\">Reset Password</a>");

                if (response.IsSuccess)
                {
                    this.ViewBag.Message = "The instructions to recover your password has been sent to email.";
                }

                return this.View();

            }

            return this.View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.UserName);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    this.ViewBag.Message = "Password reset successful.";
                    return View();
                }

                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            this.ViewBag.Message = "User not found.";
            return View(model);
        }





        public IActionResult NotAuthorized()
        {
            return View();
        }

        [HttpPost]
        [Route("Account/GetCitiesAsync")]
        public async Task<JsonResult> GetCitiesAsync(int countryId)
        {
            var country = await _countryRepository.GetCountryWithCityAsync(countryId);
            return Json(country.Cities.OrderBy(c => c.Name));
        }

    }
}
