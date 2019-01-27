using AspNetCoreBoilerPlate.Domain.DTO.Role;
using AspNetCoreBoilerPlate.Domain.Models;
using System;
using System.Collections.Generic;

namespace AspNetCoreBoilerPlate.Service.Interface
{
    public interface IRoleService
    {
        IEnumerable<RoleDTO> GetAllRoles(TableFiltration tableFiltration);
        IEnumerable<string> GetRoleNamesByRoleId(IEnumerable<Guid> roleId);
        bool CreateRole(CreateRoleDTO entity);
        bool UpdateRole(RoleDTO entity);
        bool DeleteRole(Guid roleId);
    }
}
