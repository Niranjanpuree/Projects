using System;

namespace AutoCare.Product.Infrastructure.ExceptionHandler
{
    public class RecordAlreadyExist : Exception
    {
        public RecordAlreadyExist(string message)
            :base(message)
        {
            
        }
    }
}
