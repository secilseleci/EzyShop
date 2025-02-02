namespace Business.Services.Abstract
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);

    }
}
