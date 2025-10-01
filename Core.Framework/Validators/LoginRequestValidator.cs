using Core.Framework.Contracts.Shared.Request;
using FluentValidation;
using System.Linq;

namespace Core.Framework.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio")
                .EmailAddress().WithMessage("El email no es valido")
                .MaximumLength(50).WithMessage("EL email debe tener una longitud maxima de 50 caracteres")
                .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("El email no es valido")
                .Must(email => !string.IsNullOrWhiteSpace(email) && email.Trim().Length > 0)
                .WithMessage("El email no puede ser solo espacios en blanco.")
                .Must(email => email == email.Trim())
                .WithMessage("El email no puede tener espacios en blanco al inicio o al final.")
                .Must(email => !email.Contains("  "))
                .WithMessage("El email no puede contener espacios en blanco consecutivos.")
                .Must(email => !email.Any(char.IsPunctuation) || email.Contains("@") || email.Contains("."))
                .WithMessage("El email no puede contener caracteres especiales, excepto '@' y '.'");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria")
            .MinimumLength(10).WithMessage("La contraseña debe tener una longitud minima de 10 caracteres")
            .MaximumLength(100).WithMessage("La contraseña debe tener una longitud maxima de 100 caracteres")
            .Matches(@"[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayuscula")
            .Matches(@"[a-z]").WithMessage("La contraseña debe contener al menos una letra minuscula")
            .Matches(@"[0-9]").WithMessage("La contraseña debe contener al menos un numero");
            //.Matches(@"[\!\?\*\.]").WithMessage("La contraseña debe contener al menos un caracter especial (!? *.)");
        }
    }
}
