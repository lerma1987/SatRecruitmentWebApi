using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sat.Recruitment.Core.DTOs;
using Sat.Recruitment.Core.Entities;
using Sat.Recruitment.Core.Interfaces;
using Sat.Recruitment.Infrastructure.Data;
using System.Security.Claims;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using XSystem.Security.Cryptography;

namespace Sat.Recruitment.Infrastructure.Repositories
{
    public class UserAuthRepository : IUserAuthRepository
    {
        private readonly SatRecruitmentContext _context;
        private readonly string _secretKey;
        public UserAuthRepository(SatRecruitmentContext context, IConfiguration config) 
        { 
            _context = context;
            _secretKey = config.GetValue<string>("ApiSettings:Secret");
        }
        public UserAuth GetUserById(int userAuthId)
        {
            return _context.UsersAuth.FirstOrDefault(x => x.Id == userAuthId); 
        }
        public ICollection<UserAuth> GetUsers()
        {
            return _context.UsersAuth.OrderBy(u => u.Username).ToList();
        }
        public bool IsUniqueUser(string username)
        {
            var dbUser = _context.UsersAuth.FirstOrDefault(u => u.Username == username);
            if (dbUser == null)
                return true;
            else
                return false;
        }
        public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
        {
            var encryptedPassword = GetMD5(userLoginDto.Password);
            var dbUser = _context.UsersAuth.FirstOrDefault(u => 
                                                           u.Username.ToLower() == userLoginDto.Username.ToLower() && 
                                                           u.Password == encryptedPassword);
            if (dbUser == null)
                return new UserLoginResponseDto()
                {
                    Token = string.Empty,
                    Usuario = null
                };

            var tokenManager = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, dbUser.Username.ToString()),
                    new Claim(ClaimTypes.Role, dbUser.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenManager.CreateToken(tokenDescriptor);
            UserLoginResponseDto userLoginResponseDto = new UserLoginResponseDto()
            {
                Token = tokenManager.WriteToken(token),
                Usuario = dbUser
            };

            return userLoginResponseDto;
        }
        public async Task<UserAuth> Register(UserRegisterDto userRegisterDto)
        {
            var encryptedPassword = GetMD5(userRegisterDto.Password);
            UserAuth userAuth = new UserAuth()
            {
                Username = userRegisterDto.Username,
                Password = encryptedPassword,
                Name = userRegisterDto.Name,
                Role = userRegisterDto.Role
            };

            _context.UsersAuth.Add(userAuth);
            await _context.SaveChangesAsync();
            userAuth.Password = encryptedPassword;
            return userAuth;
        }
        public static string GetMD5(string password)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
                resp += data[i].ToString("x2").ToLower();

            return resp;
        }
    }
}
