/* Sample UsersController.
 * This is a sample controller which contents needs to be coppied into the UsersContoller.cs
 * In the Web API project.
 * 
 * The controller should inherit from "ControllerBase" as a normal controller.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Onesoftdev.AspCoreJwtAuth.Models;
using Onesoftdev.AspCoreJwtAuth.Mappers;
using Onesoftdev.AspCoreJwtAuth.Helpers;
using Onesoftdev.AspCoreJwtAuth.Services;
using Microsoft.AspNetCore.Authorization;

namespace Onesoftdev.WebApi1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var userEntites = await _userService.GetUsers();
            var userModels = UserMapper.GetUserModelsFromEntities(userEntites);
            return Ok(userModels);
        }

        [HttpGet]
        [Route("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUsers(Guid id)
        {
            var user = await _userService.GetUserById(id);
            var userModel = UserMapper.GetUserModelFromEntity(user);
            return Ok(userModel);
        }

        [AllowAnonymous]
        [HttpPost(Name = "register")]
        public async Task<IActionResult> Register([FromBody]UserCreate userCreate)
        {
            // map dto to entity
            var userEntity = UserMapper.GetUserEntityFromCreateModel(userCreate);

            if (await _userService.UsernameExists(userEntity.Username))
                return Conflict(new { error = $"The username '{userEntity.Username}' is already in use." });

            try
            {
                // save 
                var user = await _userService.Create(userEntity, userCreate.Password);
                var userModel = UserMapper.GetUserModelFromEntity(user);
                return CreatedAtRoute("GetUser", new { id = user.Id }, userModel);
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody]UserUpdate userUpdate)
        {
            var userEntity = await _userService.GetUserById(id);
            if (userEntity == null)
                return NotFound();

            userEntity.IsVerified = userUpdate.IsVerified;
            userEntity.UserDetails.FirstName = userUpdate.DetailsUpdate.FirstName;
            userEntity.UserDetails.LastName = userUpdate.DetailsUpdate.LastName;
            userEntity.UserDetails.EmailAddress = userUpdate.DetailsUpdate.EmailAddress;

            try
            {
                // save 
                await _userService.Update(userEntity, userUpdate.Password);
                return NoContent();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}", Name = "PartialyUpdateUser")]
        public async Task<IActionResult> PartiallyUpdateUser(Guid id,
            [FromBody] JsonPatchDocument<UserUpdate> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            if (!await _userService.UserExits(id))
                return NotFound();

            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();

            var userUpdateModel = UserMapper.GetUserUpdateModelFromEntity(user);

            patchDoc.ApplyTo(userUpdateModel);

            TryValidateModel(userUpdateModel);

            // Add validation
            if (!ModelState.IsValid)
                return new AspCoreJwtAuth.Helpers.UnprocessableEntityObjectResult(ModelState);

            user.IsVerified = userUpdateModel.IsVerified;

            if (userUpdateModel.Password != null)
            {
                var passwordHash = UserService.HashPassword(userUpdateModel.Password);
                user.PasswordHash = passwordHash[0];
                user.PasswordSalt = passwordHash[1];
            }

            user.UserDetails.FirstName = userUpdateModel.DetailsUpdate.FirstName;
            user.UserDetails.LastName = userUpdateModel.DetailsUpdate.LastName;
            user.UserDetails.EmailAddress = userUpdateModel.DetailsUpdate.EmailAddress;

            await _userService.Update(user);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _userService.UserExits(id))
                return NotFound();

            await _userService.Delete(id);
            return NoContent();
        }
    }
}

*/
