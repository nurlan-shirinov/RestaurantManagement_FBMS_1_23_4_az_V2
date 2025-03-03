using Application.CQRS.Categories.Commands.Requests;
using FluentValidation;

namespace Application.CQRS.Categories.Validator;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(5)
            .EmailAddress();
    }
}