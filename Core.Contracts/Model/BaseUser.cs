using Core.Contracts.Model.Enums;

namespace Core.Contracts.Model
{
    public class BaseUser : BaseEntity
    {
        public string Email { get; set; } = default!;
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? Name { get; set; } = default!;
        public string? DNI { get; set; } = default!;
        public string? PhoneNumber { get; set; } = default!;
        public string? Image { get; set; }
        public string? City { get; set; } = default!;
        public string? Country { get; set; } = default!; 
        public int FailedLoginAttempts { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
    }
}
