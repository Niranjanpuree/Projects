using Northwind.Core.Import.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.Core.Import.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class TaskOrderNumberValidator : ValidationAttribute
    {
        protected override System.ComponentModel.DataAnnotations.ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (ContractModel)validationContext.ObjectInstance;
            if (model.IsTaskOrder && string.IsNullOrWhiteSpace(model.TaskOrderNumber))
            {
                return new System.ComponentModel.DataAnnotations.ValidationResult("Task Order number is required");
            }

            return System.ComponentModel.DataAnnotations.ValidationResult.Success;
        }
    }
}
