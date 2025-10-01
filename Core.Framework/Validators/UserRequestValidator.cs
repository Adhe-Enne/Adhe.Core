using Core.Framework.Contracts.Shared.Request;
using FluentValidation;
using System.Linq;

namespace Core.Framework.Validators
{
    public class UserRequestValidator : AbstractValidator<UserRegisterRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(2).MaximumLength(200)
                .WithMessage("El nombre debe tener entre 2 y 50 caracteres.")
                .Must(name => !string.IsNullOrWhiteSpace(name) && name.Trim().Length > 0)
                .WithMessage("El nombre no puede ser solo espacios en blanco.")
                .Must(name => name == name.Trim())
                .WithMessage("El nombre no puede tener espacios en blanco al inicio o al final.")
                .Must(name => !name.Contains("  "))
                .WithMessage("El nombre no puede contener espacios en blanco consecutivos.")
                .Must(name => !name.Any(char.IsDigit))
                .WithMessage("El nombre no puede contener números.")
                .Must(name => !name.Any(char.IsPunctuation))
                .WithMessage("El nombre no puede contener caracteres especiales.")
                .Must(name => !name.Any(char.IsSymbol))
                .WithMessage("El nombre no puede contener símbolos.")
                .Matches("^[a-zA-ZÀ-ÿ\\s]+$").WithMessage("El nombre debe contener solo caracteres y espacios.");

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
                  .MinimumLength(6).WithMessage("La contraseña debe tener una longitud minima de 6 caracteres")
                  .MaximumLength(100).WithMessage("La contraseña debe tener una longitud maxima de 100 caracteres")
                  .Matches(@"[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayuscula")
                  .Matches(@"[a-z]").WithMessage("La contraseña debe contener al menos una letra minuscula")
                  .Matches(@"[0-9]").WithMessage("La contraseña debe contener al menos un numero");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(26)
                .Matches(@"^\+?[0-9\s\-()]*$").WithMessage("El numero de telefono no es valido")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
                .Must(phone => !string.IsNullOrWhiteSpace(phone) && phone.Trim().Length > 0)
                .WithMessage("El numero de telefono no puede ser solo espacios en blanco.")
                .Must(phone => phone == phone.Trim())
                .WithMessage("El numero de telefono no puede tener espacios en blanco al inicio o al final.")
                .Must(phone => !phone.Contains("  "))
                .WithMessage("El numero de telefono no puede contener espacios en blanco consecutivos.")
                .Must(phone => !phone.Any(char.IsPunctuation) || phone.Contains("+") || phone.Contains("-") || phone.Contains("(") || phone.Contains(")") || phone.Contains(" "))
                .WithMessage("El numero de telefono no puede contener caracteres especiales, excepto '+', '-', '(', ')' y espacios.")
                .Must(phone => !phone.Any(char.IsLetter))
                .WithMessage("El numero de telefono no puede contener letras.")
                .Must(phone => !phone.Any(char.IsSymbol))
                .WithMessage("El numero de telefono no puede contener simbolos.");

            RuleFor(x => x.Country)
                .MaximumLength(30).WithMessage("El pais debe tener una longitud maxima de 30 caracteres")
                .Matches("^[a-zA-ZÀ-ÿ\\s]+$").WithMessage("El pais debe contener solo caracteres y espacios.")
                .Must(country => !string.IsNullOrWhiteSpace(country) && country.Trim().Length > 0)
                .WithMessage("El pais no puede ser solo espacios en blanco.")
                .Must(country => country == country.Trim())
                .WithMessage("El pais no puede tener espacios en blanco al inicio o al final.")
                .Must(country => !country.Contains("  "))
                .WithMessage("El pais no puede contener espacios en blanco consecutivos.")
                .Must(country => !country.Any(char.IsDigit))
                .WithMessage("El pais no puede contener numeros.")
                .Must(country => !country.Any(char.IsPunctuation))
                .WithMessage("El pais no puede contener caracteres especiales.")
                .Must(country => !country.Any(char.IsSymbol))
                .WithMessage("El pais no puede contener simbolos.");

            RuleFor(x => x.City)
                .MaximumLength(30).WithMessage("La ciudad debe tener una longitud maxima de 30 caracteres")
                .Matches("^[a-zA-ZÀ-ÿ\\s]+$").WithMessage("La ciudad debe contener solo caracteres y espacios.")
                .Must(city => !string.IsNullOrWhiteSpace(city) && city.Trim().Length > 0)
                .WithMessage("La ciudad no puede ser solo espacios en blanco.")
                .Must(city => city == city.Trim())
                .WithMessage("La ciudad no puede tener espacios en blanco al inicio o al final.")
                .Must(city => !city.Contains("  "))
                .WithMessage("La ciudad no puede contener espacios en blanco consecutivos.")
                .Must(city => !city.Any(char.IsDigit))
                .WithMessage("La ciudad no puede contener numeros.")
                .Must(city => !city.Any(char.IsPunctuation))
                .WithMessage("La ciudad no puede contener caracteres especiales.")
                .Must(city => !city.Any(char.IsSymbol))
                .WithMessage("La ciudad no puede contener simbolos.");
        }
    }
}
