using MediatR;
using MyBudgetManagement.Application.Common.Exceptions;
using MyBudgetManagement.Application.Features.Auth.Dtos;
using MyBudgetManagement.Application.Features.Auth.Interfaces;
using MyBudgetManagement.Application.Interfaces;
using MyBudgetManagement.Domain.Entities;
using MyBudgetManagement.Domain.Enums;

namespace MyBudgetManagement.Application.Features.Auth.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailService _mailService;

    public RegisterUserCommandHandler(IApplicationDbContext context, IJwtTokenService jwtTokenService, IPasswordHasher passwordHasher, IEmailService mailService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
        _mailService = mailService;
    }
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (_context.Users.Any(x => x.Email == request.Email))
            throw new ConflictException("Email already exists.");

        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = request.Email,
            FullName = request.FullName,
            Currency = Currencies.VND,
            Gender = Gender.Other,
            Status = Domain.Enums.AccountStatus.Pending,
            Created = DateTime.Now,
            CreatedBy = userId
        };

        user.PasswordHash = _passwordHasher.Hash(request.Password);

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var activationToken = _jwtTokenService.GenerateRandomStringToken();
        var token = new Token
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenValue = activationToken,
            Type = TokenType.ActivationToken,
            CreatedAt = DateTime.UtcNow,
            ExpireAt = DateTime.UtcNow.AddDays(7)
        };
        await _context.Tokens.AddAsync(token, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

 // Gửi email chào mừng sau khi đăng ký
            string subject = "Welcome to MyBudgetManagement!";
            string body = $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Registration Successful</title>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                    width: 100%;
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    padding: 20px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    text-align: center;
                    padding: 10px 0;
                    background-color: #4CAF50;
                    color: white;
                }}
                .content {{
                    padding: 20px;
                }}
                .footer {{
                    text-align: center;
                    padding: 10px 0;
                    background-color: #f4f4f4;
                    color: #666666;
                    font-size: 12px;
                }}
                h1 {{
                    color: #333333;
                }}
                p {{
                    color: #666666;
                    line-height: 1.5;
                }}
                .button {{
                    display: inline-block;
                    padding: 10px 20px;
                    margin: 20px 0;
                    font-size: 16px;
                    color: white;
                    background-color: #4CAF50;
                    text-decoration: none;
                    border-radius: 5px;
                }}
                .button:hover {{
                    background-color: #45a049;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>Welcome to Our Service!</h1>
                </div>
                <div class='content'>
                    <h1>Hi, {user.FullName}!</h1>
                    <p>We are excited to have you on board. Your account has been successfully created. You can now start using our service to enjoy the features and benefits we offer.</p>
                    <p>If you have any questions, feel free to contact our support team.</p>
                    <p>Before we can activate your account one last step must be taken to complete your registration.

                    Please note - you must complete this last step to become a registered member. You will only need to visit this URL once to activate your account.

                    To complete your registration, please click this button:</p>
                    <a href='https://localhost:7080/api/auths/activate?token={activationToken}' class='button'>Activate</a>
                </div>
                <div class='footer'>
                    <p>&copy; {DateTime.Now.Year} My BudgetManagement. All rights reserved.</p>
                    <p>Hanoi, Vietnam</p>
                </div>
            </div>
        </body>
        </html>";
            var emailBody = $$"""
                  <!DOCTYPE html>
                  <html xmlns:v="urn:schemas-microsoft-com:vml" xmlns:o="urn:schemas-microsoft-com:office:office" lang="en">
                  
                  <head>
                      <title>Registration Success</title>
                      <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
                      <meta name="viewport" content="width=device-width, initial-scale=1.0">
                      <link href="https://fonts.googleapis.com/css2?family=Abril+Fatface:wght@100;400;700" rel="stylesheet" type="text/css">
                      <style>
                          * {
                              box-sizing: border-box;
                          }
                  
                          body {
                              margin: 0;
                              padding: 0;
                              background-color: #f8f9fa; /* Màu nền tổng thể */
                              font-family: Arial, sans-serif;
                          }
                  
                          .container {
                              width: 100%;
                              max-width: 600px;
                              margin: 20px auto;
                              background-color: #ffffff; /* Màu nền thẻ */
                              border-radius: 8px;
                              box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
                          }
                  
                          .header {
                              background-color: #7747FF; /* Màu nền đầu thẻ */
                              color: #ffffff;
                              text-align: center;
                              padding: 20px;
                          }
                  
                          .header h1 {
                              margin: 0;
                              font-family: 'Abril Fatface', serif;
                              font-size: 36px;
                          }
                  
                          .image-block {
                              text-align: center;
                              padding: 20px;
                          }
                  
                          .image-block img {
                              width: 100%;
                              height: auto;
                              border-radius: 8px;
                          }
                  
                          .content {
                              padding: 20px;
                              color: #101112;
                          }
                  
                          .content p {
                              margin: 0 0 16px;
                              line-height: 1.5;
                          }
                  
                          .button {
                              display: inline-block;
                              background-color: #7747FF;
                              color: #ffffff;
                              padding: 10px 20px;
                              border-radius: 4px;
                              text-decoration: none;
                              font-weight: bold;
                              text-align: center;
                              margin: 20px auto; /* Căn giữa */
                          }
                  
                          .social-links {
                              text-align: center;
                              margin-top: 20px;
                          }
                  
                          .social-links img {
                              width: 32px;
                              height: auto;
                              margin: 0 5px;
                          }
                  
                          .footer {
                              text-align: center;
                              padding: 10px;
                              background-color: #f1f1f1;
                              font-size: 14px;
                              color: #101112;
                          }
                      </style>
                  </head>
                  
                  <body>
                      <div class="container">
                          <div class="header">
                              <h1>Welcome to MyBudgetManagement</h1>
                          </div>
                          <div class="image-block">
                              <img src="https://d15k2d11r6t6rl.cloudfront.net/pub/bfra/4qcmm84c/jvb/idn/5jx/1.png" alt="Budget Management">
                          </div>
                          <div class="content">
                              <p><strong>Hi User,</strong></p>
                              <p>Congratulations on starting your journey with MyBudgetManagement.</p>
                              <p>We are excited to have you on board. Your account has been successfully created. You can now start using our service to enjoy the features and benefits we offer.</p>
                              <p>If you have any questions, feel free to contact our support team.</p>
                              <div style="text-align: center;"> <!-- Thêm div này để căn giữa nút -->
                                  <a href="https://mybudgetmanagement.nguyenvanlinh.io.vn/login" class="button">Explore Now</a>
                              </div>
                          </div>
                          <div class="social-links">
                              <a href="https://www.facebook.com/nvl.1712" target="_blank">
                                  <img src="https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-only-logo-dark-gray/facebook@2x.png" alt="Facebook">
                              </a>
                              <a href="mailto:vanlinhnguyen1729@gmail.com?subject=Hi, I need your support!" target="_blank">
                                  <img src="https://app-rsrc.getbee.io/public/resources/social-networks-icon-sets/t-only-logo-dark-gray/mail@2x.png" alt="E-Mail">
                              </a>
                          </div>
                          <div class="footer">
                              <p>© 2025 MyBudgetManagement. All rights reserved.</p>
                              <p>Hanoi, Vietnam</p>
                          </div>
                      </div>
                  </body>
                  
                  </html>
                  
                  """;


            await _mailService.SendEmailAsync(user.Email, subject, emailBody);

        return userId;

    }
}