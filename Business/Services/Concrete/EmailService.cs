using Business.Services.Abstract;
using Core.Interfaces;
using Core.Utilities.Helpers;
using Microsoft.Extensions.Configuration;
using Models.ViewModels;
using System.Net;
using System.Net.Mail;
namespace Business.Services.Concrete;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly IRazorViewRenderer _razorViewRenderer;
    public EmailService(IConfiguration configuration, IRazorViewRenderer razorViewRenderer)
    {
        _configuration = configuration;
        _razorViewRenderer = razorViewRenderer;
    }

    #region SMTP gönderici 
    public async Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
            {
                Port = int.Parse(_configuration["EmailSettings:Port"]),
                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:SenderEmail"],
                    _configuration["EmailSettings:SenderPassword"]),
                EnableSsl = bool.Parse(_configuration["EmailSettings:EnableSsl"]),
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:SenderEmail"], "EzyShop Support"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Email send failed: {ex.Message}");
            return false;
        }

    }
    #endregion
    public async Task<bool> SendSellerApprovedEmail(string to, string sellerName, string shopName)
    {
        var subject = "Your Application has been Approved!";

        var model = new SendSellerEmailViewModel
        {
            Name = sellerName,
            ShopName = shopName,
        };

        var notificationTemplatePath = "~/Areas/Admin/Views/Emails/_SendSellerApprovalTemplate.cshtml";
        var emailBody = await _razorViewRenderer.RenderViewToStringAsync(notificationTemplatePath, model);
        if (emailBody == null) { return false; }

        return await SendEmailAsync(to, subject, emailBody);
    }
    public async Task<bool> SendSellerRejectedEmail(string to, string sellerName, string shopName)
    {
        var subject = "Your Application has been rejected";

        var model = new SendSellerEmailViewModel
        {
            Name = sellerName,
            ShopName = shopName,
        };

        var notificationTemplatePath = "~/Areas/Admin/Views/Emails/_SendSellerRejectedTemplate.cshtml";
        var emailBody = await _razorViewRenderer.RenderViewToStringAsync(notificationTemplatePath, model);
        if (emailBody == null) { return false; }

        return await SendEmailAsync(to, subject, emailBody);
    }
    public async Task<bool> SendOrderConfirmationEmail(string to, string orderCode, string customerName)
    {
        var subject = $"Order Confirmation - {orderCode}";

        var replacements = new Dictionary<string, string>
        {
            { "CustomerName", customerName },
            { "OrderCode", orderCode }
        };

        var body = await EmailTemplateHelper.GetTemplateContentAsync("OrderConfirmation.html", replacements);

        return await SendEmailAsync(to, subject, body);
    }

    public async Task<bool> SendSellerDeactivatedEmail(string to, string sellerName, string shopName)
    {
        var subject = "Your account has been suspended";

        var model = new SendSellerEmailViewModel
        {
            Name = sellerName,
            ShopName = shopName,
        };

        var notificationTemplatePath = "~/Areas/Admin/Views/Emails/_SendSellerDeactivatedTemplate.cshtml";
        var emailBody = await _razorViewRenderer.RenderViewToStringAsync(notificationTemplatePath, model);
        if (emailBody == null) { return false; }

        return await SendEmailAsync(to, subject, emailBody);
    }
}
