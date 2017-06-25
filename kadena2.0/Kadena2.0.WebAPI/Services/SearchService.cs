using Kadena.WebAPI.Contracts;
using AutoMapper;
using System.Data;
using Kadena.WebAPI.Models.Search;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Kadena.WebAPI.Services
{
    public class SearchService : ISearchService
    {
        private readonly IMapper mapper;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoSearchService kenticoSearch;
        private readonly IKenticoProviderService kenticoProvider;

        public SearchService(IMapper mapper, IKenticoResourceService resources, IKenticoSearchService kenticoSearch, IKenticoProviderService kenticoProvider)
        {
            this.mapper = mapper;
            this.resources = resources;
            this.kenticoSearch = kenticoSearch;
            this.kenticoProvider = kenticoProvider;
        }

        public SearchResultPage Search(string phrase)
        {
            var searchResultPages = SearchPages(phrase);
            var searchResultProducts = SearchProducts(phrase);

            return new SearchResultPage()
            {
                Pages = searchResultPages,
                Products = searchResultProducts
            };
        }

        public AutocompleteResponse Autocomplete(string phrase)
        {
            var searchResultPages = SearchPages(phrase);
            var searchResultProducts = SearchProducts(phrase);

            var result = new AutocompleteResponse()
            {
                Pages = new AutocompletePages()
                {
                    Url = "/",
                    Items = mapper.Map<List<AutocompletePage>>(searchResultPages)
                },
                Products = new AutocompleteProducts()
                {
                    Url = "/",
                    Items = searchResultProducts.Select(p => new AutocompleteProduct()
                        {
                            Id = p.Id,
                            Category = "Cat", // TODO
                            Image = "", // TODO
                            Stock = p.Stock,
                            Title = p.Title,
                            Url = "" //TODO
                        }
                    ).ToList()
                },
                Message = string.Empty
            };

            result.UpdateNotFoundMessage(resources.GetResourceString("TODO")); //TODO res string
            return result;
        }

        public List<ResultItemPage> SearchPages(string phrase)
        {
            var searchResultPages = new List<ResultItemPage>();
            var datarowsResults = kenticoSearch.Search(phrase, "KDA_PagesIndex", "/%", true);
            
            foreach (DataRow dr in datarowsResults)
            {
                int documentId = Convert.ToInt32(((dr[0].ToString()).Split(";".ToCharArray())[1]).Split("_".ToCharArray())[0]);

                var resultItem = new ResultItemPage()
                {
                    Id = documentId,
                    Text = dr[5].ToString(),
                    Title = dr[4].ToString(),
                    Url = kenticoProvider.GetDocumentUrl(documentId)
                };

                searchResultPages.Add(resultItem);
            }

            return searchResultPages;
        }

        public List<ResultItemProduct> SearchProducts(string phrase)
        {
            var searchResultProducts = new List<ResultItemProduct>();
            var datarowsResults = kenticoSearch.Search(phrase, "KDA_ProductsIndex", "/Products/%", true);

            foreach (DataRow dr in datarowsResults)
            {
                var resultItem = new ResultItemProduct()
                {
                    Breadcrumbs = new List<string>() { "fakeBreadcrumbs" },
                    ImgUrl = dr[7].ToString().Replace("~", ""),
                    IsFavourite = false,
                    Stock = new Stock()
                    {
                        Text = dr[4].ToString(),
                        Type = ""
                    },
                    UseTemplateBtn = new UseTemplateBtn()
                    {
                        Text = "",
                        Url = ""
                    }
                };

                searchResultProducts.Add(resultItem);
                        
                // var nodeID = Convert.ToInt32(((dr[0].ToString()).Split(";".ToCharArray())[1]).Split("_".ToCharArray())[0]);
                // var node = tree.SelectSingleNode(nodeID, LocalizationContext.CurrentCulture.CultureCode);
                // resultItem.Url = node.AbsoluteURL;
            }

            return searchResultProducts;
        }


    }
}
