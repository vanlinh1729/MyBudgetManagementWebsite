using System.Security.Cryptography;
using AutoMapper;
using Hangfire;
using MyBudgetManagement.AppService.MD5Service;
using MyBudgetManagement.Dtos.Users;
using MyBudgetManagement.Models;
using MyBudgetManagement.Repositories;

namespace MyBudgetManagement.AppService.UserAppService;


public class UserAppService : IUserAppService
{
    private readonly IMapper _mapper;
    private readonly MailService _mailService;
    private readonly IUserRepository _userRepository;

    public UserAppService(IMapper mapper, MailService mailService, IUserRepository userRepository)
    {
        _mapper = mapper;
        _mailService = mailService;
        _userRepository = userRepository;
    }

    public async Task<UserDto> GetUserById(Guid id)
    {
       var user = await _userRepository.GetUserById(id);
       return _mapper.Map<UserDto>(user);
    }

    public async Task<List<UserDto>> GetAllUser()
    {
        var listUsers = await _userRepository.GetAllUsers();
        return _mapper.Map<List<UserDto>>(listUsers);
    }

    public async Task<UserDto> GetUserByEmail(string email)
    {
       var user = await _userRepository.GetUserByEmail(email);
       if (user.Id != Guid.Empty)
       {
           return _mapper.Map<UserDto>(user);

       }

       return new UserDto();
    }

    public List<Role> GetRoleByUserId(Guid id)
    {
        var listRoles= _userRepository.GetUserRoleByUserId(id);
        return listRoles;
    }

    public async Task<bool> IsAuthenticated(string email, string password)
    {
        var isAuthenticated = await _userRepository.CheckPasswordAsync(email, Md5Encrypt.Encrypt(password));
        return isAuthenticated;
    }

    public async Task<UserDto> RegisterAccount(User userModel)
    {
        userModel.Id = Guid.NewGuid();
        userModel.Password = Md5Encrypt.Encrypt(userModel.Password);
        var user = await _userRepository.CreateOrUpdateUser(userModel);
        if (user.Id != Guid.Empty)
        {
            string fromName = "My Budget Management";
            string fromEmail = "searchm5v@gmail.com";
            string toName = user.FirstName + " " + user.LastName;
            string toEmail = user.Email;
            string subject = "Register successfully!";
            string body =$@"
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
                    <h1>Hi, {user.FirstName}!</h1>
                    <p>We are excited to have you on board. Your account has been successfully created. You can now start using our service to enjoy the features and benefits we offer.</p>
                    <p>If you have any questions, feel free to contact our support team.</p>
                    <a href='https://mybudgetmanagement.nguyenvanlinh.io.vn/login' class='button'>Log in to your account</a>
                </div>
                <div class='footer'>
                    <p>&copy; 2024 My BudgetManagemeny. All rights reserved.</p>
                    <p>Hanoi, Vietnam</p>
                </div>
            </div>
        </body>
        </html>";
            // Enqueue một công việc gửi email
            BackgroundJob.Enqueue(() => _mailService.SendEmail(fromName, fromEmail, toName, toEmail, subject, body, true));
            return _mapper.Map<UserDto>(user);

        }
        return new UserDto();
    }
    
    public async Task<string> EverydayMailing()
    {
        try
        { 
            var listUsers = GetAllUser().Result;
            foreach (var user in listUsers)
        {
            Console.WriteLine("sending mail to "+user.Email);
            var fromName = "My Budget Management";
            var fromEmail = "searchm5v@gmail.com";
            var toName = user.FirstName + " " + user.LastName;
            var toEmail = user.Email;
            string subject = "Good Morning from MyBudget Management!";
            string body = $@"
    <!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Good Morning from MyBudget Management</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .container {{
            background-color: #ffffff;
            margin: 50px auto;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            max-width: 600px;
        }}
        .header {{
            text-align: center;
            padding: 20px 0;
        }}
        .header h1 {{
            margin: 0;
            color: #333333;
        }}
        .content {{
            text-align: left;
            color: #555555;
        }}
        .content p {{
            line-height: 1.6;
        }}
        .footer {{
            text-align: center;
            padding: 20px 0;
            color: #999999;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h1>Good Morning from My Budget Management!</h1>
        </div>
        <div class=""content"">
            <p>Dear Valued Customer,</p>
            <p>Good morning!</p>
            <p>We hope you had a restful night and are ready for a productive day. Start your day right by logging in to My Budget Management and taking control of your finances. Our latest features and tools are designed to help you manage your budget more effectively.</p>
            <p>If you have any questions or need assistance, our support team is always here to help.</p>
            <p>Have a wonderful day!</p>
        </div>
            <div class='footer'>
                 <p>Best regards,</p>
                 <p>&copy; 2024 My BudgetManagement. All rights reserved.</p>
                    <p>Hanoi, Vietnam</p>
            </div>
        </div>
    </body>
    </html>";
            BackgroundJob.Enqueue(() =>
                _mailService.SendEmail(fromName, fromEmail, toName, toEmail, subject, body, true));
        }
            return "Success mailing to everyone!";
        }
        catch (Exception e)
        {
            return e.Message;
        }
}
}
