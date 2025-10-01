using Core.Contracts.Model;

namespace Core.SharedServices.Security.Interfaces
{
    public interface ITokenService<TUser> where TUser : BaseUser
    {
        string CreateToken(TUser user);
        Guid? GetUserIdFromToken(string token);
    }
}