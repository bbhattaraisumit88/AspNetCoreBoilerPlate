using AspNetCoreBoilerPlate.Domain.DTO.User;
using AspNetCoreBoilerPlate.Domain.Models;
using AspNetCoreBoilerPlate.Infrastructure.Repositories.Interface;
using AspNetCoreBoilerPlate.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;

namespace AspNetCoreBoilerPlate.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<AppUser> _userManager;
        private readonly ClaimsPrincipal _claimsPrincipal;
        public UserService(IUnitOfWork uow, UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _userManager = userManager;
            _claimsPrincipal = httpContextAccessor.HttpContext.User;
        }
        public IEnumerable<UserResponseDTO> GetAllUsers(Expression<Func<AppUser, bool>> filter, Func<IQueryable<AppUser>, IOrderedQueryable<AppUser>> orderBy)
        {
            var userList = (_uow.UserRepository.GetAll()
                               .Join(_uow.UserRoleRepository.GetAll(),
                                     user => user.Id,
                                     userRole => userRole.UserId,
                                     (user, userRole) => new
                                     {
                                         user,
                                         userRole
                                     }
                               ).Join(_uow.RoleRepository.GetAll(),
                                      userRole => userRole.userRole.RoleId,
                                      role => role.Id,
                                      (userRole, role) => new
                                      {
                                          userRole,
                                          role
                                      }
                               )).GroupBy(x => x.userRole.user.Id)
                               .Select(q => new UserResponseDTO
                               {
                                   UserId = q.Key,
                                   FirstName = q.FirstOrDefault().userRole.user.FirstName,
                                   LastName = q.FirstOrDefault().userRole.user.LastName,
                                   Email = q.FirstOrDefault().userRole.user.Email,
                                   UserName = q.FirstOrDefault().userRole.user.UserName,
                                   RoleNames = q.Select(x => x.role.Name).ToList()
                               }).ToList();
            return userList;
        }

        public bool CreateUser(CreateUserDTO userDTO)
        {
            try
            {
                AppUser entity = new AppUser()
                {
                    Email = userDTO.Email,
                    FirstName = userDTO.FirstName,
                    LastName = userDTO.LastName,
                    UserName = userDTO.UserName
                };
                var userRoles = AddUserRoles(entity.Id, userDTO.RoleIdList);
                _uow.UserRepository.Insert(entity);
                _uow.UserRoleRepository.InsertRange(userRoles);
                return SaveData();
                //var result = await _userManager.CreateAsync(entity, userDTO.Password);
                //if (result.Succeeded)
                //{
                //    foreach (var item in userDTO.RoleIdList)
                //    {
                //        var roleName = _roleService.GetRoleById(item).Name;
                //        await _userManager.AddToRoleAsync(entity, roleName);
                //        await _userManager.AddClaimAsync(user, new Claim("role", roleName));
                //    }
                //}
                //return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private IEnumerable<AppUserRole> AddUserRoles(Guid userId, ICollection<Guid> roleIds)
        {
            foreach (var item in roleIds)
            {
                yield return new AppUserRole { UserId = userId, RoleId = item };
            }
        }

        public Guid GetCurrentUserId()
        {
            Guid userId = Guid.Parse(_claimsPrincipal.Claims.Where(x => x.Type == "id").FirstOrDefault().Value);
            return userId;
        }

        private bool SaveData()
        {
            int result = _uow.SaveChanges();
            if (result > 0) return true;
            else return false;
        }
    }
}
