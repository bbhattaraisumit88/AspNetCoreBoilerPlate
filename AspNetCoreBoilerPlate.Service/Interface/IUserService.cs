using AspNetCoreBoilerPlate.Domain.DTO.User;
using AspNetCoreBoilerPlate.Domain.HelperClasses;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace AspNetCoreBoilerPlate.Service.Interface
{
    public interface IUserService
    {
        IEnumerable<UserResponseDTO> GetAllUsers(TableFilter tableFiltration);
        IEnumerable<string> GetRoleNames(ICollection<Guid> roleIds);
        IEnumerable<Claim> GetClaims(IEnumerable<string> roles);
        bool CreateUser(CreateUserDTO userDTO);
        bool UpdateUser(UpdateUserDTO userDTO);
        bool DeleteUser(Guid userId);
    }
}
