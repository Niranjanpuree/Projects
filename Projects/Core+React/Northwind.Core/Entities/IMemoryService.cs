using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public abstract class IMemoryService<T>
    {
        public static String PaymentTerms = "PaymentTerms";
        public static String ContractType = "ContractType";
        public static String InvoiceSubmissionMethod = "InvoiceSubmissionMethod";
        public static String NAICSCode = "NAICSCode";
        public static String PSCCode = "PSCCode";
        public static String Country = "Country";
        public static String Currency = "Currency";
        public static string ResourceAttribute = "ResourceAttribute";
        public static string State = "State";

        private readonly IMemoryCache _memory;
        private readonly string _resource;
        private readonly int _expiration;
        public IMemoryService(IMemoryCache memory,string resource, int expiration)
        {
            _memory = memory;
            _resource = resource;
            _expiration = expiration;            
        }

        protected T GetMemoryData()
        {
            T output = default(T);
            _memory.TryGetValue<T>(_resource, out output);
            return output;
        }

        protected IEnumerable<T> GetMemoryList()
        {
            IEnumerable<T> output = null;
            _memory.TryGetValue<IEnumerable<T>>(_resource, out output);
            return output;
        }

        protected void setMemoryData(T input)
        {
            _memory.Set<T>(_resource, input, TimeSpan.FromSeconds(_expiration));
        }

        protected void setMemoryList(IEnumerable<T> input)
        {
            _memory.Set<IEnumerable<T>>(_resource, input, TimeSpan.FromSeconds(_expiration));
        }
    }
}
