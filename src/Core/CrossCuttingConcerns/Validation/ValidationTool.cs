#region

using FluentValidation;

#endregion

namespace Core.CrossCuttingConcerns.Validation
{
    public static class ValidationTool
    {
        /// <summary>
        ///     Validate generic entity
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="entity"></param>
        /// <exception cref="ValidationException"></exception>
        public static void Validate(IValidator validator, object entity)
        {
            var context = new ValidationContext<object>(entity);

            var result = validator.Validate(context);
            if (!result.IsValid) throw new ValidationException(result.Errors);
        }
    }
}