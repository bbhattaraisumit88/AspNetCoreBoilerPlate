using System;
using System.Collections.Generic;

namespace AspNetCoreBoilerPlate.Domain.DTO.User
{
    public class UserResponseDTO
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<string> RoleNames { get; set; }
    }
}
