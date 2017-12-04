using CMS.EventLog;
using CMS.Membership;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Imports.Users
{
    public sealed class AddressImporter : UserImportService
    {
        public IEnumerable<int> UserIds { get; set; }

        public override ImportResult Process(byte[] importFileData, ExcelType type, int siteId)
        {
            if ((UserIds?.Count() ?? 0) == 0)
            {
                throw new ArgumentOutOfRangeException("No users selected to upload addresses to");
            }

            var rows = GetExcelRows(importFileData, type);
            var addresses = GetDtosFromExcelRows<AddressDto>(rows);
            statusMessages.Clear();

            foreach (var userId in UserIds)
            {
                var user = UserInfoProvider.GetUserInfo(userId);

                if (user == null)
                {
                    statusMessages.Add($"Nonexisting user : {userId}");
                    continue;
                }

                var customer = EnsureCustomer(user, siteId);

                var currentItemNumber = 1;
                foreach (var addressDto in addresses)
                {
                    try
                    {
                        List<string> validationResults;
                        if (!ValidatorHelper.ValidateDto(addressDto, out validationResults, "field {0} - {1}"))
                        {
                            validationResults.Sort();

                            statusMessages.Add($"Item number {currentItemNumber} has invalid values ({ string.Join("; ", validationResults) })");
                            continue;
                        }

                        CreateCustomerAddress(customer.CustomerID, new UserDto
                        {
                            Country = addressDto.Country,
                            State = addressDto.State,
                            AddressLine = addressDto.AddressLine,
                            AddressLine2 = addressDto.AddressLine2,
                            City = addressDto.City,
                            ContactName = addressDto.ContactName,
                            PostalCode = addressDto.PostalCode,
                            PhoneNumber = addressDto.PhoneNumber

                        });
                    }
                    catch (Exception ex)
                    {
                        statusMessages.Add("There was an error when processing item number " + currentItemNumber);
                        EventLogProvider.LogException("Import addresses", "EXCEPTION", ex);
                    }

                    currentItemNumber++;
                }
            }

            return new ImportResult
            {
                AllMessagesCount = statusMessages.AllMessagesCount,
                ErrorMessages = statusMessages.ToArray()
            };
        }
    }
}