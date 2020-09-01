using System;

namespace AutoCare.Product.Application.Infrastructure.ExceptionHelpers
{
    public class ChangeRequestExistException : Exception
    {
        public ChangeRequestExistException(string message)
            :base(message)
        {
        }
    }
}
