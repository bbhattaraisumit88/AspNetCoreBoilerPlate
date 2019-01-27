using AspNetCoreBoilerPlate.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace AspNetCoreBoilerPlate.Infrastructure.Repositories.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<AppUser> UserRepository { get; }
        IGenericRepository<AppRole> RoleRepository { get; }
        IGenericRepository<IdentityUserRole<Guid>> UserRoleRepository { get; }
        IGenericRepository<IdentityUserClaim<Guid>> UserClaimRepository { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
