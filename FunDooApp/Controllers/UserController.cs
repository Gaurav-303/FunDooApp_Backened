using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs;

namespace FunDooApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

     
        private readonly EmailService _emailService;
        
       

        public UserController(IUserService userService, EmailService emailService)
        {
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
        [HttpPost("login")]
        public IActionResult Login(LoginUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = _userService.Login(dto);

            if (token == null)
                return Unauthorized("Invalid email or password");

            return Ok(new
            {
                message = "Login successful",
                token = token
            });
        }
    }
}
