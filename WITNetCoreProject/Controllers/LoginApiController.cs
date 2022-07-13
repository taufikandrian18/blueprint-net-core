using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WITNetCoreProject.Models.Authentications;
using WITNetCoreProject.Models.Dtos;
using WITNetCoreProject.Models.Entities;
using WITNetCoreProject.Models.Exceptions;
using WITNetCoreProject.Services.Interfaces;
using WITNetCoreProject.Services.Interfaces.Auth;
using WITNetCoreProject.Services.Logger;
using WITNetCoreProject.Services.Repositories.Auth;
using WITNetCoreProject.Utilities;
using WITNetCoreProject.Validators;

namespace WITNetCoreProject.Controllers {

    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    public class LoginApiController : ControllerBase {

        private ILoggerManager _logger;
        private IRepositoryWrapper _repository;
        private IMapper _mapper;
        private readonly Authenticator _services;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LoginApiController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, Authenticator services, RefreshTokenValidator refreshTokenValidator, IRefreshTokenRepository refreshTokenRepository) {

            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _services = services;
            _refreshTokenValidator = refreshTokenValidator;
            _refreshTokenRepository = refreshTokenRepository;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <response code="200">Returns the products</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserProjectLogin))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Login([FromBody] LoginDto model) {

            try {

                // Model state represents errors that come from two subsystems: model binding and model validation
                if (!ModelState.IsValid) {

                    return BadRequestModelState();
                }

                // get user from database using model that user send from parameter
                var user = await _repository.User.GetUserByUsername(model.Username.Trim());
                if (user == null) {

                    // if user null write in log and send the response code 400 that user could not found in db
                    _logger.LogError("User object sent from client wrong username or password.");
                    return BadRequest("User wrong username or password");
                }
                else {

                    // login validation that username and password are matched
                    if (user.Username.Trim() == model.Username && user.Password.Trim() == model.Password) {

                        // mapping user db to user dto for return to client
                        var userResult = _mapper.Map<UserDto>(user);
                        // generate token for the client
                        var tokenUsr = _services.Authenticate(userResult);

                        // fill all the object user and token as well in UserProjectLogin object
                        UserProjectLogin resultUser = new UserProjectLogin() {

                            Users = userResult,
                            Token = tokenUsr.Result
                        };

                        // return response to client as an object
                        return StatusCode(200, ResponseModel.ResponseOk(resultUser, 1));
                    }
                    else {

                        // if username and password aren't matched record in log and send the response code 400 that username and password aren't matched
                        _logger.LogError("User object sent from client wrong username or password.");
                        return BadRequest("User wrong username or password");
                    }
                }
            }
            catch (Exception ex) {

                _logger.LogError($"Something went wrong inside Login action: {ex.Message}");
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <response code="200">Returns the products</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Register([FromBody] RegisterDto model) {

            try {

                // variable date for fill in user data
                var thisDate = DateTime.Now.Date;
                // check whether model object from client is null or not
                if (model == null) {

                    //if model object is null will return response code 400 that object is null
                    _logger.LogError("User object sent from client is null.");
                    return BadRequest("User object is null");
                }

                // Model state represents errors that come from two subsystems: model binding and model validation
                if (!ModelState.IsValid) {

                    _logger.LogError("Invalid user object sent from client.");
                    return BadRequestModelState();
                }

                // check if email is available in database
                var userIdCheck = await _repository.User.GetUserIdByEmail(model.Email.Trim());
                if (userIdCheck != null) {

                    //if model email is not null will return response code 400 that object email is already taken.
                    _logger.LogError("User email sent from client is already taken.");
                    return BadRequest("User object email is already taken.");
                }
                else {

                    // password generator
                    int length = 10;
                    const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                    StringBuilder res = new StringBuilder();
                    Random rnd = new Random();
                    while (0 < length--) {
                        res.Append(valid[rnd.Next(valid.Length)]);
                    }
                    /////////////////////


                    // add user to creation dto then system could be able to mapping to user object
                    UserForCreationDto userCreate = new UserForCreationDto();
                    userCreate.Username = model.Username;
                    userCreate.Password = res.ToString();
                    userCreate.DisplayName = model.FullName;
                    userCreate.Phone = model.Phone;
                    userCreate.Email = model.Email;
                    userCreate.CreatedAt = thisDate;
                    userCreate.CreatedBy = "SYSTEM";
                    userCreate.UpdatedAt = thisDate;
                    userCreate.UpdatedBy = "SYSTEM";


                    // insert process through mapper and become user object then from user object system insert to database
                    var userEntity = _mapper.Map<Users>(userCreate);
                    // this is a validator whether is match with the parameter object or not
                    var validationResult = new UserValidator().Validate(userEntity);
                    if (!validationResult.IsValid) {

                        return BadRequest("Validation failed");
                    }

                    _repository.User.CreateUser(userEntity);
                    _repository.Save();

                    var createdUser = _mapper.Map<UserDto>(userEntity);

                    //return StatusCode(201, createdUser);
                    return StatusCode(201, ResponseModel.Created(createdUser));
                }

            }
            catch (Exception ex) {

                _logger.LogError($"Something went wrong inside Register action: {ex.Message}");
                return StatusCode(500, ex);
            }
        }


        /// <summary>
        /// Get all products
        /// </summary>
        /// <response code="200">Returns the products</response>
        [AllowAnonymous]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto model) {

            try {

                // variable date for fill in user data
                var thisDate = DateTime.Now.Date;

                // check whether model object from client is null or not
                if (model == null) {

                    //if model object is null will return response code 400 that object is null
                    _logger.LogError("User object sent from client is null.");
                    return BadRequest("User object is null");
                }

                // this is a validator whether is match with the parameter object or not
                var validationResult = new LoginValidator().Validate(model);

                if(!validationResult.IsValid) {
                    return BadRequest("Validation failed");
                }

                // Model state represents errors that come from two subsystems: model binding and model validation
                if (!ModelState.IsValid) {

                    _logger.LogError("Invalid user object sent from client.");
                    return BadRequestModelState();
                }

                // check if email is available in database
                var userCheckMail = await _repository.User.GetUserIdByEmail(model.Email.Trim());

                if (userCheckMail == null) {

                    //if model email is null will return response code 400 that object email cant be found in db.
                    _logger.LogError("User email sent from client is already taken.");
                    return BadRequest("User object email is already taken.");
                }
                else {

                    //if model password and confirm password are not match it will return response code 400 that password and confirm password are not matched.
                    if (model.Password.Trim() == model.ConfirmPassword.Trim()) {

                        // if model password and confirm password are matched then put the request from client to user update dto
                        UserForUpdateDto request = new UserForUpdateDto();
                        request.UserId = userCheckMail.UserId;
                        request.Username = userCheckMail.Username;
                        request.Password = model.ConfirmPassword;
                        request.DisplayName = userCheckMail.DisplayName;
                        request.Phone = userCheckMail.Phone;
                        request.Email = model.Email;
                        request.CreatedAt = userCheckMail.CreatedAt;
                        request.CreatedBy = userCheckMail.CreatedBy;
                        request.UpdatedAt = thisDate;
                        request.UpdatedBy = "SYSTEM";

                        // update process to the database
                        _mapper.Map(request, userCheckMail);
                        _repository.User.UpdateUser(userCheckMail);
                        _repository.Save();
                        return NoContent();
                    }
                    else {

                        _logger.LogError("User password and confirm password sent from client aren't matched.");
                        return BadRequest("User password and confirm password sent from client aren't matched.");
                    }
                }

            }
            catch (Exception ex) {

                _logger.LogError($"Something went wrong inside ChangePassword action: {ex.Message}");
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <response code="200">Returns the products</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticatedUserResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> TokenRefresh([FromBody] RefreshRequest refreshRequest) {

            try {

                // Model state represents errors that come from two subsystems: model binding and model validation
                if (!ModelState.IsValid) {

                    return BadRequestModelState();
                }

                // this statement is for validate whether token is valid or not
                bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);

                if (!isValidRefreshToken) {

                    // if the token is invalid it send response invalid refresh token to the client
                    //return BadRequest(new MyErrorResponse("Invalid refresh token."));
                    return StatusCode(401, ResponseModel.BadRequest(new MyErrorResponse("Invalid refresh token.")));
                }

                // if the token is valid it would send refresh token to repository to check is refresh token is stored in db
                RefreshTokens refreshTokenDTO = await _refreshTokenRepository.GetByToken(refreshRequest.RefreshToken);

                if (refreshTokenDTO == null) {

                    // if the token not found in db it would send token cant be found in db
                    return NotFound(new MyErrorResponse("Refresh token is not registered in db"));
                }

                // old access token will be deleted in db to refresh / create a new one
                await _refreshTokenRepository.Delete(refreshTokenDTO.Id);

                Users user = await _repository.User.GetUserById(refreshTokenDTO.UserId);
                // check whether model object from variable user is null or not

                if (user == null) {

                    return NotFound(new MyErrorResponse("User not found."));
                }

                var createdUser = _mapper.Map<UserDto>(user);
                AuthenticatedUserResponse response = await _services.Authenticate(createdUser);

                //return Ok(response);
                return StatusCode(200, ResponseModel.ResponseOk(response, 1));

            }
            catch (Exception ex) {

                _logger.LogError($"Something went wrong inside TokenRefresh action: {ex.Message}");
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Delete a product by id
        /// </summary>
        /// <param name="id">Product id</param>
        /// <response code="204">If the product was deleted</response>
        /// <response code="404">If the product doesn't exist</response>
        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Logout() {

            try {

                // using header {bearer (token)}
                string rawUserId = HttpContext.User.FindFirstValue("id");

                if (!Guid.TryParse(rawUserId, out Guid userId)) {

                    return Unauthorized();
                }

                await _refreshTokenRepository.DeleteAll(userId);

                return NoContent();
            }
            catch (NotFoundException) {

                return NotFound();
            }
        }

        [NonAction]
        private ActionResult BadRequestModelState() {

            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

            return BadRequest(new MyErrorResponse(errorMessages));
        }

    }
}
