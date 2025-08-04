

using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators;

public class CheckoutOrderCommandValidatorV2 : AbstractValidator<CheckoutOrderCommandV2>
{
    public CheckoutOrderCommandValidatorV2()
    {
        RuleFor(o => o.UserName)
            .NotEmpty()
            .WithMessage("{Username} is required.")
            .NotNull()
            .MaximumLength(70)
            .WithMessage("{Username} must not exceed 70 characters");
        RuleFor(o => o.TotalPrice)
            .NotEmpty()
            .WithMessage("{TotalPrice} is required.")
            .GreaterThan(-1)
            .WithMessage("{TotalPrice} must be greater than or equal to 0.");
    }
}
