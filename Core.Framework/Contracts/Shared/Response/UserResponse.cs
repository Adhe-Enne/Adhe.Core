using System;

namespace Core.Framework.Contracts.Shared.Response
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Role { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
