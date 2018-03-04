using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Cqrs.Commands;

namespace Weapsy.Framework.Domain
{
    public static class ValidatorExtensions
    {
        public static void ValidateCommand<TCommand>(this IValidator<TCommand> validator, TCommand command) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var validationResult = validator.Validate(command);

            if (!validationResult.IsValid)
                throw new Exception(BuildErrorMesage(validationResult.Errors));
        }

        private static string BuildErrorMesage(IEnumerable<ValidationFailure> errors)
        {
            var errorsText = errors.Select(x => "\r\n - " + x.ErrorMessage).ToArray();
            return "Validation failed: " + string.Join("", errorsText);
        }
    }
}
