using FluentValidation;
using MyStore.OpenApi.V1.Dtos;

namespace MyStore.OpenApi.V1.Validators
{
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(p => p.Description)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(p => p.Price)
                .GreaterThanOrEqualTo(0);

            RuleFor(p => p.Category)
                .MaximumLength(50);
        }
    }
}