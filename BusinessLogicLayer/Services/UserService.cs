using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.CustomException;
using ModelLayer.DTOs;
using ModelLayer.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly FundooContext _context;
        private readonly JwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public UserService(
            FundooContext context,
            JwtService jwtService,
            IEmailService emailService,
            IConfiguration config)
        {
            _context = context;
            _jwtService = jwtService;
            _emailService = emailService;
            _config = config;
        }

        // ---------------- REGISTER ----------------
        public bool Register(RegisterUserDto dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email))
                return false;

            var user = new Users
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            return true;
        }

        // ---------------- LOGIN ----------------
        public string Login(LoginUserDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == dto.Email);
            if (user == null) return null;

            bool isValidPassword =
                BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            if (!isValidPassword) return null;

            return _jwtService.GenerateToken(user.UserId, user.Email);
        }

        // ---------------- GET USER ----------------
        public UserResponseDto GetUserById(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
                throw new AppException("User not found", 404);

            return new UserResponseDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }

        // ---------------- FORGET PASSWORD ----------------
        public bool ForgetPassword(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return false;

            var token = GenerateResetToken(email);

            var link = $"https://localhost:3000/reset-password?token={token}";

            _emailService.SendEmail(
                email,
                "Reset Your Fundoo Password",
                $"<p>Click the link to reset password:</p><a href='{link}'>Reset Password</a>"
            );

            return true;
        }

        // ---------------- TOKEN GENERATION ----------------
        private string GenerateResetToken(string email)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // ---------------- UPDATE USER ----------------
        public bool UpdateUser(int userId, UpdateUserDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return false;

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;

            _context.SaveChanges();
            return true;
        }

        // ---------------- DELETE USER ----------------
        public bool DeleteUser(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
        public bool ResetPassword(string token, string newPassword)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var email = jwtToken.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (email == null) return false;

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return false;

            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _context.SaveChanges();

            return true;
        }
       

   


}
}
