using FreeRentLibrary.Data.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeRentLibrary.Controllers.API
{
    //TODO: Delete Controller/Create another controller
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class ProductsController : Controller
    {
        private readonly IBookCNCRepository _productRepository;

        public ProductsController(IBookCNCRepository productRepository)
        {
            _productRepository = productRepository;
        }
        
        [HttpGet]
        public IActionResult GetProducts()
        {
            //return Ok(_productRepository.GetAllWithUsers());
            return View();
        }
    }
}
