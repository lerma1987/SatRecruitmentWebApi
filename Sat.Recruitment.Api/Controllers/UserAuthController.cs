using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Responses;
using Sat.Recruitment.Core.DTOs;
using Sat.Recruitment.Core.Interfaces;
using Sat.Recruitment.Core.Services;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{
    /// <summary>
    /// UserAuthController security endpoints.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserAuthController : Controller
    {
        private readonly IUserAuthService _userAuthService;
        private readonly IMapper _mapper;
        /// <summary>
        /// UserAuthController constructor.
        /// </summary>
        /// <param name="userAuthService">IUserAuthService injection instance.</param>
        /// <param name="mapper">IMapper injection instance.</param>
        public UserAuthController(IUserAuthService userAuthService, IMapper mapper)
        {
            _userAuthService = userAuthService;
            _mapper = mapper;
        }
        /// <summary>
        /// Gets all the registered users and its roles.
        /// </summary>
        /// <returns>A list of registered users.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUsersAuth()
        {
            var dbUsersAuthList = _userAuthService.GetUsers();
            var dbUserListDto = new List<UserAuthDto>();

            foreach (var dbUserItem in dbUsersAuthList)
                dbUserListDto.Add(_mapper.Map<UserAuthDto>(dbUserItem));

            return Ok(dbUserListDto);
        }
        /// <summary>
        /// GetUserAuth.
        /// </summary>
        /// <param name="userAuthId">The UserAuthId to get.</param>
        /// <returns>The UserAuth info.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("{userAuthId:int}", Name = "GetUserAuth")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetUserAuth(int userAuthId)
        {
            var userAuthItem = _userAuthService.GetUserById(userAuthId);

            if (userAuthItem == null)
                return NotFound();

            var userItemDto = _mapper.Map<UserAuthDto>(userAuthItem);
            return Ok(userItemDto);
        }
        /// <summary>
        /// UserAuth register.
        /// </summary>
        /// <param name="registeredUserDto">UserRegisterDto object.</param>
        /// <returns>ApiResponse info.</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registeredUserDto)
        {
            bool isUnique = _userAuthService.IsUniqueUser(registeredUserDto.Username);
            var apiResponse = new ApiResponse<UserRegisterDto>();
            if (!isUnique)
            {
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add("The username already exist.");
                return BadRequest(apiResponse);
            }

            var usuario = await _userAuthService.Register(registeredUserDto);
            if (usuario == null)
            {
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add("Something went wrong.");
                return BadRequest(apiResponse);
            }

            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.IsSuccess = true;
            return Ok(apiResponse);
        }
        /// <summary>
        /// Login.
        /// </summary>
        /// <param name="userLoginDto">UserLoginDto object.</param>
        /// <returns>ApiResponse info.</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var loginResponse = await _userAuthService.Login(userLoginDto);
            var apiResponse = new ApiResponse<UserLoginResponseDto>();

            if (loginResponse.Usuario == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                apiResponse.StatusCode = HttpStatusCode.BadRequest;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add("Username or password are incorrect.");
                return BadRequest(apiResponse);
            }

            apiResponse.StatusCode = HttpStatusCode.OK;
            apiResponse.IsSuccess = true;
            apiResponse.Result = loginResponse;
            return Ok(apiResponse);
        }
    }
}
