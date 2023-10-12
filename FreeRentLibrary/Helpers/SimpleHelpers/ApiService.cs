using FreeRentLibrary.Data.API;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json.Linq;

namespace FreeRentLibrary.Helpers.SimpleHelpers
{
    public class ApiService
    {
        public static async Task<Response> GetCountries()
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://countriesnow.space");

                var response = await client.GetAsync("/api/v0.1/countries/states");

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response { IsSuccess = false, Message = "Failed to get countries." };
                }

                JObject jObject = JObject.Parse(result);

                if (jObject["data"] != null)
                {
                    JArray jArray = jObject["data"].Value<JArray>();
                    
                    var countries = jArray.ToObject<List<JCountry>>();

                    return new Response { IsSuccess = true, Message = "Got countries successfully.", Results = countries };
                }
                //var countries = JsonConvert.DeserializeObject<List<JCountry>>(result);
                else
                {
                    return new Response { IsSuccess = false, Message = "API response format is not as expected." };

                }
            }
            catch (Exception ex)
            {
                return new Response { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
