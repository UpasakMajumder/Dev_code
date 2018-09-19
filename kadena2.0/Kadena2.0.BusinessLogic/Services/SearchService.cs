using Kadena.BusinessLogic.Contracts;
using AutoMapper;
using System.Data;
using Kadena.Models.Search;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Web;
using System;
using Kadena.Models.SiteSettings;

namespace Kadena.BusinessLogic.Services
{
    public class SearchService : ISearchService
    {
        private readonly IMapper mapper;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoSiteProvider siteProvider;
        private readonly IKenticoSearchService kenticoSearch;
        private readonly IKenticoProductsProvider products;
        private readonly IKenticoDocumentProvider documents;
        private readonly IImageService imageService;

        public SearchService(IMapper mapper, IKenticoResourceService resources, IKenticoSiteProvider site,
            IKenticoSearchService kenticoSearch,  IKenticoProductsProvider products, IKenticoDocumentProvider documents, IImageService imageService)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.siteProvider = site ?? throw new ArgumentNullException(nameof(site));
            this.kenticoSearch = kenticoSearch ?? throw new ArgumentNullException(nameof(kenticoSearch));
            this.products = products ?? throw new ArgumentNullException(nameof(products));
            this.documents = documents ?? throw new ArgumentNullException(nameof(documents));
            this.imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        }

        public SearchResultPage Search(string phrase, int results = 100)
        {
            var searchResultPages = SearchPages(phrase, results);
            var searchResultProducts = SearchProducts(phrase, results);

            return new SearchResultPage()
            {
                NoResultMessage = resources.GetResourceString("Kadena.Search.NoResults"),
                Pages = searchResultPages,
                Products = searchResultProducts
            };
        }

        public AutocompleteResponse Autocomplete(string phrase, int results = 3)
        {
            var searchResultPages = SearchPages(phrase, results);
            var searchResultProducts = SearchProducts(phrase, results);
            var serpUrl = documents.GetDocumentUrl(resources.GetSiteSettingsKey(Settings.KDA_SerpPageUrl));

            var result = new AutocompleteResponse()
            {
                Pages = new AutocompletePages()
                {
                    Url = $"{serpUrl}?phrase={HttpUtility.UrlEncode(phrase)}&tab=pages",
                    Items = mapper.Map<List<AutocompletePage>>(searchResultPages)
                },
                Products = new AutocompleteProducts()
                {
                    Url = $"{serpUrl}?phrase={HttpUtility.UrlEncode(phrase)}&tab=products",
                    Items = searchResultProducts.Select(p => new AutocompleteProduct()
                    {
                        Id = p.Id,
                        Category = p.Category,
                        Image = p.ImgUrl,
                        Stock = p.Stock,
                        Title = p.Title,
                        Url = documents.GetDocumentUrl(p.Id)
                    }
                ).ToList()
                },
                Message = string.Empty
            };

            result.UpdateNotFoundMessage(resources.GetResourceString("Kadena.Search.NoResults"));
            return result;
        }

        public List<ResultItemPage> SearchPages(string phrase, int results)
        {
            var site = siteProvider.GetKenticoSite();
            var searchResultPages = new List<ResultItemPage>();
            var indexName = $"KDA_PagesIndex.{site.Name}";
            var datarowsResults = kenticoSearch.Search(phrase, indexName, "/%", results, true);

            foreach (DataRow dr in datarowsResults)
            {
                int documentId = GetDocumentId(dr[0]);

                var resultItem = new ResultItemPage()
                {
                    Id = documentId,
                    Text = Regex.Replace(dr[5].ToString(), @"<[^>]+>|&nbsp;", "").Trim(),
                    Title = dr[4].ToString(),
                    Url = documents.GetDocumentUrl(documentId)
                };

                searchResultPages.Add(resultItem);
            }

            return searchResultPages;
        }

        public List<ResultItemProduct> SearchProducts(string phrase, int results)
        {
            var site = siteProvider.GetKenticoSite();
            var searchResultProducts = new List<ResultItemProduct>();
            var indexName = $"KDA_ProductsIndex.{site.Name}";
            var productsPath = resources.GetSiteSettingsKey(Settings.KDA_ProductsPageUrl)?.TrimEnd('/');
            var datarowsResults = kenticoSearch.Search(phrase, indexName, productsPath + "/%", results, true);

            foreach (DataRow dr in datarowsResults)
            {
                int documentId = GetDocumentId(dr[0]);
                var resultItem = new ResultItemProduct()
                {
                    Id = documentId,
                    Title = dr[4].ToString(),
                    Breadcrumbs = documents.GetBreadcrumbs(documentId),
                    IsFavourite = false,
                };

                var product = products.GetProductByDocumentId(documentId);
                if (product != null)
                {
                    // fill in SKU image if teaser is empty
                    if (string.IsNullOrEmpty(resultItem.ImgUrl))
                    {
                        resultItem.ImgUrl = imageService.GetThumbnailLink(product.ImageUrl);
                    }
                    resultItem.Category = product.Category;
                    if (!string.IsNullOrWhiteSpace(product.Availability))
                    {
                        resultItem.Stock = new Stock()
                        {
                            Text = string.Format(resources.GetResourceString("Kadena.Search.NumberOfAvailableProducts"), product.StockItems),
                            Type = product.Availability
                        };
                    }
                    resultItem.UseTemplateBtn = new UseTemplateBtn()
                    {
                        Text = resources.GetResourceString("Kadena.Search.GoToDetailButton"),
                        Url = product.DocumentUrl
                    };
                }


                searchResultProducts.Add(resultItem);
            }

            return searchResultProducts;
        }

        private int GetDocumentId(object o)
        {
            int documentId = 0;
            var parsedId = o.ToString().Split(new char[] { ';' })?[0].Split(new char[] { '_' })[0];
            int.TryParse(parsedId, out documentId);
            return documentId;
        }
    }
}
