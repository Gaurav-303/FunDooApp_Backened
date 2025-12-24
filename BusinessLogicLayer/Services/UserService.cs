using BusinessLogicLayer.Interfaces;
using DataLogicLayer.Context;
using ModelLayer.DTOs;
using ModelLayer.Entity;
using System.Linq;


namespace BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly FundooContext _context;
        private readonly JwtService _jwtService;


        public UserService(FundooContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public bool Register(RegisterUserDto dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email))
                return false;

            var user = new Users
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = dto.Password
            };

            _context.Users.Add(user);  
            _context.SaveChanges();

            return true;
        }
        public string Login(LoginUserDto dto)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Email == dto.Email &&
                u.Password == dto.Password);

            if (user == null)
                return null;

            
            return _jwtService.GenerateToken(user.UserId, user.Email);
        }

       
    }
}
