using System.ComponentModel.DataAnnotations;
using GaragesStructure.DATA.DTOs.User;
using FluentValidation;

namespace GaragesStructure.DATA.DTOs.User
{
    public class RegisterForm
    {
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string? Password { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        
        [Required]
        [MinLength(2, ErrorMessage = "FullName must be at least 2 characters")]
        public string? FullName { get; set; }

        [Required]
        public Guid? Role { get; set; }

        public int Gender { get; set; }

    }
}

public  class RegisterFormValidator : AbstractValidator<RegisterForm>
{
    public RegisterFormValidator()
    {
        RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(6);
        RuleFor(x => x.Email).NotNull().NotEmpty();
        RuleFor(x => x.FullName).NotNull().NotEmpty();
        RuleFor(x => x.Role).NotNull().NotEmpty();
    }
}