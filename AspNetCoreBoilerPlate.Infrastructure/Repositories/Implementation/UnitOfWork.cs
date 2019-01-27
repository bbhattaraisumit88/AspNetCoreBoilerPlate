using AspNetCoreBoilerPlate.Domain.Models;
using AspNetCoreBoilerPlate.EFCore;
using AspNetCoreBoilerPlate.Infrastructure.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace AspNetCoreBoilerPlate.Infrastructure.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _dbContext;
        IGenericRepository<AppUser> _userRepository;
        IGenericRepository<AppRole> _roleRepository;
        IGenericRepository<IdentityUserRole<Guid>> _userRoleRepository;
        IGenericRepository<IdentityUserClaim<Guid>> _userClaimRepository;


        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException("Context was not supplied");
        }
        public IGenericRepository<AppUser> UserRepository =>
            _userRepository ?? (_userRepository = new GenericRepository<AppUser>(_dbContext));
        public IGenericRepository<AppRole> RoleRepository =>
            _roleRepository ?? (_roleRepository = new GenericRepository<AppRole>(_dbContext));
        public IGenericRepository<IdentityUserRole<Guid>> UserRoleRepository =>
            _userRoleRepository ?? (_userRoleRepository = new GenericRepository<IdentityUserRole<Guid>>(_dbContext));
        public IGenericRepository<IdentityUserClaim<Guid>> UserClaimRepository =>
            _userClaimRepository ?? (_userClaimRepository = new GenericRepository<IdentityUserClaim<Guid>>(_dbContext));
        public int SaveChanges() => _dbContext.SaveChanges();
        public Task<int> SaveChangesAsync() => _dbContext.SaveChangesAsync();
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
