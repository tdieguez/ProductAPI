using FluentValidation;
using MyStore.OpenApi.V1.Controllers;

namespace MyStore.OpenApi.V1.Validators
{
    public class CategoryValidator : AbstractValidator<CategoryDto>
    {
        public CategoryValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(c => c.Name)
                .MaximumLength(150);
        }
    }
}