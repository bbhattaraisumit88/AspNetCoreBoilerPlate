using AspNetCoreBoilerPlate.Domain.Models;
using System;
using System.Threading.Tasks;

namespace AspNetCoreBoilerPlate.Infrastructure.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<AppUser> UserRepository { get; }
        IGenericRepository<AppRole> RoleRepository { get; }
        IGenericRepository<AppUserRole> UserRoleRepository { get; }
        IGenericRepository<AppUserClaim> UserClaimRepository { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
