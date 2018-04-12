using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Localization;
using CMS.MacroEngine;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;
using Kadena.Models.Product;
using Kadena.Old_App_Code.CMSModules.Macros.Kadena;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena.Old_App_Code.Kadena.Enums;
using Kadena.Old_App_Code.Kadena.Forms;
using Kadena.Old_App_Code.Kadena.Shoppingcart;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Container.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using static Kadena.Helpers.SerializerConfig;

[assembly: CMS.RegisterExtension(typeof(KadenaMacroMethods), typeof(KadenaMacroNamespace))]

namespace Kadena.Old_App_Code.CMSModules.Macros.Kadena
{
    public class KadenaMacroMethods : MacroMethodContainer
    {
        [MacroMethod(typeof(bool), "Checks whether sku weight is required for given combination of product types", 1)]
        [MacroMethodParam(0, "productTypes", typeof(string), "Product types piped string")]
        public static object IsSKUWeightRequired(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 1)
            {
                throw new NotSupportedException();
            }

            var productTypes = ValidationHelper.GetString(parameters[0], "");
            var product = new Product { ProductType = productTypes };
            var isWeightRequired = new ProductValidator().IsSKUWeightRequired(product);
            return isWeightRequired;
        }

        [MacroMethod(typeof(bool), "Validates product type and sku weight", 1)]
        [MacroMethodParam(0, "productTypes", typeof(string), "Product types piped string")]
        [MacroMethodParam(1, "skuWeight", typeof(double), "SKU weight")]
        public static object IsSKUWeightValid(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 2)
            {
                throw new NotSupportedException();
            }

            var productTypes = ValidationHelper.GetString(parameters[0], "");
            var skuWeight = ValidationHelper.GetDouble(parameters[1], 0, LocalizationContext.CurrentCulture.CultureCode);
            var product = new Product { Weight = skuWeight, ProductType = productTypes };

            var isValid = new ProductValidator().ValidateWeight(product);
            return isValid;
        }

        [MacroMethod(typeof(bool), "Validates combination of product types - static type variant.", 1)]
        [MacroMethodParam(0, "productTypes", typeof(string), "Product types piped string")]
        public static object IsStaticProductTypeCombinationValid(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 1)
            {
                throw new NotSupportedException();
            }
            var selectedProductTypeCodeNames = ValidationHelper.GetString(parameters[0], "").Split("|".ToCharArray());
            // Static product - can be of type Inventory or can be print on demand (POD) or can be withh add-on
            if (selectedProductTypeCodeNames.Contains(ProductTypes.StaticProduct))
            {
                if (selectedProductTypeCodeNames.Contains(ProductTypes.MailingProduct) ||
                    selectedProductTypeCodeNames.Contains(ProductTypes.TemplatedProduct))
                {
                    return false;
                }
            }
            return true;
        }

        [MacroMethod(typeof(bool), "Validates combination of product types - inventory type variant.", 1)]
        [MacroMethodParam(0, "productTypes", typeof(string), "Product types piped string")]
        public static object IsInventoryProductTypeCombinationValid(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 1)
            {
                throw new NotSupportedException();
            }
            var selectedProductTypeCodeNames = ValidationHelper.GetString(parameters[0], "").Split("|".ToCharArray());
            // Inventory product - Must be of type static
            if (selectedProductTypeCodeNames.Contains(ProductTypes.InventoryProduct))
            {
                if (selectedProductTypeCodeNames.Contains(ProductTypes.POD) ||
                    selectedProductTypeCodeNames.Contains(ProductTypes.MailingProduct) ||
                    selectedProductTypeCodeNames.Contains(ProductTypes.TemplatedProduct))
                {
                    return false;
                }
            }
            return true;
        }

        [MacroMethod(typeof(bool), "Validates combination of product types - mailing type variant.", 1)]
        [MacroMethodParam(0, "productTypes", typeof(string), "Product types piped string")]
        public static object IsMailingProductTypeCombinationValid(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 1)
            {
                throw new NotSupportedException();
            }
            var selectedProductTypeCodeNames = ValidationHelper.GetString(parameters[0], "").Split("|".ToCharArray());
            // Mailing product - Must be of type Template
            if (selectedProductTypeCodeNames.Contains(ProductTypes.MailingProduct))
            {
                if (!selectedProductTypeCodeNames.Contains(ProductTypes.TemplatedProduct) ||
                    selectedProductTypeCodeNames.Contains(ProductTypes.StaticProduct) ||
                    selectedProductTypeCodeNames.Contains(ProductTypes.InventoryProduct) ||
                    selectedProductTypeCodeNames.Contains(ProductTypes.POD))
                {
                    return false;
                }
            }
            return true;
        }

        [MacroMethod(typeof(bool), "Validates combination of product types - mailing type variant.", 1)]
        [MacroMethodParam(0, "productTypes", typeof(string), "Product types piped string")]
        public static object IsTemplatedProductTypeCombinationValid(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 1)
            {
                throw new NotSupportedException();
            }
            var selectedProductTypeCodeNames = ValidationHelper.GetString(parameters[0], "").Split("|".ToCharArray());
            // Templated product - Can be of type Mailing
            if (selectedProductTypeCodeNames.Contains(ProductTypes.TemplatedProduct))
            {
                if (selectedProductTypeCodeNames.Contains(ProductTypes.StaticProduct) ||
                    selectedProductTypeCodeNames.Contains(ProductTypes.InventoryProduct) ||
                    selectedProductTypeCodeNames.Contains(ProductTypes.POD))
                {
                    return false;
                }
            }
            return true;
        }

        [MacroMethod(typeof(string), "Validates whether Add to cart button can be displayed.", 1)]
        [MacroMethodParam(0, "productType", typeof(string), "Current product type")]
        [MacroMethodParam(1, "numberOfAvailableProducts", typeof(int), "NumberOfAvailableProducts")]
        [MacroMethodParam(2, "sellOnlyAvailable", typeof(string), "Sell only if available in stock")]
        public static object CanDisplayAddToCartButton(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 3)
            {
                throw new NotSupportedException();
            }

            var productType = (string)parameters[0];
            var numberOfAvailableProducts = (int?)parameters[1];
            var sellOnlyIfAvailable = ValidationHelper.GetBoolean(parameters[2], false);

            return DIContainer.Resolve<IProductsService>().CanDisplayAddToCartButton(productType, numberOfAvailableProducts, sellOnlyIfAvailable);
        }

        [MacroMethod(typeof(string), "Gets formated and localized product availability string.", 1)]
        [MacroMethodParam(0, "productType", typeof(string), "Current product type")]
        [MacroMethodParam(1, "numberOfAvailableProducts", typeof(int), "NumberOfAvailableProducts")]
        [MacroMethodParam(2, "cultureCode", typeof(string), "Current culture code")]
        [MacroMethodParam(3, "numberOfAvailableProductsHelper", typeof(int), "NumberOfAvailableProducts of ECommerce")]
        [MacroMethodParam(4, "unitOfMeasure", typeof(string), "Unit of measure")]
        public static object GetAvailableProductsString(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 5)
            {
                throw new NotSupportedException();
            }

            var productType = (string)parameters[0];
            var numberOfAvailableProducts = (int?)parameters[1];
            var cultureCode = (string)parameters[2];
            var numberOfStockProducts = (int)parameters[3];
            var unitOfmeasure = (string)parameters[4];

            return DIContainer.Resolve<IProductsService>()
                .GetAvailableProductsString(productType, numberOfAvailableProducts, cultureCode, numberOfStockProducts, unitOfmeasure);
        }

        [MacroMethod(typeof(string), "Gets formated and localized product availability string.", 1)]
        [MacroMethodParam(0, "numberOfAvailableProducts", typeof(int), "NumberOfAvailableProducts")]
        [MacroMethodParam(1, "unitOfMeasure", typeof(string), "Unit of measure")]
        [MacroMethodParam(2, "cultureCode", typeof(string), "Current culture code")]
        public static object GetPackagingString(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 3)
            {
                throw new NotSupportedException();
            }

            var numberOfItemsInPackage = (int)parameters[0];
            var unitOfmeasure = (string)parameters[1];
            var cultureCode = (string)parameters[2];

            return DIContainer.Resolve<IProductsService>()
                .GetPackagingString(numberOfItemsInPackage, unitOfmeasure, cultureCode);
        }


        [MacroMethod(typeof(string), "Gets appropriate css class for label that holds amount of products in stock", 1)]
        [MacroMethodParam(0, "numberOfAvailableProducts", typeof(object), "NumberOfAvailableProducts")]
        [MacroMethodParam(1, "productType", typeof(string), "Current product type")]
        [MacroMethodParam(2, "numberOfAvailableProductsHelper", typeof(object), "NumberOfAvailableProducts of ECommerce")]
        public static object GetAppropriateCssClassOfAvailability(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 3)
            {
                throw new NotSupportedException();
            }

            var numberOfAvailableProducts = (int?)parameters[0];
            var productType = (string)parameters[1];
            var numberOfStockProducts = (int)parameters[2];

            var availability = DIContainer.Resolve<IProductsService>().GetInventoryProductAvailability(productType, numberOfAvailableProducts, numberOfStockProducts);

            var mappingCssDictionary = new Dictionary<string, string>()
            {
                { ProductAvailability.Unavailable, "stock stock--unavailable" },
                { ProductAvailability.Available, "stock stock--available" },
                { ProductAvailability.OutOfStock, "stock stock--out" },
            };

            if (mappingCssDictionary.TryGetValue(availability, out string cssClass))
            {
                return cssClass;
            }

            return string.Empty;
        }

        [MacroMethod(typeof(string), "Returns html (set) of products, that could be in a kit with particular product (for particular user).", 1)]
        [MacroMethodParam(0, "nodeID", typeof(int), "ID of Node, that represents the base product")]
        [MacroMethodParam(1, "productsAliasPath", typeof(string), "Alias path for products")]
        public static object GetKitProductsHtml(EvaluationContext context, params object[] parameters)
        {
            var result = string.Empty;
            var selectedItemTemplate = "<div class=\"input__wrapper input__wrapper--disabled\"><input id=\"dom-0\" type=\"checkbox\" class=\"input__checkbox\" data-id=\"{1}\" checked disabled><label for=\"dom-0\" class=\"input__label input__label--checkbox\">{0}</label></div>";
            var itemTemplate = "<div class=\"input__wrapper\"><input id=\"dom-{0}\" type=\"checkbox\" class=\"input__checkbox\" data-id=\"{2}\"><label for=\"dom-{0}\" class=\"input__label input__label--checkbox\">{1}</label></div>";

            if (parameters.Length != 2)
            {
                throw new NotSupportedException();
            }
            var originalNodeID = ValidationHelper.GetInteger(parameters[0], 0);
            var productsPath = ValidationHelper.GetString(parameters[1], string.Empty);

            var tree = new TreeProvider(MembershipContext.AuthenticatedUser);

            if (originalNodeID > 0)
            {
                var originalDocument = tree.SelectSingleNode(originalNodeID, LocalizationContext.CurrentCulture.CultureCode);
                result += string.Format(selectedItemTemplate, originalDocument.DocumentName, originalDocument.NodeID);
            }

            var wantedTypes = new[] { ProductTypes.InventoryProduct, ProductTypes.StaticProduct, "KDA.POD" };

            var allKitDocuments = tree.SelectNodes()
                .OnCurrentSite()
                .Path(productsPath, PathTypeEnum.Children)
                .Culture(LocalizationContext.CurrentCulture.CultureCode)
                .Types("KDA.Product")
                .CheckPermissions();

            var kitDocuments = allKitDocuments.Where(x => IsProductType(x, wantedTypes));

            if (kitDocuments != null)
            {
                var kitList = kitDocuments.ToList();
                for (int i = 1; i <= kitList.Count; i++)
                {
                    var node = kitList[i - 1];
                    if (node.NodeID != originalNodeID && !node.IsLink)
                    {
                        result += string.Format(itemTemplate, i, node.DocumentName, node.NodeID);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Checks if TreeNode's value "ProductType" contains any of given type strings
        /// </summary>
        private static bool IsProductType(TreeNode tn, IEnumerable<string> types)
        {
            var nodeType = tn.GetStringValue("ProductType", string.Empty);
            return types.Any(t => nodeType.Contains(t));
        }

        [MacroMethod(typeof(string), "Returns where codition for one of main navigation repeaters based on enabled modules for customer.", 1)]
        [MacroMethodParam(0, "forEnabledItems", typeof(bool), "For enabled items")]
        public static object GetMainNavigationWhereCondition(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 1)
            {
                throw new NotSupportedException();
            }
            var isForEnabledItems = ValidationHelper.GetBoolean(parameters[0], false);
            string adminCacheKey = IsUserInKadenaAdminRole() ? "_admin" : string.Empty;
            return CacheHelper.Cache(cs => GetMainNavigationWhereConditionInternal(isForEnabledItems), new CacheSettings(20, "Kadena.MacroMethods.GetMainNavigationWhereCondition" + adminCacheKey + "_" + SiteContext.CurrentSiteName + "|" + isForEnabledItems));
        }

        [MacroMethod(typeof(string[]), "Returns array of parsed urls items.", 1)]
        [MacroMethodParam(0, "fieldValue", typeof(string), "Value stored MediaMultiField field")]
        public static object GetUrlsFromMediaMultiField(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 1)
            {
                throw new NotSupportedException();
            }
            var fieldValue = parameters[0] as string;
            var urls = MediaMultiField.GetValues(fieldValue);
            return urls;
        }

        [MacroMethod(typeof(string), "Returns file name from media attachment url.", 1)]
        [MacroMethodParam(0, "url", typeof(string), "Url")]
        public static object GetFilenameFromMediaUrl(EvaluationContext context, params object[] parameters)
        {
            if (parameters.Length != 1)
            {
                throw new NotSupportedException();
            }
            var url = parameters[0] as string;
            var filename = MediaMultiField.ParseFrom(url).Name;
            return filename;
        }

        [MacroMethod(typeof(string), "Returns localized url of the document for current culture.", 1)]
        [MacroMethodParam(0, "aliasPath", typeof(string), "Alias path of the document.")]
        public static object GetLocalizedDocumentUrl(EvaluationContext context, params object[] parameters)
        {
            var aliasPath = ValidationHelper.GetString(parameters[0], string.Empty);
            if (!string.IsNullOrWhiteSpace(aliasPath))
            {
                var documents = DIContainer.Resolve<IKenticoDocumentProvider>();
                return documents.GetDocumentUrl(aliasPath);
            }
            return string.Empty;
        }

        [MacroMethod(typeof(string), "Returns localized urls for language selector.", 1)]
        [MacroMethodParam(0, "aliasPath", typeof(string), "Alias path of the document.")]
        public static object GetUrlsForLanguageSelector(EvaluationContext context, params object[] parameters)
        {
            var aliasPath = ValidationHelper.GetString(parameters[0], string.Empty);
            if (!string.IsNullOrWhiteSpace(aliasPath))
            {
                var kenticoLocalization = DIContainer.Resolve<IKenticoLocalizationProvider>();
                return Newtonsoft.Json.JsonConvert.SerializeObject(kenticoLocalization.GetUrlsForLanguageSelector(aliasPath), CamelCaseSerializer);
            }
            return string.Empty;
        }

        [MacroMethod(typeof(string), "Returns unified date string in Kadena format", 1)]
        [MacroMethodParam(0, "datetime", typeof(DateTime), "DateTime to format")]
        public static object FormatDate(EvaluationContext context, params object[] parameters)
        {
            var datetime = ValidationHelper.GetDateTime(parameters[0], DateTime.MinValue);
            return DIContainer.Resolve<IDateTimeFormatter>().Format(datetime);
        }

        [MacroMethod(typeof(string), "Returns unified date format string", 0)]
        public static object GetDateFormatString(EvaluationContext context, params object[] parameters)
        {
            return DIContainer.Resolve<IDateTimeFormatter>().GetFormatString();
        }

        [MacroMethod(typeof(bool), "Determines whether to show T&C confirmation popup or not.", 0)]
        public static object ShowTaC(EvaluationContext context, params object[] parameters)
        {
            return DIContainer.Resolve<IUserService>().CheckTaC().Show;
        }


        private static string GetMainNavigationWhereConditionInternal(bool isForEnabledItems)
        {
            var result = string.Empty;
            var pageTypes = new List<string>();

            var moduleSettingsMappingsDataClassInfo = DataClassInfoProvider.GetDataClassInfo("KDA.KadenaModuleAndPageTypeConnection");
            if (moduleSettingsMappingsDataClassInfo != null)
            {
                var mappingItems = CustomTableItemProvider.GetItems("KDA.KadenaModuleAndPageTypeConnection");

                if (mappingItems != null)
                {
                    foreach (var mappingItem in mappingItems)
                    {
                        var moduleState = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{mappingItem.GetStringValue("SettingsKeyCodeName", string.Empty)}");
                        if ((isForEnabledItems && moduleState.ToLowerInvariant().Equals(KadenaModuleState.enabled.ToString())) || (!isForEnabledItems && moduleState.ToLowerInvariant().Equals(KadenaModuleState.disabled.ToString())))
                        {
                            pageTypes.Add(mappingItem.GetStringValue("PageTypeCodeName", string.Empty));
                        }
                    }
                }
            }
            foreach (var pageType in pageTypes)
            {
                if (GetRoleBasedAdminAccessModuleStatus(pageType))
                {
                    result += $"ClassName = N'{pageType}' OR ";
                }
            }
            if (result.Length > 0)
            {
                result = result.Substring(0, result.Length - 3);
            }
            else
            {
                result = "1 = 0";
            }
            return result;
        }

        private static bool GetRoleBasedAdminAccessModuleStatus(string className)
        {
            bool status = true;
            if (className.ToLower().Equals("kda.adminmodule"))
            {
                status = IsUserInKadenaAdminRole();
            }
            return status;
        }

        private static bool IsUserInKadenaAdminRole()
        {
            bool isKadenaAdmin = false;
            string adminRoles = SettingsKeyInfoProvider.GetValue("KDA_AdminRoles", SiteContext.CurrentSiteID);
            UserInfo user = MembershipContext.AuthenticatedUser;
            if (user != null && !string.IsNullOrWhiteSpace(adminRoles))
            {
                string[] roles = adminRoles.Split(';');
                foreach (string role in roles)
                {
                    isKadenaAdmin = user.IsInRole(role, SiteContext.CurrentSiteName);
                    if(isKadenaAdmin)
                    {
                        break;
                    }
                }
            }
            return isKadenaAdmin;
        }

        #region TWE macro methods

        /// <summary>
        /// Returns Division name based on Division ID
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [MacroMethod(typeof(string), "Returns Division name based on Division ID", 1)]
        [MacroMethodParam(0, "DivisionID", typeof(int), "DivisionID")]
        public static object GetDivisionName(EvaluationContext context, params object[] parameters)
        {
            try
            {
                int divisionID = ValidationHelper.GetInteger(parameters[0], 0);
                DivisionItem division = CustomTableItemProvider.GetItem<DivisionItem>(divisionID);
                string divisionName = division?.DivisionName ?? string.Empty;
                return divisionName;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena Macro methods", "GetDivisionName", ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns Program name based on Program ID
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [MacroMethod(typeof(string), "Returns Program name based on Program ID", 1)]
        [MacroMethodParam(0, "ProgramID", typeof(int), "ProgramID")]
        public static object GetProgramName(EvaluationContext context, params object[] parameters)
        {
            try
            {
                int programID = ValidationHelper.GetInteger(parameters[0], 0);
                string programName = string.Empty;
                Program program = ProgramProvider.GetPrograms().WhereEquals("NodeSiteID", SiteContext.CurrentSite.SiteID).WhereEquals("ProgramID", programID).Columns("ProgramName").FirstObject;
                programName = program?.ProgramName ?? string.Empty;
                return programName;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena Macro methods", "GetProgramName", ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns Category name based on Category ID
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [MacroMethod(typeof(string), "Returns Category name based on Category ID", 1)]
        [MacroMethodParam(0, "CategoryID", typeof(int), "CategoryID")]
        public static object GetCategoryName(EvaluationContext context, params object[] parameters)
        {
            try
            {
                int categoryID = ValidationHelper.GetInteger(parameters[0], 0);
                string categoryName = string.Empty;
                ProductCategory category = ProductCategoryProvider.GetProductCategories().WhereEquals("NodeSiteID", SiteContext.CurrentSite.SiteID).WhereEquals("ProductCategoryID", categoryID).Columns("ProductCategoryTitle").FirstObject;
                categoryName = category?.ProductCategoryTitle ?? string.Empty;
                return categoryName;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena Macro methods", "GetProgramName", ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns Currently opened campaign name
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [MacroMethod(typeof(string), "Returns Currently opened campaign name", 1)]
        public static string GetCampaignName(EvaluationContext context, params object[] parameters)
        {
            try
            {
                string campaignName = string.Empty;
                var campaign = CampaignProvider.GetCampaigns().Columns("Name")
                                .WhereEquals("OpenCampaign", true)
                                .Where(new WhereCondition().WhereEquals("CloseCampaign", false).Or()
                                .WhereEquals("CloseCampaign", null))
                                .WhereEquals("NodeSiteID", SiteContext.CurrentSiteID).FirstOrDefault();
                if (campaign != null)
                {
                    campaignName = ValidationHelper.GetString(campaign.GetValue("Name"), string.Empty);
                }
                return campaignName;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena Macro methods", "BindPrograms", ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        ///Returns  campaign name by campaignID
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [MacroMethod(typeof(string), "Returns  campaign name by campaignID", 1)]
        [MacroMethodParam(0, "CampaignID", typeof(int), "CampaignID")]
        public static string GetCampaignNameByID(EvaluationContext context, params object[] parameters)
        {
            try
            {
                int campaignID = ValidationHelper.GetInteger(parameters[0], 0);
                string campaignName = string.Empty;
                var campaign = CampaignProvider.GetCampaigns().Columns("Name")
                                .WhereEquals("CampaignID", campaignID)
                                .WhereEquals("NodeSiteID", SiteContext.CurrentSiteID).FirstOrDefault();
                if (campaign != null)
                {
                    campaignName = ValidationHelper.GetString(campaign.GetValue("Name"), string.Empty);
                }
                return campaignName;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena Macro methods", "BindPrograms", ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns shopping cart items count
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [MacroMethod(typeof(string), "Returns cart items count", 1)]
        [MacroMethodParam(0, "userID", typeof(int), "UserID")]
        [MacroMethodParam(1, "inventoryType", typeof(int), "InventoryType")]
        public static object GetCartCountByInventoryType(EvaluationContext context, params object[] parameters)
        {
            try
            {
                int userID = ValidationHelper.GetInteger(parameters[1], default(int));
                int inventoryType = ValidationHelper.GetInteger(parameters[2], default(int));
                int openCampaignID = ValidationHelper.GetInteger(parameters[3], default(int));
                var query = new DataQuery(SQLQueries.getShoppingCartCount);
                QueryDataParameters queryParams = new QueryDataParameters();
                queryParams.Add("@ShoppingCartUserID", userID);
                queryParams.Add("@ShoppingCartInventoryType", inventoryType);
                queryParams.Add("@ShoppingCartCampaignID", openCampaignID);
                var countData = ConnectionHelper.ExecuteScalar(query.QueryText, queryParams, QueryTypeEnum.SQLQuery, true);
                return ValidationHelper.GetInteger(countData, default(int));
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena Macro methods", "BindPrograms", ex.Message);
                return default(int);
            }
        }

        /// <summary>
        /// Returns shopping cart total
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [MacroMethod(typeof(string), "Returns cart items count", 1)]
        [MacroMethodParam(0, "userID", typeof(int), "UserID")]
        [MacroMethodParam(1, "inventoryType", typeof(int), "InventoryType")]
        public static object GetCartTotalByInventoryType(EvaluationContext context, params object[] parameters)
        {
            try
            {
                int userID = ValidationHelper.GetInteger(parameters[1], default(int));
                int inventoryType = ValidationHelper.GetInteger(parameters[2], default(int));
                int openCampaignID = ValidationHelper.GetInteger(parameters[3], default(int));
                if (inventoryType == (Int32)ProductType.PreBuy)
                {
                    var query = new DataQuery(SQLQueries.getShoppingCartTotal);
                    QueryDataParameters queryParams = new QueryDataParameters();
                    queryParams.Add("@ShoppingCartUserID", userID);
                    queryParams.Add("@ShoppingCartInventoryType", inventoryType);
                    queryParams.Add("@ShoppingCartCampaignID", openCampaignID);
                    var cartTotal = ConnectionHelper.ExecuteScalar(query.QueryText, queryParams, QueryTypeEnum.SQLQuery, true);
                    return ValidationHelper.GetDecimal(cartTotal, default(decimal));
                }
                else
                {
                    var loggedInUSerCartIDs = ShoppingCartHelper.GetCartsByUserID(userID, ProductType.GeneralInventory,openCampaignID);
                    decimal cartTotal = 0;
                    loggedInUSerCartIDs.ForEach(cartID =>
                    {
                        var Cart = ShoppingCartInfoProvider.GetShoppingCartInfo(cartID);
                        if (Cart.ShippingOption != null && Cart.ShippingOption.ShippingOptionCarrierServiceName.ToLower() != ShippingOption.Ground)
                        {
                            var estimationdto = new[] { ShoppingCartHelper.GetEstimationDTO(Cart) };
                            var estimation = ShoppingCartHelper.CallEstimationService(estimationdto);
                            cartTotal += ValidationHelper.GetDecimal(estimation?.Payload?[0]?.Cost, default(decimal));
                        }
                    });
                    return ValidationHelper.GetDecimal(cartTotal, default(decimal));
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena Macro methods", "BindPrograms", ex.Message);
                return default(double);
            }
        }

        /// <summary>
        /// Returns Business unit name based on user id
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [MacroMethod(typeof(string), "Returns Business unit name based on user id", 1)]
        [MacroMethodParam(0, "UserID", typeof(int), "UserID")]
        public static object GetBusinessUnits(EvaluationContext context, params object[] parameters)
        {
            try
            {
                int userID = ValidationHelper.GetInteger(parameters[0], 0);
                var data = CustomTableItemProvider.GetItems<UserBusinessUnitsItem>()
                    .WhereEquals("UserID", userID)
                    .Columns("BusinessUnitID")
                    .ToList();
                var buList = new List<string>();
                if (!DataHelper.DataSourceIsEmpty(data))
                {
                    data.ForEach(x =>
                    {
                        var unitName = CustomTableItemProvider.GetItem<BusinessUnitItem>(x.BusinessUnitID);
                        buList.Add(unitName?.GetStringValue("BusinessUnitName", string.Empty) ?? string.Empty);
                    });
                }
                return String.Join(",", buList);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena Macro methods", "GetBusinessUnits", ex.Message);
                return string.Empty;
            }
        }

        [MacroMethod(typeof(string), "Returns localized url of the document for current culture.", 1)]
        [MacroMethodParam(0, "aliasPath", typeof(string), "GUID of the document.")]
        public static object GetLocalizedDocumentUrlByGUID(EvaluationContext context, params object[] parameters)
        {
            Guid pageGUID = ValidationHelper.GetGuid(parameters[0], Guid.Empty);
            if (!pageGUID.Equals(Guid.Empty))
            {
                var documents = DIContainer.Resolve<IKenticoDocumentProvider>();
                return documents.GetDocumentUrl(pageGUID);
            }
            return string.Empty;
        }


        [MacroMethod(typeof(string), "Returns per-site localization string.", 1)]
        [MacroMethodParam(0, "name", typeof(string), "Name of localization project.")]
        public static object GetPerSiteResourceString(EvaluationContext context, params object[] parameters)
        {
            string name = parameters[0] as string;
            return DIContainer.Resolve<IKenticoResourceService>().GetPerSiteResourceString(name);
        }

        /// <summary>
        /// Returns Category name based on Category ID
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [MacroMethod(typeof(string), "Checks uniqueness of value in custom table", 3)]
        [MacroMethodParam(0, "CustomTableClassName", typeof(string), "CustomTableClassName")]
        [MacroMethodParam(1, "ItemID", typeof(int), "ItemID")]
        [MacroMethodParam(2, "UniqueValueFieldName", typeof(string), "UniqueValueFieldName")]
        [MacroMethodParam(3, "FieldValue", typeof(string), "FieldValue")]
        public static object CheckUniqueInMyTable(EvaluationContext context, params object[] parameters)
        {
            try
            {
                string customTableClassName = ValidationHelper.GetString(parameters[0], string.Empty);
                int itemID = ValidationHelper.GetInteger(parameters[1], 0);
                string uniqueValueFieldName = ValidationHelper.GetString(parameters[2], string.Empty);
                string fieldValue = ValidationHelper.GetString(parameters[3], string.Empty);
                DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
                if (customTable != null)
                {
                    return CustomTableItemProvider.GetItems(customTableClassName, uniqueValueFieldName + "='" + fieldValue + "' AND ItemID!=" + itemID).Count <= 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena Macro methods", "CheckUniqueInMyTable", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Returns if any campaign is open
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [MacroMethod(typeof(bool), "Returns if any campaign is open", 0)]
        public static object IsCampaignOpen(EvaluationContext context, params object[] parameters)
        {
            bool IsOpen = false;
            try
            {
                var campaign = CampaignProvider.GetCampaigns()
                    .WhereEquals("OpenCampaign", true)
                    .WhereEqualsOrNull("CloseCampaign", false)
                    .WhereEquals("NodeSiteID", SiteContext.CurrentSite.SiteID)
                    .FirstOrDefault();
                return campaign != null ? campaign.StartDate <= DateTime.Now.Date && campaign.EndDate >= DateTime.Now.Date ? true : false : false;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena Macro methods", "IsCampaignOpen", ex.Message);
                return IsOpen;
            }
        }
        /// <summary>
        /// Returns if any campaign is open
        /// </summary>
        /// <param name="context"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [MacroMethod(typeof(int), "Returns campaignid if any campaign is open",0)]
        public static object OpenCampaignID(EvaluationContext context, params object[] parameters)
        {
            try
            {
                var campaign = ShoppingCartHelper.GetOpenCampaign();
                return campaign!=null? campaign.CampaignID:default(int);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena Macro methods", "OpenCampaignID", ex.Message);
                return default(int);
            }
        }

        #endregion TWE macro methods
    }
}