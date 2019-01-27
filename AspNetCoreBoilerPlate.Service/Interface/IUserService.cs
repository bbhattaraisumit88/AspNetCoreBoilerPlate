using AspNetCoreBoilerPlate.Domain.DTO.User;
using AspNetCoreBoilerPlate.Domain.Models;
using System;
using System.Collections.Generic;

namespace AspNetCoreBoilerPlate.Service.Interface
{
    public interface IUserService
    {
        IEnumerable<UserResponseDTO> GetAllUsers(TableFiltration tableFiltration);
        bool CreateUser(CreateUserDTO userDTO);
        bool UpdateUser(UpdateUserDTO userDTO);
        bool DeleteUser(Guid userId);
    }
}
