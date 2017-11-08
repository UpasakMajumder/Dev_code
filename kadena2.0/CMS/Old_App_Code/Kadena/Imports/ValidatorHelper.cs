using Kadena.Old_App_Code.Kadena.Imports.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Imports
{
    public static class ValidatorHelper
    {
        public static bool ValidateDto(object dto, out List<string> validationErrors, string errorMessageFormat)
        {
            // validate annotations
            var context = new ValidationContext(dto, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(
                dto, context, validationResults,
                validateAllProperties: true
            );

            validationErrors = new List<string>();
            if (!isValid)
            {
                foreach (var item in validationResults.Where(res => res != ValidationResult.Success))
                {
                    validationErrors.Add(string.Format(errorMessageFormat, item.MemberNames.First(), item.ErrorMessage));
                }
            }

            return isValid;
        }

        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            var parts = email.Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                return parts[1]
                    .Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)
                    .Length >= 2;
            }

            return false;
        }

        public static bool ValidateRole(string role, Role[] roles)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return false;
            }

            return roles.Any(r => r.Description == role);
        }
    }
}