using FluentValidation;

namespace CourierCounter.Models.ApiModels.Validator
{
    public class RegistrationValidator : AbstractValidator<RegistrationViewModel>
    {
        public RegistrationValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(50).WithMessage("Name cannot exceed  50 characters")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name can only contain letters and spaces");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format"); 

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"\d").WithMessage("Password must contain at least one number")
                .Matches(@"[@$!%*?&]").WithMessage("Password must contain at least one special character");
        }
    }
}
