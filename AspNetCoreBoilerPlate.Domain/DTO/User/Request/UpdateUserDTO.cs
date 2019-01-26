using System;

namespace AspNetCoreBoilerPlate.Domain.DTO.User
{
    public class UpdateUserDTO : CreateUserDTO
    {
        public Guid UserId { get; set; }
    }
}
