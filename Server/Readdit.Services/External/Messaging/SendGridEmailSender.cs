using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Readdit.Services.External.Messaging;

public class SendGridEmailSender : IEmailSender
{
    private readonly ISendGridClient _client;
    private readonly ILogger<SendGridEmailSender> _logger;

    public SendGridEmailSender(
        ISendGridClient client,
        ILogger<SendGridEmailSender> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task SendEmailAsync(
        string from,
        string fromName,
        string to,
        string subject,
        string htmlContent,
        IEnumerable<EmailAttachment>? attachments = null)
    {
        if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlContent))
        {
            throw new ArgumentException("Subject and message are required.");
        }

        var fromAddress = new EmailAddress(from, fromName);
        var toAddress = new EmailAddress(to);
        var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, null, htmlContent);

        attachments = attachments?.ToList();
        if (attachments is not null && attachments.Any())
        {
            foreach (var attachment in attachments)
            {
                message.AddAttachment(
                    attachment.FileName,
                    Convert.ToBase64String(attachment.Content),
                    attachment.MimeType);
            }
        }
            
        try
        {
            var response = await _client.SendEmailAsync(message);
            _logger.LogInformation(response.StatusCode.ToString());

            var bodyAsString = await response.Body.ReadAsStringAsync();
            _logger.LogInformation(bodyAsString);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}