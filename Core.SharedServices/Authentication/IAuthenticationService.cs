using Core.Contracts.Model;

namespace Core.SharedServices.Authentication
{
    public interface IAuthenticationService<TUser> where TUser : BaseUser
    {
        Task<TUser?> AuthenticateAsync(string email, string password);
        Task<TUser> RegisterAsync(TUser user, string password);
        Task ChangePasswordAsync(Guid idUser, string currentPassword, string newPassword);
    }
}