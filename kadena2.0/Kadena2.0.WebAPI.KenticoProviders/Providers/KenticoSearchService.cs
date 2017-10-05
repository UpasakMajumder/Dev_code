using Kadena.WebAPI.KenticoProviders.Contracts;
using CMS.Helpers;
using System;
using CMS.Membership;
using CMS.Search;
using CMS.Localization;
using System.Data;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoSearchService : IKenticoSearchService
    {
        public IEnumerable<DataRow> Search(string phrase, string indexName, string path, int results, bool checkPermissions)
        {
            var index = SearchIndexInfoProvider.GetSearchIndexInfo(indexName);

            if (index == null)
                yield break;

            var searchText = string.Format("+({0})", phrase);
            var culture = LocalizationContext.CurrentCulture.CultureCode;

            var documentSearchCondition = new DocumentSearchCondition { Culture = culture };
            var condition = new SearchCondition(documentCondition: documentSearchCondition);
            searchText = SearchSyntaxHelper.CombineSearchCondition(searchText, condition);

            SearchParameters parameters = new SearchParameters()
            {
                SearchFor = searchText,
                SearchSort = "##SCORE##",
                Path = path,
                CurrentCulture = culture,
                DefaultCulture = null,
                CombineWithDefaultCulture = false,
                CheckPermissions = checkPermissions,
                SearchInAttachments = false,
                User = (UserInfo)MembershipContext.AuthenticatedUser,
                SearchIndexes = index.IndexName,
                StartingPosition = 0,
                DisplayResults = results,
                NumberOfProcessedResults = 5000,
                NumberOfResults = results,
                AttachmentWhere = String.Empty,
                AttachmentOrderBy = String.Empty,
            };

            var dataset = CacheHelper.Cache<DataSet>(() => SearchHelper.Search(parameters), new CacheSettings(1, $"search|{indexName}|{path}|{phrase}|{results}|{checkPermissions}|{culture}"));

            if (dataset != null)
            {
                foreach (DataTable table in dataset.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        yield return dr;
                    }
                }
            }
        }
    }
}
