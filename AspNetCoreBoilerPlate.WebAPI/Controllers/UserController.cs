using AspNetCoreBoilerPlate.Domain.DTO.User;
using AspNetCoreBoilerPlate.Domain.HelperClasses;
using AspNetCoreBoilerPlate.Domain.Models;
using AspNetCoreBoilerPlate.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreBoilerPlate.WebAPI.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        public UserController(IUserService userService,
            UserManager<AppUser> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("get/all")]
        public IActionResult GetAllUsers([FromBody]TableFilter tableFiltration)
        {
            try
            {
                var userList = _userService.GetAllUsers(tableFiltration);
                if (userList.Any())
                {
                    return Ok(JsonConvert.SerializeObject(userList));
                }
                else
                {
                    return NotFound("No users available.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateUser([FromBody]CreateUserDTO createUserDTO)
        {
            try
            {
                var user = new AppUser { UserName = createUserDTO.UserName, FirstName = createUserDTO.FirstName, LastName = createUserDTO.LastName, Email = createUserDTO.Email };
                var result = await _userManager.CreateAsync(user, createUserDTO.Password);
                var roleNames = _userService.GetRoleNames(createUserDTO.RoleIdList);
                var claims = _userService.GetClaims(roleNames);
                if (result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(user, roleNames);
                    await _userManager.AddClaimsAsync(user, claims);
                    return Ok("User Created Successfully");
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateUser([FromBody]UpdateUserDTO updateUserDTO)
        {
            try
            {
                bool result = _userService.UpdateUser(updateUserDTO);
                if (result)
                {
                    return Ok("User Updated Successfully.");
                }
                else
                {
                    return BadRequest("Couldn't Update User");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{userId}")]
        public IActionResult DeleteUser(Guid userId)
        {
            try
            {
                bool result = _userService.DeleteUser(userId);
                if (result)
                {
                    return Ok("User Deleted Successfully");
                }
                else
                {
                    return BadRequest("Couldn't Delete User.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}