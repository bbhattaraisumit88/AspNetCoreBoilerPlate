using AspNetCoreBoilerPlate.Domain.DTO.User;
using AspNetCoreBoilerPlate.Domain.Models;
using AspNetCoreBoilerPlate.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace AspNetCoreBoilerPlate.WebAPI.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("get/all")]
        public IActionResult GetAllUsers([FromBody]TableFiltration tableFiltration)
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
        public IActionResult CreateUser([FromBody]CreateUserDTO createUserDTO)
        {
            try
            {
                bool result = _userService.CreateUser(createUserDTO);
                if (result)
                {
                    return Ok("User Created Successfully.");
                }
                else
                {
                    return BadRequest("Couldn't Create User");
                }

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