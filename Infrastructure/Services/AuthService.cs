using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Settings;
using Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthService(AppDbContext context, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<string> RegisterAsync(CreateUserDto userDto)
        {
            // Email kontrolü
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
            {
                throw new Exception("Bu email zaten kayıtlı!");
            }

            // Username kontrolü
            if (await _context.Users.AnyAsync(u => u.Username == userDto.Username))
            {
                throw new Exception("Bu kullanıcı adı zaten alınmış!");
            }

            // Şimdilik basit hash (production'da BCrypt kullan!)
            var passwordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(userDto.Password));

            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return GenerateJwtToken(user.Id, user.Email, user.Username);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                throw new Exception("Email veya şifre hatalı!");
            }

            // Şifre kontrolü
            var passwordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
            if (user.PasswordHash != passwordHash)
            {
                throw new Exception("Email veya şifre hatalı!");
            }

            // LastLogin güncelle
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return GenerateJwtToken(user.Id, user.Email, user.Username);
        }

        public string GenerateJwtToken(int userId, string email, string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}