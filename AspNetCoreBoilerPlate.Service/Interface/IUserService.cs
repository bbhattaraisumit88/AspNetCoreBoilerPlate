using AspNetCoreBoilerPlate.Domain.DTO.User;
using System;
using System.Collections.Generic;

namespace AspNetCoreBoilerPlate.Service.Interface
{
    public interface IUserService
    {
        IEnumerable<UserResponseDTO> GetAllUsers();
        bool CreateUser(CreateUserDTO userDTO);
        bool UpdateUser(UpdateUserDTO userDTO);
        bool DeleteUser(Guid userId);
    }
}
