using Microsoft.AspNetCore.Identity;
using System;

namespace AspNetCoreBoilerPlate.Domain.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
