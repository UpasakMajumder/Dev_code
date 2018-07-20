using Kadena.BusinessLogic.Contracts;
using Kadena.Models.Settings;
using Kadena.Models.SiteSettings;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.BusinessLogic.Services
{
    public class DialogService : IDialogService
    {
        private readonly IKenticoResourceService resources;
        private readonly IKenticoLocalizationProvider kenticoLocalization;

        public DialogService(IKenticoResourceService resources, IKenticoLocalizationProvider kenticoLocalization)
        {
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.kenticoLocalization = kenticoLocalization ?? throw new ArgumentNullException(nameof(kenticoLocalization));
        }

        public IEnumerable<DialogField> GetAddressFields()
        {
            var countries = kenticoLocalization.GetCountries();
            var states = kenticoLocalization.GetStates();
            var defaultCountryId = resources.GetSiteSettingsKey<int>(Settings.KDA_AddressDefaultCountry);
            return new[] {
                    new DialogField
                    {
                        Id = "customerName",
                        Label = resources.GetResourceString("Kadena.Settings.CustomerName"),
                        Type = "text"
                    },
                    new DialogField
                    {
                        Id = "company",
                        Label = resources.GetResourceString("Kadena.ContactForm.Company"),
                        Type = "text",
                        IsOptional = true
                    },
                    new DialogField
                    {
                        Id = "address1",
                        Label = resources.GetResourceString("Kadena.Settings.Addresses.AddressLine1"),
                        Type = "text"
                    },
                    new DialogField
                    {
                        Id = "address2",
                        Label = resources.GetResourceString("Kadena.Settings.Addresses.AddressLine2"),
                        IsOptional = true,
                        Type = "text"
                    },
                    new DialogField
                    {
                        Id = "city",
                        Label = resources.GetResourceString("Kadena.Settings.Addresses.City"),
                        Type = "text"
                    },
                    new DialogField
                    {
                        Id = "state",
                        Label = resources.GetResourceString("Kadena.Settings.Addresses.State"),
                        IsOptional = false,
                        Type = "select",
                        Values = new List<object>()
                    },
                    new DialogField
                    {
                        Id = "zip",
                        Label = resources.GetResourceString("Kadena.Settings.Addresses.Zip"),
                        Type = "text"
                    },
                    new DialogField
                    {
                        Id = "country",
                        Label = resources.GetResourceString("Kadena.Settings.Addresses.Country"),
                        Type = "select",
                        Values = countries
                                .GroupJoin(states, c => c.Id, s => s.CountryId, (c, sts) => (object) new
                                {
                                    Id = c.Id.ToString(),
                                    Name = c.Name,
                                    IsDefault = (c.Id == defaultCountryId),
                                    Values = sts.Select(s => new
                                    {
                                        Id = s.Id.ToString(),
                                        Name = s.StateDisplayName
                                    }).ToArray()
                                }).ToList()
                    },
                    new DialogField
                    {
                        Id = "phone",
                        Label = resources.GetResourceString("Kadena.ContactForm.Phone"),
                        IsOptional = true,
                        Type = "text"
                    },
                    new DialogField
                    {
                        Id = "email",
                        Label = resources.GetResourceString("Kadena.ContactForm.Email"),
                        Type = "text"
                    }
            };
        }
    }
}
