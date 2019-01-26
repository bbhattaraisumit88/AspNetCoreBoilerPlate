using AspNetCoreBoilerPlate.Domain.Models;
using AspNetCoreBoilerPlate.EFCore;
using AspNetCoreBoilerPlate.Infrastructure.Repositories.Interface;
using System;
using System.Threading.Tasks;

namespace AspNetCoreBoilerPlate.Infrastructure.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _dbContext;
        IGenericRepository<AppUser> _userRepository;
        IGenericRepository<AppRole> _roleRepository;
        IGenericRepository<AppUserRole> _userRoleRepository;
        IGenericRepository<AppUserClaim> _userClaimRepository;


        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException("Context was not supplied");
        }
        public IGenericRepository<AppUser> UserRepository =>
            _userRepository ?? (_userRepository = new GenericRepository<AppUser>(_dbContext));
        public IGenericRepository<AppRole> RoleRepository =>
            _roleRepository ?? (_roleRepository = new GenericRepository<AppRole>(_dbContext));
        public IGenericRepository<AppUserRole> UserRoleRepository =>
            _userRoleRepository ?? (_userRoleRepository = new GenericRepository<AppUserRole>(_dbContext));
        public IGenericRepository<AppUserClaim> UserClaimRepository =>
            _userClaimRepository ?? (_userClaimRepository = new GenericRepository<AppUserClaim>(_dbContext));
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
