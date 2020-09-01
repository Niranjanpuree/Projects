using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.Core.Import.Helper
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class Trim : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            validationContext
                 .ObjectType
                 .GetProperty(validationContext.MemberName)
                 .SetValue(validationContext.ObjectInstance, value.ToString().Trim().ToLower(), null);

            return System.ComponentModel.DataAnnotations.ValidationResult.Success;
        }
    }
}
