using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MyStore.OpenApi.Extensions
{
    public static class ValidationResultExtensions
    {
        public static ModelStateDictionary ToModelState(this ValidationResult source)
        {
            var modelState = new ModelStateDictionary();
            foreach (var error in source.Errors)
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return modelState;
        }
    }
}