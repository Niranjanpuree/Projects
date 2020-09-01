using Northwind.Core.Import.Interface;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Services.ContractRefactor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.Core.Import.Validation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class ContractDuplicateValidatorAttribute : ValidationAttribute
    {
        //public override bool IsValid(object value)
        //{
        //    var contractNumber = value as string;
        //    var isValid = true;
        //    if (!string.IsNullOrWhiteSpace(contractNumber))
        //    {
               
        //        var c = _contractsService.CheckDuplicateContractNumber();
        //        isValid = contractNumber.ToUpperInvariant() != "123";
        //    }
        //    return isValid;
        //}

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var contractNumber = value as string;
                var _service = (IContractsService) validationContext
                          .GetService(typeof(IContractsService)) as IContractsService;
                //if (_service == null)
                //var c = _service.GetContractNumberById(Guid.Parse("059EABF9-DE15-40BB-A78A-B9E811E4C0F5"));
                //if (contractNumber != "123")
                //{
                //    return ValidationResult.Success;
                //}
                //else
                //{
                //    return new ValidationResult
                //        ("Duplicate Value");
                //}

                return ValidationResult.Success;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
