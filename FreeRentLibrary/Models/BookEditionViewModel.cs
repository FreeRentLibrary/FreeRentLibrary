using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using FreeRentLibrary.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Xunit.Sdk;

namespace FreeRentLibrary.Models
{
    public class BookEditionViewModel : BookEdition
    {
        [Display(Name = "Cover")]
        public IFormFile ImageFile { get; set; }

        public IEnumerable<SelectListItem> BookTypes { get; set; }

        public IEnumerable<SelectListItem> BookPublishers { get; set; }

        public bool SameBookName { get; set; }
    }
}
