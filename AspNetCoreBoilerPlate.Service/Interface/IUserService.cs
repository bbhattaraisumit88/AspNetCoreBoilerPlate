using AspNetCoreBoilerPlate.Domain.DTO.User;
using AspNetCoreBoilerPlate.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AspNetCoreBoilerPlate.Service.Interface
{
    public interface IUserService
    {
        IEnumerable<UserResponseDTO> GetAllUsers(
       Expression<Func<AppUser, bool>> filter = null,
       Func<IQueryable<AppUser>, IOrderedQueryable<AppUser>> orderBy = null);
        bool CreateUser(CreateUserDTO userDTO);
    }
}
