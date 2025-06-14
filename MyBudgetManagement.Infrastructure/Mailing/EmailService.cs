using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using MyBudgetManagement.Application.Interfaces;

namespace MyBudgetManagement.Infrastructure.EmailService;

public class EmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUsername;
    private readonly string _smtpPassword;
    private readonly bool _enableSsl;

    public EmailService(IConfiguration configuration)
    {
        var emailConfig = configuration.GetSection("EmailSettings");
        _smtpServer = emailConfig["SmtpServer"];
        _smtpPort = int.Parse(emailConfig["SmtpPort"]);
        _smtpUsername = emailConfig["SmtpUsername"];
        _smtpPassword = emailConfig["SmtpPassword"];
        _enableSsl = bool.Parse(emailConfig["EnableSsl"]);
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_smtpUsername));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        message.Body = new BodyBuilder
        {
            HtmlBody = body
        }.ToMessageBody();

        using var smtp = new SmtpClient();

        try
        {
            Console.WriteLine($"SMTP: {_smtpServer}:{_smtpPort} - {_smtpUsername}");
            await smtp.ConnectAsync(_smtpServer, _smtpPort, _enableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto);
            await smtp.AuthenticateAsync(_smtpUsername, _smtpPassword);
            await smtp.SendAsync(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Email send failed: {ex.Message}");
            throw;
        }
        finally
        {
            await smtp.DisconnectAsync(true);
        }
    }

    public async Task SendActivateEmailAsync(string to, string fullName, string activateToken)
    {
         // Gửi email chào mừng sau khi đăng ký
        var subject = "Welcome to MyBudgetManagement!";
        var body = $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
    <title>Registration Success</title>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <link href=""https://fonts.googleapis.com/css2?family=Abril+Fatface:wght@100;400;700"" rel=""stylesheet"" type=""text/css"">
    <style>
        * {{
            box-sizing: border-box;
        }}

        body {{
            margin: 0;
            padding: 0;
            background-color: #f8f9fa; /* Màu nền tổng thể */
            font-family: Arial, sans-serif;
        }}

        .container {{
            width: 100%;
            max-width: 600px;
            margin: 20px auto;
            background-color: #ffffff; /* Màu nền thẻ */
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        }}

        .header {{
            background-color: #7747FF; /* Màu nền đầu thẻ */
            color: #ffffff;
            text-align: center;
            padding: 20px;
        }}

        .header h1 {{
            margin: 0;
            font-family: 'Abril Fatface', serif;
            font-size: 36px;
        }}

        .image-block {{
            text-align: center;
            padding: 20px;
        }}

        .image-block img {{
            width: 100%;
            height: auto;
            border-radius: 8px;
        }}

        .content {{
            padding: 20px;
            color: #101112;
        }}

        .content p {{
            margin: 0 0 16px;
            line-height: 1.5;
        }}

        .button {{
            display: inline-block;
            background-color: #7747FF;
            color: #ffffff;
            padding: 10px 20px;
            border-radius: 4px;
            text-decoration: none;
            font-weight: bold;
            text-align: center;
            margin: 20px auto; /* Căn giữa */
        }}

        .social-links {{
            text-align: center;
            margin-top: 20px;
        }}

        .social-links img {{
            width: 32px;
            height: auto;
            margin: 0 5px;
        }}

        .footer {{
            text-align: center;
            padding: 10px;
            background-color: #f1f1f1;
            font-size: 14px;
            color: #101112;
        }}
    </style>
</head>

        <body>
            <div class='container'>
                <div class=""header"">
            <h1>Welcome to MyBudgetManagement</h1>
        </div>
        <div class=""image-block"">
            <img src=""https://d15k2d11r6t6rl.cloudfront.net/pub/bfra/4qcmm84c/jvb/idn/5jx/1.png"" alt=""Budget Management"">
        </div>
                <div class='content'>
                    <h1>Hi, {fullName}!</h1>
                    <p>We are excited to have you on board. Your account has been successfully created. You can now start using our service to enjoy the features and benefits we offer.</p>
                    <p>If you have any questions, feel free to contact our support team.</p>
                    <p>Before we can activate your account one last step must be taken to complete your registration.

                    Please note - you must complete this last step to become a registered member. You will only need to visit this URL once to activate your account.

                    To complete your registration, please click this button:</p>
<div style=""text-align: center;""> <!-- Thêm div này để căn giữa nút -->

                    <a href='https://localhost:7080/api/auths/activate?token={activateToken}' class='button'>Activate</a>
/div>
                </div>
<div class=""social-links"">
            <a href=""https://www.facebook.com/nvl.1712"" target=""_blank"">
                <img src=""https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-only-logo-dark-gray/facebook@2x.png"" alt=""Facebook"">
            </a>
            <a href=""mailto:vanlinhnguyen1729@gmail.com?subject=Hi, I need your support!"" target=""_blank"">
                <img src=""https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-only-logo-dark-gray/mail@2x.png"" alt=""E-Mail"">
            </a>
        </div>
                <div class='footer'>
                    <p>&copy; {DateTime.Now.Year} My BudgetManagement. All rights reserved.</p>
                    <p>Hanoi, Vietnam</p>
                </div>
            </div>
        </body>
        </html>";

        await SendEmailAsync(to, subject, body);
    }
}