using GaragesStructure.DATA.DTOs.User;
using FluentValidation;

namespace GaragesStructure.DATA.DTOs.User
{
    public class LoginForm
    {
        
        public String Email { get; set; }
        public String Password { get; set; }
    }
}

public class LoginFormValidator : AbstractValidator<LoginForm>
{
    public LoginFormValidator()
    {
        RuleFor(x => x.Email).NotNull().NotEmpty();
        RuleFor(x => x.Password).NotNull().NotEmpty();
    }
}