using System;
using System.Linq;
using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using Core.Utilities.Messages;
using FluentValidation;

namespace Core.Aspects.Autofac.Validation
{
    /// <summary>
    ///     ValidationAspect
    /// </summary>
    public class ValidationAspect : MethodInterception
    {
        private readonly Type _validatorType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationAspect"/> class.
        /// </summary>
        /// <param name="validatorType">Validator type.</param>
        public ValidationAspect(Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new ArgumentException(AspectMessages.WrongValidationType);
            }

            _validatorType = validatorType;
        }

        /// <summary>
        /// Validation aspect on before
        /// </summary>
        /// <param name="invocation">invocation</param>
        protected override void OnBefore(IInvocation invocation)
        {
            var validator = (IValidator) Activator.CreateInstance(_validatorType);
            var entityType = _validatorType.BaseType?.GetGenericArguments()[0];
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType);
            foreach (var entity in entities)
            {
                ValidationTool.Validate(validator, entity);
            }
        }
    }
}