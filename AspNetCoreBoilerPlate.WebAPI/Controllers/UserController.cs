using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreBoilerPlate.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AspNetCoreBoilerPlate.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("get/all")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var userList = _userService.GetAllUsers();
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

    }
}