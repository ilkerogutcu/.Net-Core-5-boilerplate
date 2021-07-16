using FluentValidation;

namespace Core.CrossCuttingConcerns.Validation
{
    /// <summary>
    /// Validation Tool
    /// </summary>
    public static class ValidationTool
    {
        /// <summary>
        ///     Validate generic entity
        /// </summary>
        /// <param name="validator">Validator.</param>
        /// <param name="entity">Entity.</param>
        public static void Validate(IValidator validator, object entity)
        {
            var context = new ValidationContext<object>(entity);

            var result = validator.Validate(context);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}