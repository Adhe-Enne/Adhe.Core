using Core.Contracts.Model.Enums;
using System;

namespace Core.Framework.Contracts.Shared.Response
{
    public class LoginResponse
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string DNI { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Image { get; set; }
        public string City { get; set; } = default!;
        public string Country { get; set; } = default!;
        public UserRole Role { get; set; } = UserRole.User;
    }
}
