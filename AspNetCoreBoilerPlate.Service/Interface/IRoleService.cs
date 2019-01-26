using AspNetCoreBoilerPlate.Domain.DTO.Role;
using System.Collections.Generic;

namespace AspNetCoreBoilerPlate.Service.Interface
{
    public interface IRoleService
    {
        IEnumerable<RoleDTO> GetAllRoles();
        bool CreateRole(RoleDTO entity);
    }
}
