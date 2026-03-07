using Core.Contracts.Model;
using Core.SharedServices.Exceptions;
using Core.SharedServices.Security.Interfaces;

namespace Core.SharedServices.Authentication
{
    public class AuthenticationService<TUser> : IAuthenticationService<TUser> where TUser : BaseUser
    {
        private readonly IGenericService<TUser> _userService;
        private readonly IPasswordHasherService<TUser> _passwordHasher;
        private const int MaxFailedAccessAttempts = 5;
        private const int LockoutDurationInMinutes = 15;

        public AuthenticationService(IGenericService<TUser> userService, IPasswordHasherService<TUser> passwordHasher)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        public async Task<TUser?> AuthenticateAsync(string email, string password)
        {
            var user = await _userService.FindAsync(u => u.Email == email && u.IsActive);

            if (user == null)
                throw new BusinessException("El Email no pertenece a ningun usuario",
                    Contracts.Exceptions.EnumBusinessErrorCode.UserNotFound,
                    "Invalid credentials");


            // Verifica si el usuario está bloqueado
            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow)
                throw new BusinessException($"El usuario ha sido bloqueado hasta {user.LockoutEnd.Value}.",
                             Contracts.Exceptions.EnumBusinessErrorCode.UserLockedOut,
                             "User is locked out");

            await ValidateAttempts(user, password);

            user.FailedLoginAttempts = 0;
            user.LockoutEnd = null;
            await _userService.UpdateAsync(user);

            return user;
        }

        public async Task<TUser> RegisterAsync(TUser user, string password)
        {
            if (await _userService.ExistsAsync(x => x.IsActive && (x.Email == user.Email || x.DNI == user.DNI)))
            {
                throw new BusinessException(password, Contracts.Exceptions.EnumBusinessErrorCode.UserAlreadyExists, "User already exists");
            }

            _passwordHasher.HashPassword(user, password);
            await _userService.InsertAsync(user);
            return user;
        }

        private async Task ValidateAttempts(TUser user, string password)
        {
            // Verifica si el usuario está bloqueado
            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow)
                throw new BusinessException($"El usuario ha sido bloqueado hasta {user.LockoutEnd.Value}.",
                             Contracts.Exceptions.EnumBusinessErrorCode.UserLockedOut,
                             "User is locked out");

            if (!_passwordHasher.VerifyPassword(user, password))
            {
                user.FailedLoginAttempts++;

                if (user.FailedLoginAttempts >= MaxFailedAccessAttempts)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(LockoutDurationInMinutes);
                    user.FailedLoginAttempts = 0;
                }

                await _userService.UpdateAsync(user);

                if (user.FailedLoginAttempts == MaxFailedAccessAttempts - 1)
                    throw new BusinessException($"Contraseña invalida, intente nuevamente, solo le queda 1 reintento)",
                        Contracts.Exceptions.EnumBusinessErrorCode.InvalidPassword,
                        "Invalid Password");

                throw new BusinessException($"Contraseña invalida, intente nuevamente (recuerde que solo tiene {MaxFailedAccessAttempts} Reintentos)",
                    Contracts.Exceptions.EnumBusinessErrorCode.InvalidPassword,
                    "Invalid Password");
            }
        }

        public async Task ChangePasswordAsync(Guid idUser, string currentPassword, string newPassword)
        {
            TUser? user = await _userService.GetByIdAsync(id: idUser);

            if (user == null || !user.IsActive)
                throw new BusinessException("Usuario no encontrado", Contracts.Exceptions.EnumBusinessErrorCode.UserNotFound, "User not found");

            // Verificar contraseña actual
            if (!_passwordHasher.VerifyPassword(user, currentPassword))
                throw new BusinessException("La contraseña actual es incorrecta", Contracts.Exceptions.EnumBusinessErrorCode.InvalidPassword, "Invalid current password");

            // Hashear y guardar la nueva contraseña
            _passwordHasher.HashPassword(user, newPassword);
            await _userService.UpdateAsync(user);
        }
    }
}