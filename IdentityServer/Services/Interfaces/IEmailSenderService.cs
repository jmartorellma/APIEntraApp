using IdentityServer.Models;
using System.Threading.Tasks;

namespace IdentityServer.Services.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(EmailMessage message);
    }
}
