using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Data.Entities
{
    public class User : IdentityUser
    {
        [Display(Name = "Name")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string FirstName { get; set; }

        [Display(Name = "Surname")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string LastName { get; set; }

        //[Display(Name = "Profile Picture")]
        //public Guid ProfilePicId { get; set; }

        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string Address { get; set; }

        [Display(Name = "Phone Number")]
        [MaxLength(15, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string PhoneNumber { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }
        public bool TwoFactorEnabled { get; set; }

        /* In Case of District, County and Parish Combo Boxes
        public int CountyId { get; set; }
        public County County { get; set; }

        public int ParishId { get; set; }
        public Parish Parish { get; set; }
        */

        /* In Case of Approvals
        [Display(Name = "Employee Verified")]
        public bool EmployeeApproved { get; set; }

        [Display(Name = "Admin Verified")]
        public bool AdminApproved { get; set; }
        */

        public IEnumerable<Rent> Rentals { get; set; }

        public IEnumerable<Reservation> Reservations { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        //public string ImageFullPath => ProfilePicId == Guid.Empty
        //    ? $"https://frlcontainer.blob.core.windows.net/default/noimage.png"
        //    : $"https://frlcontainer.blob.core.windows.net/users/{ProfilePicId}";
    }
}
