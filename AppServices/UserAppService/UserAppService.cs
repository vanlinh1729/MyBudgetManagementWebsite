using System.Security.Cryptography;
using AutoMapper;
using MyBudgetManagement.AppService.MD5Service;
using MyBudgetManagement.Dtos.Users;
using MyBudgetManagement.Models;
using MyBudgetManagement.Repositories;

namespace MyBudgetManagement.AppService.UserAppService;


public class UserAppService : IUserAppService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserAppService(IMapper mapper,IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }
    
    public async Task<UserDto> GetUserById(Guid id)
    {
       var user = await _userRepository.GetUserById(id);
       return _mapper.Map<UserDto>(user);
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
            string smtpServer = "smtp.gmail.com";
            int port = 465;
            string username = "searchm5v@gmail.com";
            string password = "dnbklsyltyscqial";
            bool enableSsl = true;

            MailService mailService = new MailService(smtpServer, port, username, password, enableSsl);

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
            mailService.SendEmail(fromName, fromEmail, toName, toEmail, subject, body, true); // Đặt 'true' để chỉ định nội dung HTML
            return _mapper.Map<UserDto>(user);

        }
        return new UserDto();
    }
}
