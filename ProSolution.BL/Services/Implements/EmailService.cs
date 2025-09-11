using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using ProSolution.BL.DTOs.ContactMessages;
using ProSolution.BL.Services.Interfaces;
using ProSolution.BL.Settings;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace ProSolution.BL.Services.Implements
{

    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendContactMessageAsync(ContactMessageDto contactDto)
        {
            // Письмо суперадмину
            var adminMessage = new MimeMessage();
            //adminMessage.From.Add(MailboxAddress.Parse(_smtpSettings.Username));
            adminMessage.From.Add(new MailboxAddress("ProSolution", _smtpSettings.Username));
            adminMessage.To.Add(MailboxAddress.Parse(_smtpSettings.AdminEmail));
            adminMessage.Subject = "📩Veb saytdan yeni mesaj daxil olub:";

            adminMessage.Body = new TextPart("html")
            {
                Text = $@"
                <h3>Əlaqə məlumatları:</h3>
                <p><strong>Ad:</strong> {contactDto.FirstName}</p>
                <p><strong>Soyad:</strong> {contactDto.LastName}</p>
                <p><strong>📧 E-poçt:</strong> {contactDto.Email}</p>
                <p><strong>📞 Telefon:</strong> {contactDto.PhoneNumber}</p>
                <p><strong>📨 Mesaj:</strong><br/>{contactDto.Message}</p>
            "
            };

            // Письмо пользователю
            var userMessage = new MimeMessage();
            userMessage.From.Add(new MailboxAddress("ProSolution", _smtpSettings.Username));


            userMessage.To.Add(MailboxAddress.Parse(contactDto.Email));
            userMessage.Subject = "✅ Mesajınız alındı.";

            userMessage.Body = new TextPart("html")
            {
                Text = $@"
                <h3>Salam, {contactDto.FirstName}!</h3>
                <p>Mesajınız alındı.</p>
                <p>Bizə müraciət etdiyiniz üçün təşəkkür edirik.</p>
                <p>Tezliklə sizinlə əlaqə saxlayacağıq.</p>
                <br/>
                <p>Hörmətlə,<br/>ProSolution komandasi</p>
            "
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);

            await smtp.SendAsync(adminMessage);
            await smtp.SendAsync(userMessage);

            await smtp.DisconnectAsync(true);
        }
        public async Task SendPasswordAsync(string email, string username, string password)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ProSolution", _smtpSettings.Username));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "🛡️ Şifrəniz və istifadəçi məlumatları";

            message.Body = new TextPart("html")
            {
                Text = $@"
            <h3>Salam, {username}!</h3>
            <p>Sizin üçün avtomatik şifrə yaradıldı:</p>
            <p><strong>İstifadəçi adı:</strong> {username}</p>
            <p><strong>Şifrə:</strong> {password}</p>
            <p>Xahiş edirik ilk girişdən sonra şifrənizi dəyişin.</p>
        "
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
        public async Task SendHtmlEmailAsync(string to, string subject, string htmlContent)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("ProSolution", _smtpSettings.Username));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlContent };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }


    }


}
