using AspNetCoreBoilerPlate.Domain.DTO.Role;
using AspNetCoreBoilerPlate.Domain.HelperClasses;
using AspNetCoreBoilerPlate.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace AspNetCoreBoilerPlate.WebAPI.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Route("get/all")]
        public IActionResult GetAllRoles([FromBody]TableFiltration tableFiltration)
        {
            try
            {
                var roleList = _roleService.GetAllRoles(tableFiltration);
                if (roleList.Any())
                {
                    return Ok(JsonConvert.SerializeObject(roleList));
                }
                else
                {
                    return NotFound("No roles available.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("create")]
        public IActionResult CreateRole([FromBody]CreateRoleDTO createRoleDTO)
        {
            try
            {
                bool result = _roleService.CreateRole(createRoleDTO);
                if (result)
                {
                    return Ok("Role Created Successfully.");
                }
                else
                {
                    return BadRequest("Couldn't Create Role");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateRole([FromBody]RoleDTO roleDTO)
        {
            try
            {
                bool result = _roleService.UpdateRole(roleDTO);
                if (result)
                {
                    return Ok("Role Updated Successfully.");
                }
                else
                {
                    return BadRequest("Couldn't Update Role");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete/{roleId}")]
        public IActionResult DeleteRole(Guid roleId)
        {
            try
            {
                bool result = _roleService.DeleteRole(roleId);
                if (result)
                {
                    return Ok("Role Deleted Successfully");
                }
                else
                {
                    return BadRequest("Couldn't Delete Role.");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}