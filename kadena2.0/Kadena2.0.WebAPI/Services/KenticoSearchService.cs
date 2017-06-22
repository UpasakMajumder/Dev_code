using Kadena.WebAPI.Contracts;
using CMS.Ecommerce;
using Kadena.WebAPI.Models;
using AutoMapper;
using System.Linq;
using CMS.Helpers;
using System;
using CMS.DocumentEngine;
using CMS.Membership;
using CMS.Search;
using CMS.Localization;
using System.Data;

namespace Kadena.WebAPI.Services
{
    public class KenticoSearchService : IKenticoSearchService
    {
        private readonly IMapper mapper;
        private readonly IKenticoResourceService resources;

        public KenticoSearchService(IMapper mapper, IKenticoResourceService resources)
        {
            this.mapper = mapper;
            this.resources = resources;
        }

        public DeliveryAddress[] GetCustomerAddresses(string addressType = null)
        {
            var customer = ECommerceContext.CurrentCustomer;
            var query = AddressInfoProvider.GetAddresses(customer.CustomerID);
            if (!string.IsNullOrWhiteSpace(addressType))
            {
                query = query.Where($"AddressType ='{addressType}'");
            }
            var addresses = query.ToArray();
            return mapper.Map<DeliveryAddress[]>(addresses);
        }


        public DataSet Search(string phrase, string indexName, string path, bool checkPermissions)
        {
            var index = SearchIndexInfoProvider.GetSearchIndexInfo(indexName); 

            if (index == null)
                return null;
            
            SearchParameters parameters = new SearchParameters()
            {
                SearchFor = String.Format("+({0})", phrase),
                SearchSort = "##SCORE##",
                Path = path,
                CurrentCulture = LocalizationContext.CurrentCulture.CultureCode,
                DefaultCulture = null,
                CombineWithDefaultCulture = false,
                CheckPermissions = checkPermissions,
                SearchInAttachments = false,
                User = (UserInfo)MembershipContext.AuthenticatedUser,
                SearchIndexes = index.IndexName,
                StartingPosition = 0,
                DisplayResults = 3,
                NumberOfProcessedResults = 5000,
                NumberOfResults = 3,
                AttachmentWhere = String.Empty,
                AttachmentOrderBy = String.Empty,
            };

            DataSet searchResults = SearchHelper.Search(parameters);
            return searchResults;
        }
    }
}
