using FreeRentLibrary.Helpers.SimpleHelpers;

namespace FreeRentLibrary.Helpers.IHelpers
{
    public interface IMailHelper
    {
        Response SendEmail(string to, string Subject, string body);
    }
}
