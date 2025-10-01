namespace Core.Framework.Contracts.Shared.Request
{
    public class UserRegisterRequest
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Dni { get; set; }
    }
}
