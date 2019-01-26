using System;
using System.Collections.Generic;

namespace AspNetCoreBoilerPlate.Domain.DTO.User
{
    public class CreateUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ICollection<Guid> RoleIdList { get; set; }
        public CreateUserDTO()
        {
            this.RoleIdList = new List<Guid>();
        }
    }
}
