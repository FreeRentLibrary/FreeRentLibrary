﻿using FreeRentLibrary.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace FreeRentLibrary.Models
{
    public class RegisterAsAdminViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirtsName { get; set; }


        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Remote(action: "IsEmailInUse", controller: "Account")]
        public string Username { get; set; }

        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length")]
        public string Address { get; set; }

        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length")]
        public string PhoneNumber { get; set; }

        [Display(Name = "City")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a city")]
        public int CityId { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }

        [Display(Name = "Country")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a country")]
        public int CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
        //TODO: verificar aceitação de roles
        [Required]
        [StringRequired(ErrorMessage = "You must select a role")]
        [Display(Name = "Function")]
        public string RoleId { get; set; }


        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }
    }
}
