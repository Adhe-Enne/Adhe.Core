using AutoMapper;
using Core.Contracts.Model;
using Core.Framework.Contracts.Api;
using Core.Framework.Contracts.Api.Interfaces;
using Core.Framework.Contracts.Shared.Request;
using Core.Framework.Contracts.Shared.Response;
using Core.SharedServices;
using Core.SharedServices.Authentication;
using Core.SharedServices.Security.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
namespace Core.Framework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController<TUser>(
          ILogger<AuthController<TUser>> logger,
          IGenericService<TUser> service,
          IMapper mapper,
          IAuthenticationService<TUser> authService,
          ITokenService<TUser> tokenService
      ) : BaseController(logger, mapper)
          where TUser : BaseUser, new()
    {
        private readonly IAuthenticationService<TUser> _authService = authService;
        private readonly ITokenService<TUser> _tokenService = tokenService;

        [HttpPost("register")]
        public async Task<ActionResult<IApiResult>> Register([FromBody] UserRegisterRequest dto)
        {
            var entity = _mapper.Map<TUser>(dto);
            if (entity == null)
                return ResponseApi(new ApiResult("Error al crear usuario", HttpStatusCode.BadRequest));

            // Puedes sobrescribir UserAdded en un controller concreto si lo necesitas
            // entity.UserAdded = "OneSelf";

            await _authService.RegisterAsync(entity, dto.Password);

            var result = HandleSuccess("Usuario registrado correctamente", dto.Email);
            return ResponseApi(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ILoginResult<LoginResponse>>> Login([FromBody] LoginRequest dto)
        {
            var user = await _authService.AuthenticateAsync(dto.Email, dto.Password);
            var result = new LoginResult<LoginResponse>();

            if (user == null)
            {
                result.Set(HandleSuccess("Usuario no válido", HttpStatusCode.Unauthorized));
                return ResponseApi(result);
            }

            result.Token = _tokenService.CreateToken(user);
            result.UserData = _mapper.Map<LoginResponse>(user);
            result.Set(HandleSuccess("Login exitoso"));

            return ResponseApi(result);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<ActionResult<IApiResult>> ChangePassword([FromBody] ChangePasswordRequest dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _authService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);

            var result = HandleSuccess("Contraseña cambiada correctamente");
            return ResponseApi(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public Task<ActionResult<IApiResult>> Logout()
        {
            var result = HandleSuccess("Logout exitoso");
            return Task.FromResult(ResponseApi(result));
        }
    }
}
