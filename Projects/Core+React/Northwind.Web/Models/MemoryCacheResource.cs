using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models
{
    public class MemoryCacheResource
    {
        public static String PaymentTerms = "PaymentTerms";
        public static String ContractType = "ContractType";
        public static String InvoiceSubmissionMethod = "InvoiceSubmissionMethod";
        public static String NAICSCode = "NAICSCode";
        public static String PSCCode = "PSCCode";
        public static String Country = "Country";
        public static String Currency = "Currency";
        
        public static IEnumerable<Core.Entities.ResourceAttributeValue> GetPaymentTermsOptions(IMemoryCache cache)
        {
            return cache.Get<IEnumerable<Core.Entities.ResourceAttributeValue>>(Models.MemoryCacheResource.PaymentTerms);
        }

        public static string GetPaymentTermsLabel(string value, IMemoryCache cache)
        {
            var paymentTerms = cache.Get<IEnumerable<Core.Entities.ResourceAttributeValue>>(Models.MemoryCacheResource.PaymentTerms);
            foreach(var item in paymentTerms)
            {
                if (item.Value.ToLower() == value.ToLower())
                {
                    return item.Name;
                }
            }
            return "";
        }

        public static IEnumerable<Core.Entities.ResourceAttributeValue> GetCurrencyOptions(IMemoryCache cache)
        {
            return cache.Get<IEnumerable<Core.Entities.ResourceAttributeValue>>(Models.MemoryCacheResource.Currency);
        }

        public static string GetCurrencyLabel(string value, IMemoryCache cache)
        {
            var currencies = cache.Get<IEnumerable<Core.Entities.ResourceAttributeValue>>(Models.MemoryCacheResource.Currency);
            //if(currencies == null)
            //    cache.Set
            foreach (var item in currencies)
            {
                if (item.Value.ToLower() == value.ToLower())
                {
                    return item.Name;
                }
            }
            return "";
        }

        public static IEnumerable<Core.Entities.Psc> GetPSCs(IMemoryCache cache)
        {
            return cache.Get<IEnumerable<Core.Entities.Psc>>(Models.MemoryCacheResource.PSCCode);
        }

        public static string GetPscDescription(string value, IMemoryCache cache)
        {
            var pscs = cache.Get<IEnumerable<Core.Entities.Psc>>(Models.MemoryCacheResource.PSCCode);
            foreach (var item in pscs)
            {
                if (item.Code.ToLower() == value.ToLower())
                {
                    return item.CodeDescription;
                }
            }
            return "";
        }
    }
}
