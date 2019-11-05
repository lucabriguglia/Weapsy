using FluentValidation;
using FluentValidation.Results;
using Kledex.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weapsy.Domain.Handlers
{
    public static class ValidatorExtensions
    {
        public static async Task ValidateCommandAsync<TCommand>(this IValidator<TCommand> validator, TCommand command) where TCommand : ICommand
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var validationResult = await validator.ValidateAsync(command);

            if (!validationResult.IsValid)
                throw new Exception(BuildErrorMesage(validationResult.Errors));
        }

        private static string BuildErrorMesage(IEnumerable<ValidationFailure> errors)
        {
            var errorsText = errors.Select(x => $"\r\n - {x.ErrorMessage}").ToArray();
            return $"Validation failed: {string.Join("", errorsText)}";
        }
    }
}
