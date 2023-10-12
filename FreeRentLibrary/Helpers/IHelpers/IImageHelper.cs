using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;


namespace FreeRentLibrary.Helpers.IHelpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);
    }
}
