using System;
using System.ComponentModel.DataAnnotations;

namespace FreeRentLibrary.Helpers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class StringRequiredAttribute : ValidationAttribute
    {
        public StringRequiredAttribute()
        {
            ErrorMessage = "The {0} field is required.";
        }

        public override bool IsValid(object value)
        {
            return value != null && !string.IsNullOrWhiteSpace(value.ToString());
        }
    }
}
