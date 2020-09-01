using System;
using System.Threading.Tasks;

namespace Northwind.Core.Interfaces
{
    public interface IEmailSender
    {
        Task<Boolean> SendEmailAsync(string email, string recipientName, string subject, string message);
    }
}
