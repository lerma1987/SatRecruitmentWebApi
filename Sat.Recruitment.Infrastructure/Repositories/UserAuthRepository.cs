using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sat.Recruitment.Core.DTOs;
using Sat.Recruitment.Core.Entities;
using Sat.Recruitment.Core.Interfaces;
using Sat.Recruitment.Infrastructure.Data;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace Sat.Recruitment.Infrastructure.Repositories
{
    public class UserAuthRepository : IUserAuthRepository
    {
        private readonly SatRecruitmentContext _context;
        private readonly IMapper _mapper;
        private readonly string _secretKey;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserAuthRepository(SatRecruitmentContext context,
                                  UserManager<AppUser> userManager,
                                  RoleManager<IdentityRole> roleManager,
                                  IMapper mapper,
                                  IConfiguration config)
        { 
            _context = context;
            _secretKey = config.GetValue<string>("ApiSettings:SecretKey");
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public AppUser GetUserById(string id)
        {
            return _context.AppUsers.FirstOrDefault(x => x.Id == id); 
        }
        public ICollection<AppUser> GetUsers()
        {
            return _context.AppUsers.OrderBy(u => u.UserName).ToList();
        }
        public bool IsUniqueUser(string username)
        {
            var dbUser = _context.AppUsers.FirstOrDefault(u => u.UserName == username);
            if (dbUser == null)
                return true;
            else
                return false;
        }
        public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
        {
            //var encryptedPassword = GetMD5(userLoginDto.Password);

            /*---COMPARES IF THE USERNAME EXISTS IN THE APPUSER TABLE---*/
            var dbUser = _context.AppUsers.FirstOrDefault(u => u.UserName.ToLower() == userLoginDto.Username.ToLower());
            bool isValidUser = await _userManager.CheckPasswordAsync(dbUser, userLoginDto.Password);
            if (dbUser == null || isValidUser == false)
                return new UserLoginResponseDto()
                {
                    Token = string.Empty,
                    Usuario = null
                };

            /*---GETS THE dbRoles ACCORDING TO THE dbUser---*/
            var dbRoles = await _userManager.GetRolesAsync(dbUser);

            var tokenManager = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, dbUser.UserName.ToString()),
                    new Claim(ClaimTypes.Role, dbRoles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenManager.CreateToken(tokenDescriptor);
            UserLoginResponseDto userLoginResponseDto = new UserLoginResponseDto()
            {
                Token = tokenManager.WriteToken(token),
                Usuario = _mapper.Map<UserAuthDto>(dbUser)
            };

            return userLoginResponseDto;
        }
        public async Task<UserAuthDto> Register(UserRegisterDto userRegisterDto)
        {
            //var encryptedPassword = GetMD5(userRegisterDto.Password);

            AppUser tempUserApp = new AppUser()
            {
                UserName = userRegisterDto.Username,
                Email = userRegisterDto.Username,
                NormalizedUserName = userRegisterDto.Name.ToUpper(),
                Fullname = userRegisterDto.Name
            };

            var result = await _userManager.CreateAsync(tempUserApp, userRegisterDto.Password);

            if (result.Succeeded)
            {
                /*---THIS VALIDATION IS ONLY TO CREATE THE ROLES FOR THE FIRST TIME---*/
                if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("Registered"));
                }

                await _userManager.AddToRoleAsync(tempUserApp, "Admin");
                var userResponse = _context.AppUsers
                                            .Select(dbUsers => new AppUser { Fullname = dbUsers.Fullname, UserName = dbUsers.UserName })
                                            .Where(users => users.UserName == userRegisterDto.Username)
                                            .FirstOrDefault();

                return _mapper.Map<UserAuthDto>(userResponse);
            }

            //_context.AppUsers.Add(userAuth);
            //await _context.SaveChangesAsync();
            //userAuth.Password = encryptedPassword;
            return new UserAuthDto();
        }
        //public static string GetMD5(string password)
        //{
        //    MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
        //    byte[] data = System.Text.Encoding.UTF8.GetBytes(password);
        //    data = x.ComputeHash(data);
        //    string resp = "";
        //    for (int i = 0; i < data.Length; i++)
        //        resp += data[i].ToString("x2").ToLower();

        //    return resp;
        //}
    }
}
