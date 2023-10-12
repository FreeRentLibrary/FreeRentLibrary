using System.Collections.Generic;

namespace FreeRentLibrary.Data.API
{
    public class JCountry
    {
        public string Name { get; set; }

        public List<JState> States { get; set; }
    }
}
