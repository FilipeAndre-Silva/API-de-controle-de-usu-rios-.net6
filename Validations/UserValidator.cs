using FluentValidation;
using UserControl.Models;

namespace UserControl.Validations
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Username).NotNull().WithMessage("Nome do usuário não pode ser nulo");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Nome do usuário não pode ser vazio");

            RuleFor(user => user.Password).NotNull().WithMessage("Nome do usuário não pode ser nulo");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Nome do usuário não pode ser vazio");
            RuleFor(user => user.Password).Length(5,50).WithMessage("A senha deve conter no mínimo cico caracteres.");
        }
    }
}