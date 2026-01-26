using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(CreateUserDto userDto);
        Task<string> LoginAsync(string email, string password);
        string GenerateJwtToken(int userId, string email, string username);
    }
}
