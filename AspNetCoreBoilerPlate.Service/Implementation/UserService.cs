using AspNetCoreBoilerPlate.Domain.DTO.User;
using AspNetCoreBoilerPlate.Domain.Models;
using AspNetCoreBoilerPlate.Infrastructure.Repositories.Interface;
using AspNetCoreBoilerPlate.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AspNetCoreBoilerPlate.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        public UserService(IUnitOfWork uow) => _uow = uow;
        public IEnumerable<UserResponseDTO> GetAllUsers(Expression<Func<AppUser, bool>> filter, Func<IQueryable<AppUser>, IOrderedQueryable<AppUser>> orderBy)
        {
            try
            {
                var userList = (_uow.UserRepository.GetAll()
                                   .Join(_uow.UserRoleRepository.GetAll(),
                                         user => user.Id,
                                         userRole => userRole.UserId,
                                         (user, userRole) => new {
                                             user, userRole
                                         }
                                   ).Join(_uow.RoleRepository.GetAll(),
                                          userRole => userRole.userRole.RoleId,
                                          role => role.Id,
                                          (userRole, role) => new
                                          {
                                              userRole, role
                                          }
                                   )).GroupBy(x => x.userRole.user.Id)
                                   .Select(q => new UserResponseDTO {
                                       UserId = q.Key,
                                       FirstName = q.FirstOrDefault().userRole.user.FirstName,
                                       LastName = q.FirstOrDefault().userRole.user.LastName,
                                       Email = q.FirstOrDefault().userRole.user.Email,
                                       UserName = q.FirstOrDefault().userRole.user.UserName,
                                       RoleNames = q.Select(x => x.role.Name).ToList()
                                   }).ToList();
                return userList;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
