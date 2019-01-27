﻿using AspNetCoreBoilerPlate.Domain.DTO.Role;
using System;
using System.Collections.Generic;

namespace AspNetCoreBoilerPlate.Service.Interface
{
    public interface IRoleService
    {
        IEnumerable<RoleDTO> GetAllRoles();
        IEnumerable<string> GetRoleNamesByRoleId(IEnumerable<Guid> roleId);
        bool CreateRole(CreateRoleDTO entity);
        bool UpdateRole(RoleDTO entity);
        bool DeleteRole(Guid roleId);
    }
}
