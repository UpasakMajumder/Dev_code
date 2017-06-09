﻿using Kadena.WebAPI.Models;

namespace Kadena.WebAPI.Contracts
{
    public interface IKenticoResourceService
    {
        string GetResourceString(string name);

        string GetSettingsKey(string key);

        KenticoSite GetKenticoSite();

        Currency GetSiteCurrency();

        string GetDefaultSiteCompanyName();

        string GetDefaultSitePersonalName();

        int GetOrderStatusId(string name);
    }
}