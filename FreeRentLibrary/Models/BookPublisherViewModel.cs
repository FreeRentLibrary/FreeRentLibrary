using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Xunit.Sdk;

namespace FreeRentLibrary.Models
{
    public class BookPublisherViewModel : BookPublisher
    {
        public IEnumerable<SelectListItem> Countries { get; set; }
    }
}
