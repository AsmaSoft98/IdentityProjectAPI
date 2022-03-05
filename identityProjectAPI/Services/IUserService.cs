using identityProjectAPI.Data;
using identityProjectAPI.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SharedProject.Requests;
using SharedProject.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace identityProjectAPI.Services
{
    public interface IUserService
    {
        Task<ResultResponse> RegisterUserAsync(RegisterRequest model);
        Task<UserManagerResponse> GenerateTokenAsync(LoginRequest model);
        Task<UserManagerResponse> ForgetPasswordAsync(string email);
        Task<ResultResponse> ChangePassword(ResetPasswordRequest model);
    }
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppConfiguration _appConfiguration;
        private readonly IConfiguration _configuration;
        public UserService(AppConfiguration appConfiguration,
                IConfiguration configuration,
                UserManager<ApplicationUser> userManager, 
                SignInManager<ApplicationUser> signInManager)
        {
            _appConfiguration = appConfiguration;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public async Task<ResultResponse> RegisterUserAsync(RegisterRequest model)
        {
            var userByEmail = await _userManager.FindByEmailAsync(model.Email);
            if (userByEmail != null)
                return new ResultResponse
                {
                    Message = "User already exists", 
                    IsSuccess = false
                };

            var user = new ApplicationUser // TODO mapper for this
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };

            await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, "USER"); // default new user to User role
            return new ResultResponse
            {
                Message = "Welcome",
                IsSuccess = true
            };
        }
        public async Task<UserManagerResponse> GenerateTokenAsync(LoginRequest model)
        {
            var result = await _signInManager.PasswordSignInAsync(
                           model.Email,
                           model.Password,
                           isPersistent: false,
                           lockoutOnFailure: false);
            if (!result.Succeeded)
            return new UserManagerResponse
            {
                Message = "Login failed!",
                IsSuccess = false
            };
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, model.Email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfiguration.Key));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var Expiry = DateTime.Now.AddDays(Convert.ToInt32(_appConfiguration.ExpiryInDays));

            var token = new JwtSecurityToken(
                _appConfiguration.Issuer,
                _appConfiguration.Audience,
                claims,
                expires: Expiry,
                signingCredentials: credentials
            );
            
            var tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
            var response = new UserManagerResponse
            {
                AccessToken = tokenAsString,
                Message = "Logged on",
                ExpiryDate = Expiry,
                IsSuccess = true
            };
            return response;
        }

        public async Task<UserManagerResponse> ForgetPasswordAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return new UserManagerResponse
                {
                    Message = "email is empty!",
                    IsSuccess = false
                };

            var userByEmail = await _userManager.FindByEmailAsync(email);
            if (userByEmail == null)
                return new UserManagerResponse
                {
                     Message = "No user associated with email",
                     IsSuccess = false
                };
            //add service EmailSender
            //var token = await _userManager.GeneratePasswordResetTokenAsync(userByEmail);

            //string url = $"{_configuration["AppUrl"]}/ResetPassword?email={email}&token={token}";

            //var From = _configuration["EmailFrom"];

            //await _mailService.SendEmailAsync(email, From, "Reset Password",
            //                                    "<h1> Follow the instruction to reset your password </h1>" +
            //                                    $"To reset your password, click here : {url}");

            return new UserManagerResponse
            {
                Message = "Reset password URL has been sent to your email successfully",
                IsSuccess = true
            };
        }
        public async Task<ResultResponse> ChangePassword(ResetPasswordRequest model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return new ResultResponse
                {
                    Message = "email is empty!",
                    IsSuccess = false
                };

            var userByEmail = await _userManager.FindByEmailAsync(model.Email);

            if (userByEmail == null)
                return new ResultResponse
                {
                    Message = "Invalid username or password",
                    IsSuccess = false
                };

            
            if (!await _userManager.CheckPasswordAsync(userByEmail, model.CurrentPassword))
            {
                return new ResultResponse
                {
                    Message = "The credentials are not valid",
                    IsSuccess = false
                };

            }
            if (!model.NewPassword.Equals(model.ConfirmNewPassword))
            {
                return new ResultResponse
                {
                    Message = "Passwords do not match",
                    IsSuccess = false
                };

            }
            
            await _userManager.ChangePasswordAsync(userByEmail, model.CurrentPassword, model.NewPassword);
            return new ResultResponse
            {
                Message = "Welcome",
                IsSuccess = true
            };

        }
     
    }
}
