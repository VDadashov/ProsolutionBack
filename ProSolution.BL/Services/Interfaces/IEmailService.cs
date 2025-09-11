using ProSolution.BL.DTOs.ContactMessages;

namespace ProSolution.BL.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendContactMessageAsync(ContactMessageDto contactDto);

        Task SendPasswordAsync(string email, string username, string password);

        Task SendHtmlEmailAsync(string to, string subject, string htmlContent);

    }
}
