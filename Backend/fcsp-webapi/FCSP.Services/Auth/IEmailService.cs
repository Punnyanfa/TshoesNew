using System.Threading.Tasks;

namespace FCSP.Services.Auth;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = false);
} 