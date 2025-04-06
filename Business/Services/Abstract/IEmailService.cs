namespace Business.Services.Abstract
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
        Task<bool> SendSellerApprovedEmail(string to, string sellerName);
        Task<bool> SendOrderConfirmationEmail(string to, string orderCode, string customerName);

    }
}
