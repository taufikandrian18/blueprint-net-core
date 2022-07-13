using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WITNetCoreProject.Models.Dtos;
using WITNetCoreProject.Models.Entities;
using WITNetCoreProject.Models.Exceptions;
using WITNetCoreProject.Services.Interfaces;
using WITNetCoreProject.Services.Logger;
using WITNetCoreProject.Utilities;
using WITNetCoreProject.Validators;

namespace WITNetCoreProject.Controllers {

    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    public class UserApiController : ControllerBase {

        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;
        private IMapper _mapper;

        public UserApiController(ILoggerManager logger, IRepositoryWrapper repoWrapper, IMapper mapper) {

            _logger = logger;
            _repoWrapper = repoWrapper;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <response code="200">Returns the users</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<UserDto>))]
        public async Task<ActionResult> GetUsers() {

            // using header {bearer (token)}
            string rawUserId = HttpContext.User.FindFirstValue("id");

            if (!Guid.TryParse(rawUserId, out Guid userId)) {
                return Unauthorized();
            }

            // get data to repositories
            var result = await _repoWrapper.User.GetUsers();

            var usersResult = _mapper.Map<IEnumerable<UserDto>>(result);
            var count = usersResult.ToList().Count();
            //return Ok(usersResult);
            return StatusCode(200, ResponseModel.ResponseOk(usersResult, count));
        }

        /// <summary>
        /// Get a product by id
        /// </summary>
        /// <param name="id">Product id</param>
        /// <response code="200">Returns the existing users</response>
        /// <response code="404">If the users doesn't exist</response>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetUserById(Guid id) {

            try {

                // using header {bearer (token)}
                string rawUserId = HttpContext.User.FindFirstValue("id");

                if (!Guid.TryParse(rawUserId, out Guid userId)) {
                    return Unauthorized();
                }

                // get data from database
                var user = await _repoWrapper.User.GetUserById(id);

                var usersResult = _mapper.Map<UserDto>(user);
                //return Ok(usersResult);
                return StatusCode(200, ResponseModel.ResponseOk(usersResult, 1));
            }
            catch (NotFoundException) {

                return NotFound();
            }
        }

        /// <summary>
        /// Create a product
        /// </summary>
        /// <param name="request">Product data</param>
        /// <response code="201">Returns the created user</response>
        /// <response code="400">If the data doesn't pass the validations</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Create(UserForCreationDto request) {

            // using header {bearer (token)}
            string rawUserId = HttpContext.User.FindFirstValue("id");

            if (!Guid.TryParse(rawUserId, out Guid userId)) {

                return Unauthorized();
            }

            // insert process through mapper first, after become an user object then insert to database
            var userEntity = _mapper.Map<Users>(request);

            var validationResult = new UserValidator().Validate(userEntity);

            if(validationResult.IsValid) {

                _repoWrapper.User.CreateUser(userEntity);
                _repoWrapper.Save();
                var createdUser = _mapper.Map<UserDto>(userEntity);
                //return StatusCode(201, createdUser);
                return StatusCode(201, ResponseModel.Created(createdUser));
            }
            else {

                return BadRequest("Validation failed");
            }
        }

        /// <summary>
        /// Update a product by id
        /// </summary>
        /// <param name="id">Product id</param>
        /// <param name="request">Product data</param>
        /// <response code="200">Returns the updated user</response>
        /// <response code="400">If the data doesn't pass the validations</response>
        /// <response code="404">If the user doesn't exist</response>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(Guid id, UserForUpdateDto request) {

            try {

                // using header {bearer (token)}
                string rawUserId = HttpContext.User.FindFirstValue("id");

                if (!Guid.TryParse(rawUserId, out Guid userId)) {

                    return Unauthorized();
                }

                // get data from database
                var userEntity = await _repoWrapper.User.GetUserById(id);

                if (userEntity == null) {

                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                // update process through mapper first, after become an user object then update to database
                var validationResult = new UserValidator().Validate(userEntity);
                if(validationResult.IsValid) {

                    _mapper.Map(request, userEntity);
                    _repoWrapper.User.UpdateUser(userEntity);
                    _repoWrapper.Save();
                    var createdUser = _mapper.Map<UserDto>(userEntity);
                    //return Ok(createdUser);
                    return StatusCode(200, ResponseModel.ResponseOk(createdUser, 1));
                }
                else {

                    return BadRequest("Validation failed");
                }
            }
            catch (NotFoundException) {

                return NotFound();
            }
        }

        /// <summary>
        /// Delete a product by id
        /// </summary>
        /// <param name="id">Product id</param>
        /// <response code="204">If the user was deleted</response>
        /// <response code="404">If the user doesn't exist</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(Guid id) {

            try {

                // using header {bearer (token)}
                string rawUserId = HttpContext.User.FindFirstValue("id");

                if (!Guid.TryParse(rawUserId, out Guid userId)) {

                    return Unauthorized();
                }

                // hard delete to database by id
                _repoWrapper.User.DeleteUserById(id);
                return NoContent();
            }
            catch (NotFoundException) {

                return NotFound();
            }
        }
    }
}
