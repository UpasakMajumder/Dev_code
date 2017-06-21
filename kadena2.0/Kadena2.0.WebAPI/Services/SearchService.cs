/*


        private Result SmartSearchProductsWhisperInternal(string text)
        {
            var result = new Result { Success = true, Message = String.Empty };

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            var index = SearchIndexInfoProvider.GetSearchIndexInfo(_SmartSearchProductsIndexStartName + LocalizationContext.CurrentCulture.CultureCode);

            if (index != null)
            {
                // Prepares the search parameters
                SearchParameters parameters = new SearchParameters()
                {
                    SearchFor = String.Format("+({0})", text),
                    SearchSort = "##SCORE##",
                    Path = "/Products/%",
                    CurrentCulture = LocalizationContext.CurrentCulture.CultureCode,
                    DefaultCulture = null,
                    CombineWithDefaultCulture = false,
                    CheckPermissions = false,
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
                System.Data.DataSet searchResults = SearchHelper.Search(parameters);

                if (!DataHelper.DataSourceIsEmpty(searchResults))
                {
                    foreach (DataTable table in searchResults.Tables)
                    {
                        foreach (DataRow dr in table.Rows)
                        {
                            var resultItem = new SearchResultItem { Name = dr[4].ToString(), ImageUrl = dr[7].ToString().Replace("~", "") };
                            var nodeID = Convert.ToInt32(((dr[0].ToString()).Split(";".ToCharArray())[1]).Split("_".ToCharArray())[0]);
                            var node = tree.SelectSingleNode(nodeID, LocalizationContext.CurrentCulture.CultureCode);
                            resultItem.Url = node.AbsoluteURL;

                            result.SearchResult.Add(resultItem);
                        }
                    }
                    return result;
                }
                else
                {
                    return new Result { Success = false, Message = ResHelper.GetString("_mss.Header.Search.NoProductsFound", LocalizationContext.CurrentCulture.CultureCode) };
                }
            }
            else
            {
                return new Result { Success = false, Message = ResHelper.GetString("_mss.Header.Search.SmartSearchIndexNotExists", LocalizationContext.CurrentCulture.CultureCode) };
            }
        }

        

*/