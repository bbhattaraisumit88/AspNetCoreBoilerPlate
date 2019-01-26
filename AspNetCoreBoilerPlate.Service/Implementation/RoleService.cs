using AspNetCoreBoilerPlate.Domain.DTO.Role;
using AspNetCoreBoilerPlate.Domain.Models;
using AspNetCoreBoilerPlate.Infrastructure.Repositories.Interface;
using AspNetCoreBoilerPlate.Service.Interface;
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

        public IEnumerable<RoleDTO> GetAllRoles()
        {
            var roles = _uow.RoleRepository.GetAll().Select(q => new RoleDTO
            {
                Id = q.Id,
                Name = q.Name
            });
            return roles.ToList();
        }

        public bool CreateRole(RoleDTO entity)
        {
            AppRole appRole = new AppRole { Name = entity.Name };
            _uow.RoleRepository.Insert(appRole);
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
