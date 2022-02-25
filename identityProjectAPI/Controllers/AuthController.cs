using identityProjectAPI.Services;
using Microsoft.AspNetCore.Mvc;
using SharedProject.Requests;
using SharedProject.Response;

namespace identityProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(ApiResponse<UserManagerResponse>))]
        [ProducesResponseType(400, Type = typeof(ApiResponse<UserManagerResponse>))]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var result = await _userService.GenerateTokenAsync(model);

            if (result.IsSuccess)
                return Ok(new ApiResponse<UserManagerResponse>(result));

            return BadRequest(new ApiResponse<UserManagerResponse>(result));
        }

        [HttpPost("register")]
        [ProducesResponseType(200, Type = typeof(ApiResponse<ResultResponse>))]
        [ProducesResponseType(400, Type = typeof(ApiResponse<ResultResponse>))]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            var result = await _userService.RegisterUserAsync(model);

            if (result.IsSuccess) return Ok(new ApiResponse<ResultResponse>(result));

            return BadRequest(new ApiResponse<ResultResponse>(result));
        }

        [HttpPut("forgotpassword")]
        [ProducesResponseType(200, Type = typeof(ApiResponse<UserManagerResponse>))]
        [ProducesResponseType(400, Type = typeof(ApiResponse<UserManagerResponse>))]
        public async Task<IActionResult> ForgetPassword(ForgotPasswordRequest model)
        {
           
            var result = await _userService.ForgetPasswordAsync(model.Email);

            if (result.IsSuccess)
            {
                return Ok(new ApiResponse<UserManagerResponse>(result)); //200
            }

            return BadRequest(new ApiResponse<UserManagerResponse>(result)); //400
        }
        
        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePasswordUser(ResetPasswordRequest changePassword)
        {

            var result = await _userService.ChangePassword(changePassword);
            if (result.IsSuccess) return Ok(new ApiResponse<ResultResponse>(result));

            return BadRequest(new ApiResponse<ResultResponse>(result));
        }
      
    }
}
