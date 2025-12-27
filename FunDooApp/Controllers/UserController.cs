using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataLogicLayer.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.DTOs;
using ModelLayer.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FunDooApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

     
        private readonly EmailService _emailService;
        private readonly FundooContext _context;
        private readonly IConfiguration _config;

        public UserController(
    FundooContext context,
    IConfiguration config,
    IUserService userService,
    EmailService emailService)
        {
            _context = context;
            _config = config;
            _userService = userService;
            _emailService = emailService;
        }



        [HttpPost("register")]
        public IActionResult Register(RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _userService.Register(dto);

            if (!result)
                return BadRequest("User already exists");
                     _emailService.SendEmail(
                     dto.Email,
                           "Welcome to BridgeLabz",
                      $"Hello {dto.FirstName}, your account has been created successfully."
   );

            return Ok("User registered successfully. Email sent.");
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginUserDto dto)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
                if (user == null)
                    return Unauthorized("Invalid email");

                var hasher = new PasswordHasher<Users>();
                var result = hasher.VerifyHashedPassword(
                    user,
                    user.Password,
                    dto.Password
                );

                if (result == PasswordVerificationResult.Failed)
                    return Unauthorized("Invalid password");

                // generate token here
                return Ok(new { token = GenerateJwt(user) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // TEMP for debugging
            }
        }
        private string GenerateJwt(Users user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_config["Jwt:DurationInMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpGet]
        public IActionResult GetProfile()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _userService.GetUserById(userId);

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }
        [HttpGet("{userId}")]
        public IActionResult GetUserById(int userId)
        {
            var user = _userService.GetUserById(userId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(user);
        }


        [HttpPut]
        public IActionResult UpdateProfile(UpdateUserDto dto)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            bool updated = _userService.UpdateUser(userId, dto);
            if (!updated)
                return NotFound("User not found");

            return Ok("User updated successfully");
        }

        [HttpDelete]
        public IActionResult DeleteAccount()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            bool deleted = _userService.DeleteUser(userId);
            if (!deleted)
                return NotFound("User not found");

            return Ok("User deleted successfully");
        }
    }
}
