using AspNetCoreBoilerPlate.Domain.DTO.User;
using AspNetCoreBoilerPlate.Domain.Models;
using AspNetCoreBoilerPlate.Infrastructure.Repositories.Interface;
using AspNetCoreBoilerPlate.Service.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AspNetCoreBoilerPlate.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRoleService _roleService;
        private readonly ClaimsPrincipal _claimsPrincipal;
        public UserService(IUnitOfWork uow, IRoleService roleService,
            IHttpContextAccessor httpContextAccessor)
        {
            _uow = uow;
            _roleService = roleService;
            _claimsPrincipal = httpContextAccessor.HttpContext.User;
        }
        public IEnumerable<UserResponseDTO> GetAllUsers(TableFiltration tableFiltration)
        {
            var userList = (_uow.UserRepository.GetAll(null, null, null, tableFiltration.Page, tableFiltration.PageSize)
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
            AppUser entity = new AppUser
            {
                Id = Guid.NewGuid(),
                Email = userDTO.Email,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                UserName = userDTO.UserName
            };
            var userRoles = AddUserRoles(entity.Id, userDTO.RoleIdList);
            var userClaims = AddUserClaims(entity.Id, GetRoleNames(userDTO.RoleIdList));
            _uow.UserRepository.Insert(entity);
            _uow.UserRoleRepository.InsertRange(userRoles);
            _uow.UserClaimRepository.InsertRange(userClaims);
            return SaveData();
        }
        public bool UpdateUser(UpdateUserDTO userDTO)
        {
            Guid userId = userDTO.UserId;
            AppUser entity = _uow.UserRepository.GetAll(x => x.Id == userId).FirstOrDefault();
            entity.Email = userDTO.Email;
            entity.FirstName = userDTO.FirstName;
            entity.LastName = userDTO.LastName;
            entity.UserName = userDTO.UserName;
            var userRoleList = _uow.UserRoleRepository.GetAll(q => q.UserId == userId).ToList();
            var userClaimsList = _uow.UserClaimRepository.GetAll(q => q.UserId == userId).ToList();
            if (userRoleList.Any())
            {
                _uow.UserRoleRepository.DeleteRange(userRoleList);
            }
            if (userClaimsList.Any())
            {
                _uow.UserClaimRepository.DeleteRange(userClaimsList);
            }

            var userRoles = AddUserRoles(userId, userDTO.RoleIdList);
            var userClaims = AddUserClaims(userId, GetRoleNames(userDTO.RoleIdList));
            _uow.UserRepository.Update(entity);
            _uow.UserRoleRepository.InsertRange(userRoles);
            _uow.UserClaimRepository.InsertRange(userClaims);
            return SaveData();
        }
        public bool DeleteUser(Guid userId)
        {
            var roleIdList = _uow.UserRoleRepository.Find(x => x.UserId == userId)
                .Select(x => x.RoleId).ToList();
            _uow.UserRoleRepository.Delete(userId, roleIdList);
            _uow.UserRepository.Delete(userId);
            return SaveData();
        }
        private IEnumerable<AppUserRole> AddUserRoles(Guid userId, ICollection<Guid> roleIds)
        {
            foreach (var item in roleIds)
            {
                yield return new AppUserRole { UserId = userId, RoleId = item };
            }
        }
        private IEnumerable<string> GetRoleNames(ICollection<Guid> roleIds)
        {
            return _roleService.GetRoleNamesByRoleId(roleIds);
        }
        private IEnumerable<AppUserClaim> AddUserClaims(Guid userId, IEnumerable<string> roles)
        {
            foreach (var item in roles)
            {
                yield return new AppUserClaim { UserId = userId, ClaimType = ClaimTypes.Role, ClaimValue = item };
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
