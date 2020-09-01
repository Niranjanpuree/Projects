using System;

namespace AutoCare.Product.Infrastructure.ExceptionHandler
{
    public class NoRecordFound : Exception
    {
        public NoRecordFound(string message)
            :base(message)
        {
        }
    }
}