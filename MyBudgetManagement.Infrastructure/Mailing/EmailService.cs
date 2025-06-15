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
    var subject = "Welcome to MyBudgetManagement - Activate Your Account";
    var body = $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <title>Activate Your Account</title>
        <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <link href=""https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap"" rel=""stylesheet"">
        <style>
            * {{
                box-sizing: border-box;
                margin: 0;
                padding: 0;
            }}
            
            body {{
                font-family: 'Inter', Arial, sans-serif;
                background-color: #f5f7fa;
                color: #2d3748;
                line-height: 1.6;
            }}
            
            .container {{
                max-width: 600px;
                margin: 20px auto;
                background: #ffffff;
                border-radius: 12px;
                overflow: hidden;
                box-shadow: 0 4px 24px rgba(0, 0, 0, 0.08);
            }}
            
            .header {{
                background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                padding: 40px 20px;
                text-align: center;
                color: white;
            }}
            
            .header h1 {{
                font-size: 28px;
                font-weight: 700;
                margin-bottom: 10px;
            }}
            
            .header p {{
                font-size: 16px;
                opacity: 0.9;
            }}
            
            .logo {{
                font-size: 24px;
                font-weight: 700;
                margin-bottom: 15px;
                display: inline-block;
            }}
            
            .content {{
                padding: 30px;
            }}
            
            .greeting {{
                font-size: 20px;
                font-weight: 600;
                margin-bottom: 20px;
                color: #1a202c;
            }}
            
            .message {{
                margin-bottom: 25px;
                font-size: 15px;
                color: #4a5568;
            }}
            
            .cta-container {{
                text-align: center;
                margin: 30px 0;
            }}
            
            .cta-button {{
                display: inline-block;
                background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                color: white !important;
                text-decoration: none;
                font-weight: 600;
                padding: 14px 28px;
                border-radius: 8px;
                font-size: 16px;
                box-shadow: 0 4px 12px rgba(102, 126, 234, 0.3);
                transition: all 0.3s ease;
            }}
            
            .cta-button:hover {{
                transform: translateY(-2px);
                box-shadow: 0 6px 16px rgba(102, 126, 234, 0.4);
            }}
            
            .image-block {{
                margin: 25px 0;
                text-align: center;
            }}
            
            .image-block img {{
                max-width: 100%;
                border-radius: 8px;
                height: auto;
            }}
            
            .social-links {{
                text-align: center;
                margin: 30px 0;
            }}
            
            .social-links a {{
                display: inline-block;
                margin: 0 10px;
                transition: transform 0.3s ease;
            }}
            
            .social-links a:hover {{
                transform: scale(1.1);
            }}
            
            .social-links img {{
                width: 32px;
                height: 32px;
            }}
            
            .footer {{
                text-align: center;
                padding: 20px;
                background-color: #edf2f7;
                font-size: 13px;
                color: #718096;
            }}
            
            .footer p {{
                margin-bottom: 8px;
            }}
            
            .highlight {{
                font-weight: 600;
                color: #5a67d8;
            }}
        </style>
    </head>
    <body>
        <div class='container'>
            <div class='header'>
                <div class='logo'>MyBudgetManagement</div>
                <h1>Welcome Aboard!</h1>
                <p>Let's get your account activated</p>
            </div>
            
            <div class='content'>
                <div class='greeting'>Hi, {fullName}!</div>
                
                <div class='message'>
                    <p>Thank you for joining MyBudgetManagement! We're excited to have you on board and can't wait to see how you'll use our platform to take control of your finances.</p>
                    
                    <p>Before you can start using all the features, we need to verify your email address. This helps us keep your account secure.</p>
                    
                    <p class='highlight'>Please click the button below to activate your account:</p>
                </div>
                
                <div class='cta-container'>
                    <a href='https://localhost:7080/api/auth/activate?token={activateToken}' class='cta-button'>Activate My Account</a>
                </div>
                
                <div class='message'>
                    <p>If you didn't request this account, please ignore this email or contact our support team.</p>
                </div>
                
                <div class='image-block'>
                    <img src='https://d15k2d11r6t6rl.cloudfront.net/pub/bfra/4qcmm84c/jvb/idn/5jx/1.png' alt='Personal Finance Management'>
                </div>
                
                <div class='social-links'>
                    <p>Connect with us:</p>
                    <a href='https://www.facebook.com/nvl.1712' target='_blank'>
                        <img src='https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-only-logo-dark-gray/facebook@2x.png' alt='Facebook'>
                    </a>
                    <a href='mailto:vanlinhnguyen1729@gmail.com?subject=Support Request' target='_blank'>
                        <img src='https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-only-logo-dark-gray/mail@2x.png' alt='Email Support'>
                    </a>
                </div>
            </div>
            
            <div class='footer'>
                <p>&copy; {DateTime.Now.Year} MyBudgetManagement. All rights reserved.</p>
                <p>Hanoi, Vietnam</p>
                <p>You're receiving this email because you signed up for a MyBudgetManagement account.</p>
            </div>
        </div>
    </body>
    </html>";

    await SendEmailAsync(to, subject, body);
}
}