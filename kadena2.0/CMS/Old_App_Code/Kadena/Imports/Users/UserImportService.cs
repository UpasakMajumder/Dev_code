using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Globalization;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Models;
using Kadena.Old_App_Code.Kadena.Email;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Imports.Users
{
    public class UserImportService
    {
        public ImportResult ProcessImportFile(byte[] importFileData, ExcelType type, int siteID, string passwordEmailTemplateName)
        {
            var rows = new ExcelReader().ReadDataFromExcelFile(importFileData, type);
            if (rows.Count <= 1)
            {
                throw new Exception("The file contains no data");
            }

            var site = SiteInfoProvider.GetSiteInfo(siteID);
            if (site == null)
            {
                throw new Exception("Invalid site id " + siteID);
            }
            var siteRoles = new RoleProvider().GetAllRoles(site.SiteID);

            var header = rows.First();
            var mapRowToUser = ImportHelper.GetImportMapper<UserDto>(header);
            var users = rows.Skip(1)
                .Select(row => mapRowToUser(row))
                .ToList();

            var statusMessages = new List<string>();
            var emailService = new EmailService();

            var currentItemNumber = 1;
            foreach (var userDto in users)
            {
                if (IsExistingUser(userDto.Email))
                {
                    statusMessages.Add("Skipped duplicate email address " + userDto.Email);
                    continue;
                }

                try
                {
                    List<string> validationResults;
                    if (!ValidateImportItem(userDto, siteRoles, out validationResults))
                    {
                        // sort errors by field
                        validationResults.Sort();

                        statusMessages.Add($"Item number {currentItemNumber} has invalid values ({ string.Join("; ", validationResults) })");
                        continue;
                    }

                    var newUser = CreateUser(userDto, site);
                    var newCustomer = CreateCustomer(newUser.UserID, siteID, userDto);
                    CreateCustomerAddress(newCustomer.CustomerID, userDto);

                    emailService.SendResetPasswordEmail(newUser, passwordEmailTemplateName, site.SiteName);
                }
                catch (Exception ex)
                {
                    statusMessages.Add("There was an error when processing item number " + currentItemNumber);
                    EventLogProvider.LogException("Import users", "EXCEPTION", ex);
                }

                currentItemNumber++;
            }

            return new ImportResult
            {
                ErrorMessages = statusMessages.ToArray()
            };
        }

        private bool ValidateImportItem(UserDto userDto, Role[] roles, out List<string> validationErrors)
        {
            var errorMessageFormat = "field {0} - {1}";

            // validate annotations
            var context = new ValidationContext(userDto, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(
                userDto, context, validationResults,
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

            // validate special rules
            if (!ValidateEmail(userDto.Email))
            {
                validationErrors.Add(string.Format(errorMessageFormat, nameof(userDto.Email), "Not a valid email address"));
            }
            if (!ValidateRole(userDto.Role, roles))
            {
                validationErrors.Add(string.Format(errorMessageFormat, nameof(userDto.Role), "Not a valid role"));
            }

            return isValid;
        }

        private bool ValidateEmail(string email)
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

        private bool ValidateRole(string role, Role[] roles)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return false;
            }

            return roles.Any(r => r.Description == role);
        }

        private CountryInfo FindCountry(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
            {
                return null;
            }

            var code = country.ToUpper();
            return CountryInfoProvider.GetCountries()
                .WhereStartsWith("CountryDisplayName", country)
                .Or()
                .WhereEquals("CountryTwoLetterCode", code)
                .Or()
                .WhereEquals("CountryThreeLetterCode", code)
                .FirstOrDefault();
        }

        private StateInfo FindState(string state)
        {
            if (string.IsNullOrWhiteSpace(state))
            {
                return null;
            }

            var code = state.ToUpper();
            return StateInfoProvider.GetStates()
                .WhereStartsWith("StateDisplayName", state)
                .Or()
                .WhereEquals("StateTwoLetterCode", code)
                .Or()
                .WhereEquals("StateThreeLetterCode", code)
                .FirstOrDefault();
        }

        private void CreateCustomerAddress(int customerID, UserDto userDto)
        {
            var country = FindCountry(userDto.Country);
            if (country == null)
            {
                EventLogProvider.LogInformation("Import users", "INFO", $"Skipping creation of address of user { userDto.Email }. Reason - invalid country.");
                return;
            }

            var state = FindState(userDto.State);

            var addressNameFields = new[] { $"{userDto.FirstName} {userDto.LastName}", userDto.AddressLine, userDto.AddressLine2, userDto.City }
                .Where(af => !string.IsNullOrWhiteSpace(af));
            var newAddress = new AddressInfo
            {
                AddressName = string.Join(", ", addressNameFields),
                AddressLine1 = userDto.AddressLine,
                AddressLine2 = userDto.AddressLine2,
                AddressCity = userDto.City,
                AddressZip = userDto.PostalCode,
                AddressPersonalName = userDto.ContactName,
                AddressPhone = userDto.PhoneNumber,
                AddressCustomerID = customerID,
                AddressCountryID = country.CountryID,
                AddressStateID = state?.StateID
            };
            newAddress.SetValue("AddressType", AddressType.Shipping.Code);

            AddressInfoProvider.SetAddressInfo(newAddress);
        }

        private CustomerInfo CreateCustomer(int userID, int siteID, UserDto userDto)
        {
            var newCustomer = new CustomerInfo
            {
                CustomerFirstName = userDto.FirstName,
                CustomerLastName = userDto.LastName,
                CustomerEmail = userDto.Email,
                CustomerSiteID = siteID,
                CustomerUserID = userID,
                CustomerCompany = userDto.Company,
                CustomerOrganizationID = userDto.OrganizationID,
                CustomerPhone = userDto.PhoneNumber,
                CustomerTaxRegistrationID = userDto.TaxRegistrationID,
                CustomerCountryID = FindCountry(userDto.Country)?.CountryID ?? 0
            };

            CustomerInfoProvider.SetCustomerInfo(newCustomer);
            return newCustomer;
        }

        private UserInfo CreateUser(UserDto userDto, SiteInfo site)
        {
            var newUser = new UserInfo
            {
                UserName = userDto.Email,
                UserEnabled = true,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                FullName = userDto.FirstName + " " + userDto.LastName,
                Email = userDto.Email,
                SiteIndependentPrivilegeLevel = CMS.Base.UserPrivilegeLevelEnum.None
            };
            var newUserSettings = newUser.UserSettings ?? new UserSettingsInfo();
            newUserSettings.UserPhone = userDto.PhoneNumber;

            UserInfoProvider.SetUserInfo(newUser);

            UserInfoProvider.AddUserToSite(newUser.UserName, site.SiteName);

            var role = new RoleProvider().GetAllRoles(site.SiteID)
                .FirstOrDefault(r => r.Description == userDto.Role);
            if (role == null)
            {
                throw new Exception("Invalid user role");
            }
            UserInfoProvider.AddUserToRole(newUser.UserID, role.ID);

            return newUser;
        }

        private bool IsExistingUser(string emailAddress)
        {
            return UserInfoProvider.GetUsers()
                .WhereEquals("Email", emailAddress)
                .Any();
        }
    }
}