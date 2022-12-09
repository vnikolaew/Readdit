namespace Readdit.Services.External.Messaging;

public class NoopEmailSender : IEmailSender
{
    public Task SendEmailAsync(
        string from,
        string fromName,
        string to,
        string subject,
        string htmlContent,
        IEnumerable<EmailAttachment>? attachments = null)
        => Task.CompletedTask;
}