using ModelLayer.DTOs;
using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLogicLayer.Interfaces
{
    public interface IUserService
    {
        bool Register(RegisterUserDto dto);
        string Login(LoginUserDto dto);
        UserResponseDto GetUserById(int userId);
        bool UpdateUser(int userId, UpdateUserDto dto);
        bool DeleteUser(int userId);
    }


}
