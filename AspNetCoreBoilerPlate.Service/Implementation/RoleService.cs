using AspNetCoreBoilerPlate.Domain.DTO.Role;
using AspNetCoreBoilerPlate.Domain.Models;
using AspNetCoreBoilerPlate.Infrastructure.Repositories.Interface;
using AspNetCoreBoilerPlate.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCoreBoilerPlate.Service.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _uow;
        public RoleService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IEnumerable<RoleDTO> GetAllRoles(TableFiltration tableFiltration)
        {
            var roles = _uow.RoleRepository.GetAll(null, null, null, tableFiltration.Page, tableFiltration.PageSize)
                .Select(q => new RoleDTO
                {
                    Id = q.Id,
                    Name = q.Name
                });
            return roles.ToList();
        }

        public IEnumerable<string> GetRoleNamesByRoleId(IEnumerable<Guid> roleId)
        {
            var roles = _uow.RoleRepository.GetAll(q => roleId.Contains(q.Id))
                .Select(q => q.Name);
            return roles.ToList();
        }

        public bool CreateRole(CreateRoleDTO entity)
        {
            var roleName = entity.Name;
            AppRole appRole = new AppRole { Name = roleName, NormalizedName = roleName.ToUpper() };
            _uow.RoleRepository.Insert(appRole);
            return SaveData();
        }

        public bool UpdateRole(RoleDTO entity)
        {
            var roleId = entity.Id;
            var roleName = entity.Name;
            AppRole appRole = _uow.RoleRepository.GetAll(x => x.Id == roleId).FirstOrDefault();
            appRole.Name = roleName;
            appRole.NormalizedName = roleName.ToUpper();
            _uow.RoleRepository.Update(appRole);
            return SaveData();
        }

        public bool DeleteRole(Guid roleId)
        {
            _uow.RoleRepository.Delete(roleId);
            return SaveData();
        }

        private bool SaveData()
        {
            int result = _uow.SaveChanges();
            if (result > 0) return true;
            else return false;
        }

    }
}
