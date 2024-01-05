using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Responses;
using Sat.Recruitment.Core.DTOs;
using Sat.Recruitment.Core.Entities;
using Sat.Recruitment.Core.Interfaces;
using Sat.Recruitment.Core.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using Newtonsoft.Json;
using Sat.Recruitment.Core.Services;

namespace Sat.Recruitment.Api.Controllers
{
    /// <summary>
    /// UsersController endpoints.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]    
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserTypeService _userTypeService;
        private readonly IMapper _mapper;
        /// <summary>
        /// UsersController constructor.
        /// </summary>
        /// <param name="userService">IUserService injection instance.</param>
        /// <param name="userTypeService">IUserTypeService injection instance.</param>
        /// <param name="mapper">IMapper injection instance.</param>
        public UsersController(IUserService userService, IUserTypeService userTypeService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
            _userTypeService = userTypeService;
        }

        [HttpPost]
        public IActionResult TestAction([FromBody] UserRegisterDto registeredUserDto)
        {
            var dbUsersAuthList = _userService.GetUsers();
            var dbUserListDto = new List<UserAuthDto>();

            foreach (var dbUserItem in dbUsersAuthList)
                dbUserListDto.Add(_mapper.Map<UserAuthDto>(dbUserItem));

            return Ok(dbUserListDto);
        }
        /// <summary>
        /// Loads a Users.txt file for the first time.
        /// </summary>
        /// <returns>The users inserted in the DB.</returns>
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoadUsers()
        {
            List<string> usersResult = new List<string>();
            Debug.WriteLine(Directory.GetCurrentDirectory());
            var usersFromFile = _userService.GetUsersFromFile(_userTypeService, $"{Directory.GetCurrentDirectory()}/Files/Users.txt");
            var duplicatedAndNotDuplicated = await _userService.InsertUserAsync(usersFromFile);
            
            usersResult.Add(JsonConvert.SerializeObject(duplicatedAndNotDuplicated[0]));
            usersResult.Add(JsonConvert.SerializeObject(duplicatedAndNotDuplicated[1]));

            var response = new ApiResponse<List<string>>(usersResult);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Ok(response);
        }
        /// <summary>
        /// Gets all the Users records from the DB
        /// </summary>
        /// <returns>An ApiResponse instance with the Users in Data property</returns>
        //[AllowAnonymous]
        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30seconds")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            var user = _userService.GetUsers();
            var userDto = _mapper.Map<IEnumerable<UserDto>>(user);
            var response = new ApiResponse<IEnumerable<UserDto>>(userDto);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Ok(response);
        }
        /// <summary>
        /// Gets a User by the user Id. Id == Primary key.
        /// </summary>
        /// <param name="id">The User Id primary key.</param>
        /// <returns>An ApiResponse instance with a single User in Data property.</returns>
        //[AllowAnonymous]
        [HttpGet]
        [ResponseCache(CacheProfileName = "Default30seconds")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            var response = new ApiResponse<UserDto>();
            if (id <= 0)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }
            
            var user = await _userService.GetUser(id);
            if (user == null) {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            } 

            var userDto = _mapper.Map<UserDto>(user);
            response.Result = userDto;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);
        }
        /// <summary>
        /// Inserts a single user.
        /// </summary>
        /// <param name="userDto">A UserDto instance to insert.</param>
        /// <returns>An ApiResponse instance with a single User in Data property.</returns>
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] UserDto userDto)
        {
            var response = new ApiResponse<UserDto>();
            if (userDto == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

            if (await _userTypeService.GetUserType((int)userDto.UserTypeId) == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }

            var user = _mapper.Map<UserDetails>(userDto);
            await _userService.InsertUserAsync(user);

            userDto = _mapper.Map<UserDto>(user);
            response.Result = userDto;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);
        }
        /// <summary>
        /// Updates a single user.
        /// </summary>
        /// <param name="id">The user Id (identity-primary key).</param>
        /// <param name="userDto">A UserDto instance to update.</param>
        /// <returns>An ApiResponse instance with the User updated.</returns>
        //[Authorize(Roles = "Admin")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] UserDto userDto)
        {
            var response = new ApiResponse<UserDetails>();
            if (id <= 0 || userDto == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

            if (await _userTypeService.GetUserType((int)userDto.UserTypeId) == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }

            var user = _mapper.Map<UserDetails>(userDto);
            user.Id = id;

            var result = await _userService.UpdateUser(user);
            response.Result = user;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);
        }
        /// <summary>
        /// Deletes a user by Id.
        /// </summary>
        /// <param name="id">The user Id.</param>
        /// <returns>An ApiResponse instance.</returns>
        //[Authorize(Roles = "Admin")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new ApiResponse<bool>();
            if (id <= 0)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

            if (await _userService.GetUser(id) == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }

            var result = await _userService.DeleteUser(id);
            response.Result = result;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);
        }
    }    
}
