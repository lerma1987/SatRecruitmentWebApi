using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Api.Responses;
using Sat.Recruitment.Core.DTOs;
using Sat.Recruitment.Core.Entities;
using Sat.Recruitment.Core.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using XAct.Users;

namespace Sat.Recruitment.Api.Controllers
{
    /// <summary>
    /// UserTypeController with Endpoints.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserTypesController : ControllerBase
    {
        private readonly IUserTypeService _userTypeService;
        private readonly IMapper _mapper;
        /// <summary>
        /// UserTypesController constructor.
        /// </summary>
        /// <param name="userTypeService">IUserTypeService injection instance.</param>
        /// <param name="mapper">IMapper injection instance.</param>
        public UserTypesController(IUserTypeService userTypeService, IMapper mapper)
        {
            _userTypeService = userTypeService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all the UserTypes.
        /// </summary>
        /// <returns>A list of UserTypes.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            var userTypes = _userTypeService.GetUserTypes();
            var userTypesDto = _mapper.Map<IEnumerable<UserTypeDto>>(userTypes);
            var response = new ApiResponse<IEnumerable<UserTypeDto>>(userTypesDto);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);
        }
        /// <summary>
        /// Gets a UserType by Id.
        /// </summary>
        /// <param name="id">UserType Id.</param>
        /// <returns>A single UserType.</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            var response = new ApiResponse<UserTypeDto>();
            if (id <= 0)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

            var userType = await _userTypeService.GetUserType(id);
            if (userType == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }

            var userTypeDto = _mapper.Map<UserTypeDto>(userType);
            response.Result = userTypeDto;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);
        }
        /// <summary>
        /// Inserts a UserType.
        /// </summary>
        /// <param name="userTypeDto">The UserType object to insert.</param>
        /// <returns>The UserType inserted.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(UserTypeDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] UserTypeDto userTypeDto)
        {
            var response = new ApiResponse<UserTypeDto>();
            if (userTypeDto == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

            var userType = _mapper.Map<UserType>(userTypeDto);
            await _userTypeService.InsertUserType(userType);

            userTypeDto = _mapper.Map<UserTypeDto>(userType);
            response.Result = userTypeDto;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);
        }
        /// <summary>
        /// Updates a UserType.
        /// </summary>
        /// <param name="id">The UserType Id to update.</param>
        /// <param name="userTypeDto">The UserType object to update.</param>
        /// <returns>The UserType updated.</returns>
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, [FromBody] UserTypeDto userTypeDto)
        {
            var response = new ApiResponse<bool>();
            if (id <= 0 || userTypeDto == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

            if (await _userTypeService.GetUserType(id) == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }

            var userType = _mapper.Map<UserType>(userTypeDto);
            userType.Id = id;

            var result = await _userTypeService.UpdateUserType(userType);
            response.Result = result;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);
        }
        /// <summary>
        /// Deletes a UserType.
        /// </summary>
        /// <param name="id">The UserType Id to delete.</param>
        /// <returns>True or false if the UserType was deleted.</returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

            if (await _userTypeService.GetUserType(id) == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(response);
            }

            var result = await _userTypeService.DeleteUserType(id);
            response.Result = result;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;

            return Ok(response);
        }
    }
}