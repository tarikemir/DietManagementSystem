﻿using FluentValidation;

namespace DietManagementSystem.Application.Features.Auth.RegisterDietitian;

public class RegisterDietitianCommandValidator : AbstractValidator<RegisterDietitianCommand>
{
    public RegisterDietitianCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.");
    }
}
