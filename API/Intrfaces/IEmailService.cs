
namespace API.Interfaces;

public interface IEmailService
{
    Task SendFromFamilyAppAsync(string to, string subject, string body);
}
