using Core.Contracts.Model;

namespace Core.SharedServices.Security.Interfaces
{
    public interface IPasswordHasherService<TUser> where TUser : BaseUser
    {
        void HashPassword(TUser user, string password);
        bool VerifyPassword(TUser user, string password);
    }
}