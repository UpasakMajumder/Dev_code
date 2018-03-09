using CMS.DataEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.Scheduler;
using CMS.SiteProvider;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceResponses;
using Kadena.Dto.General;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Old_App_Code.Kadena.Constants;
using Kadena.Old_App_Code.Kadena.EmailNotifications;
using Kadena.Old_App_Code.Kadena.Enums;
using Kadena.Old_App_Code.Kadena.PDFHelpers;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Container.Default;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Shoppingcart
{
    public class ShoppingCartHelper
    {
        private static ShoppingCartInfo Cart { get; set; }
        private const string _serviceUrlShippingSettingKey = "KDA_ShippingCostServiceUrl";
        private const string _serviceUrlOrderSettingKey = "KDA_OrderServiceEndpoint";

        /// <summary>
        /// creating estimation DTO
        /// </summary>
        /// <returns></returns>
        public static EstimateDeliveryPriceRequestDto GetEstimationDTO(ShoppingCartInfo cart)
        {
            try
            {
                Cart = cart;
                return new EstimateDeliveryPriceRequestDto
                {
                    SourceAddress = GetSourceAddressFromConfig(),
                    TargetAddress = GetTargetAddress(),
                    Weight = GetWeight(),
                    Provider = CarrierInfoProvider.GetCarrierInfo(Cart.ShippingOption.ShippingOptionCarrierID).CarrierName,
                    ProviderService = Cart.ShippingOption.ShippingOptionName
                };
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "GetEstimationDTO", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Returns order dto
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static OrderDTO CreateOrdersDTO(ShoppingCartInfo cart, int userID, string orderType, decimal shippingCost)
        {
            try
            {
                Cart = cart;
                return new OrderDTO
                {
                    Type = orderType,
                    Campaign = GetCampaign(),
                    BillingAddress = GetBillingAddress(),
                    ShippingAddress = GetBillingAddress(),
                    ShippingOption = ShippingOption(),
                    Customer = GetCustomer(),
                    Site = GetSite(),
                    OrderStatus = new OrderStatusDTO()
                    {
                        KenticoOrderStatusID = DIContainer.Resolve<IKenticoOrderProvider>().GetOrderStatusId("Pending"),
                        OrderStatusName = "PENDING"
                    },
                    PaymentOption = new PaymentOptionDTO()
                    {
                        PaymentOptionName = "NoPaymentRequired",
                        PaymentGatewayCustomerCode = string.Empty,
                        PONumber = string.Empty,
                        TokenId = string.Empty,
                        TransactionKey = string.Empty
                    },
                    NotificationsData = GetNotification(),
                    Items = GetCartItems(),
                    OrderDate = DateTime.Now,
                    TotalPrice = GetOrderTotal(orderType),
                    TotalShipping = shippingCost,
                    OrderCurrency = GetCurrencyDTO(Cart.Currency),
                    TotalTax = 0
                };
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "CreateOrdersDTO", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Returns user Cart IDs based on product type
        /// </summary>
        /// <returns></returns>
        public static List<int> GetCartsByUserID(int userID, ProductType type, int? campaignID)
        {
            try
            {
                return CartPDFHelper.GetLoggedInUserCartData(Convert.ToInt32(type), userID, campaignID).AsEnumerable().Select(x => x.Field<int>("ShoppingCartID")).Distinct().ToList();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "CreateOrdersDTO", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Calling shipping estimation service
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public static BaseResponseDto<EstimateDeliveryPricePayloadDto> CallEstimationService(EstimateDeliveryPriceRequestDto requestBody)
        {
            try
            {
                var microserviceClient = DIContainer.Resolve<IShippingCostServiceClient>();
                var response = microserviceClient.EstimateShippingCost(requestBody).Result;

                if (!response.Success || response.Payload == null)
                {
                    EventLogProvider.LogInformation("DeliveryPriceEstimationClient", "ERROR", $"Call from '{Cart.ShippingOption.ShippingOptionName}' provider to service URL '{SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_serviceUrlShippingSettingKey}")}' resulted with error {response.Error?.Message ?? string.Empty}");
                }
                return response;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "CallEstimationService", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Calling shipping order submission service
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public static BaseResponseDto<string> CallOrderService(OrderDTO requestBody)
        {
            try
            {
                var microserviceClient = DIContainer.Resolve<IOrderSubmitClient>();
                var response = microserviceClient.SubmitOrder(requestBody).Result;

                if (!response.Success || response.Payload == null)
                {
                    EventLogProvider.LogInformation("DeliveryPriceEstimationClient", "ERROR", $"Call from to service URL '{SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.{_serviceUrlOrderSettingKey}")}' resulted with error {response.Error?.Message ?? string.Empty}");
                }
                return response;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "CallOrderService", ex.Message);
                return null;
            }
        }
        /// <summary>
        ///Processes order and returns response
        /// </summary>
        /// <returns></returns>
        public static BaseResponseDto<string> ProcessOrder(ShoppingCartInfo cart, int userID, string orderType, OrderDTO ordersDTO, decimal shippingCost = default(decimal))
        {
            try
            {
                if (ordersDTO != null && ordersDTO.Campaign != null)
                {
                    UpdateDistributorsBusinessUnit(ordersDTO.Campaign.DistributorID);
                }
                var response = CallOrderService(ordersDTO);
                return response;
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("Kadena_CMSWebParts_Kadena_Cart_CartCheckout", "ProcessOrder", ex.Message);
                return null;
            }
        }


        /// <summary>
        /// Updates business unit for distributor
        /// </summary>
        /// <param name="distributorID">Distributor ID</param>
        public static void UpdateDistributorsBusinessUnit(int distributorID)
        {
            AddressInfo distributor = AddressInfoProvider.GetAddressInfo(distributorID);
            long businessUnitNumber = ValidationHelper.GetLong(Cart.GetValue("BusinessUnitIDForDistributor"), default(long));
            if (distributor != null && businessUnitNumber != default(long))
            {
                distributor.SetValue("BusinessUnit", businessUnitNumber);
                AddressInfoProvider.SetAddressInfo(distributor);
            }
        }
        /// <summary>
        /// getting total weight
        /// </summary>
        /// <returns></returns>
        private static WeightDto GetWeight()
        {
            try
            {
                var weight = Cart.CartItems.Sum(x => (x.CartItemUnits * x.UnitWeight));
                return new WeightDto { Unit = SKUMeasuringUnits.Lb, Value = weight };
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "GetWeight", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Gets target shipping address
        /// </summary>
        /// <returns></returns>
        private static AddressDto GetTargetAddress()
        {
            try
            {
                var distributorID = Cart.GetIntegerValue("ShoppingCartDistributorID", default(int));
                var distributorAddress = AddressInfoProvider.GetAddresses().WhereEquals("AddressID", distributorID).FirstOrDefault();
                var addressLines = new[]{
                                            distributorAddress.GetStringValue("AddressLine1",string.Empty),
                                            distributorAddress.GetStringValue("AddressLine2",string.Empty)
                                        }.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
                var country = CountryInfoProvider.GetCountries().WhereEquals("CountryID", distributorAddress.GetStringValue("AddressCountryID", string.Empty))
                                    .Column("CountryTwoLetterCode").FirstOrDefault();
                var state = StateInfoProvider.GetStates().WhereEquals("StateID", distributorAddress.GetStringValue("AddressStateID", string.Empty)).Column("StateCode").FirstOrDefault();
                return new AddressDto()
                {
                    City = distributorAddress.GetStringValue("AddressCity", string.Empty),
                    Country = country?.CountryTwoLetterCode,
                    Postal = distributorAddress.GetStringValue("AddressZip", string.Empty),
                    State = state?.StateCode,
                    StreetLines = addressLines
                };
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "GetTargetAddress", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Gets source  address
        /// </summary>
        /// <returns></returns>
        private static AddressDto GetSourceAddressFromConfig()
        {
            try
            {
                var addressLines = new[]
                                    {
                                        SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderAddressLine1"),
                                        SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderAddressLine2")
                                    }.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();

                return new AddressDto()
                {
                    City = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderCity"),
                    Country = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderCountry"),
                    Postal = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderPostal"),
                    State = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderState"),
                    StreetLines = addressLines
                };
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "GetSourceAddressFromConfig", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Returns campaign details
        /// </summary>
        /// <returns></returns>
        private static CampaignDTO GetCampaign()
        {
            try
            {
                return new CampaignDTO
                {
                    ID = Cart.GetValue("ShoppingCartCampaignID", default(int)),
                    ProgramID = Cart.GetValue("ShoppingCartProgramID", default(int)),
                    DistributorID = Cart.GetIntegerValue("ShoppingCartDistributorID", 0)
                };
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "GetCampaign", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Gets target shipping address
        /// </summary>
        /// <returns></returns>
        private static AddressDTO GetBillingAddress()
        {
            try
            {
                var distributorID = Cart.GetIntegerValue("ShoppingCartDistributorID", default(int));
                var distributorAddress = AddressInfoProvider.GetAddresses().WhereEquals("AddressID", distributorID).FirstOrDefault();
                var country = CountryInfoProvider.GetCountries().WhereEquals("CountryID", distributorAddress.GetStringValue("AddressCountryID", string.Empty)).FirstOrDefault();
                var state = StateInfoProvider.GetStates().WhereEquals("StateID", distributorAddress.GetStringValue("AddressStateID", string.Empty)).FirstOrDefault();
                return new AddressDTO()
                {
                    KenticoAddressID = distributorAddress.AddressID,
                    AddressLine1 = distributorAddress.AddressLine1,
                    AddressLine2 = distributorAddress.AddressLine2,
                    City = distributorAddress.AddressCity,
                    State = state.StateCode,
                    Zip = distributorAddress.GetStringValue("AddressZip", string.Empty),
                    KenticoCountryID = distributorAddress.AddressCountryID,
                    Country = country.CountryName,
                    isoCountryCode = country.CountryTwoLetterCode,
                    KenticoStateID = distributorAddress.AddressStateID,
                    AddressPersonalName = distributorAddress.AddressPersonalName,
                    AddressCompanyName = distributorAddress.GetStringValue("CompanyName", string.Empty)
                };
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "GetBillingAddress", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Returns shipping details
        /// </summary>
        /// <returns></returns>
        private static ShippingOptionDTO ShippingOption()
        {
            try
            {
                var carrier = CarrierInfoProvider.GetCarrierInfo(Cart.ShippingOption.ShippingOptionCarrierID);
                return new ShippingOptionDTO
                {
                    KenticoShippingOptionID = Cart.ShoppingCartShippingOptionID,
                    ShippingService = Cart.ShippingOption.ShippingOptionCarrierServiceName,
                    ShippingCompany = carrier != null ? carrier.CarrierName : Cart.ShippingOption.ShippingOptionName,
                    CarrierCode = Cart.ShippingOption.GetStringValue("ShippingOptionSAPName", string.Empty)
                };
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "ShippingOption", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Returns customer details
        /// </summary>
        /// <returns></returns>
        private static CustomerDTO GetCustomer()
        {
            try
            {
                var settingKeyValue = DIContainer.Resolve<IKenticoResourceService>().GetSettingsKey("KDA_SoldToGeneralInventory");
                var distributorID = Cart.GetIntegerValue("ShoppingCartDistributorID", default(int));
                var distributorAddress = AddressInfoProvider.GetAddresses().WhereEquals("AddressID", distributorID).FirstOrDefault();
                var customer = CustomerInfoProvider.GetCustomerInfo(distributorAddress.AddressCustomerID);
                return new CustomerDTO
                {
                    FirstName = customer.CustomerFirstName,
                    LastName = customer.CustomerLastName,
                    KenticoCustomerID = customer.CustomerID,
                    Email = customer.CustomerEmail,
                    CustomerNumber = settingKeyValue,
                    KenticoUserID = customer.CustomerUserID,
                    Phone = customer.CustomerPhone
                };
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "GetCustomer", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Returns site details
        /// </summary>
        /// <returns></returns>
        private static SiteDTO GetSite()
        {
            var settingKeyValue = DIContainer.Resolve<IKenticoResourceService>().GetSettingsKey("KDA_ErpCustomerId");
            return new SiteDTO
            {
                KenticoSiteID = SiteContext.CurrentSiteID,
                KenticoSiteName = SiteContext.CurrentSiteName,
                ErpCustomerId = settingKeyValue
            };
        }

        /// <summary>
        /// Returns notification detaills
        /// </summary>
        /// <returns></returns>
        private static List<NotificationInfoDto> GetNotification()
        {
            try
            {
                return new List<NotificationInfoDto> {
                                        new NotificationInfoDto {
                                                Email=Cart.Customer.CustomerEmail,
                                                Language=LocalizationContext.CurrentCulture.CultureCode
                                        }
                           };
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "GetNotification", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Returns Shopping cart Items
        /// </summary>
        /// <returns></returns>
        private static List<OrderItemDTO> GetCartItems()
        {
            List<OrderItemDTO> items = new List<OrderItemDTO>();
            try
            {
                Cart.CartItems.ForEach(item =>
                {
                    items.Add(new OrderItemDTO
                    {
                        SKU = new SKUDTO
                        {
                            KenticoSKUID = item.SKUID,
                            Name = item.SKU.SKUName,
                            SKUNumber = item.SKU.SKUNumber
                        },
                        UnitCount = item.CartItemUnits,
                        UnitOfMeasure = SKUMeasuringUnits.EA,
                        UnitPrice = ValidationHelper.GetDecimal(item.UnitPrice, default(decimal)),
                        TotalPrice = ValidationHelper.GetDecimal(item.TotalPrice, default(decimal))
                    });
                });
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "GetCartItems", ex.Message);
                return null;
            }
            return items;
        }

        /// <summary>
        /// Returns order total
        /// </summary>
        /// <param name="inventoryType"></param>
        /// <returns></returns>
        private static CurrencyDTO GetCurrencyDTO(CurrencyInfo currency)
        {
            try
            {
                return new CurrencyDTO
                {
                    KenticoCurrencyID = currency.CurrencyID,
                    CurrencyCode = currency.CurrencyCode
                };
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "GetOrderTotal", ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Returns order total
        /// </summary>
        /// <param name="inventoryType"></param>
        /// <returns></returns>
        private static decimal GetOrderTotal(string type)
        {
            try
            {
                if (type == OrderType.prebuy)
                {
                    return ValidationHelper.GetDecimal(Cart.TotalItemsPrice, default(decimal));
                }
                else
                {
                    return default(decimal);
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "GetOrderTotal", ex.Message);
                return default(decimal);
            }
        }

        /// <summary>
        /// returns Shipping total
        /// </summary>
        /// <param name="inventoryType"></param>
        /// <returns></returns>
        public static BaseResponseDto<EstimateDeliveryPricePayloadDto> GetOrderShippingTotal(ShoppingCartInfo cart)
        {
            try
            {
                EstimateDeliveryPriceRequestDto estimationdto = GetEstimationDTO(cart);
                return CallEstimationService(estimationdto);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "GetOrderTotal", ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Closing the campaign
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ProcessOrders(int campaignID)
        {
            try
            {
                Campaign campaign = CampaignProvider.GetCampaigns()
                    .WhereEquals("NodeSiteID", SiteContext.CurrentSiteID)
                    .WhereEquals("CampaignID", campaignID)
                    .FirstObject;
                if (campaign != null)
                {
                    var _failedOrders = DIContainer.Resolve<IFailedOrderStatusProvider>();
                    _failedOrders.UpdateCampaignOrderStatus(campaign.CampaignID);
                    TaskInfo runTask = TaskInfoProvider.GetTaskInfo(ScheduledTaskNames.PrebuyOrderCreation, SiteContext.CurrentSiteID);
                    if (runTask != null)
                    {
                        runTask.TaskRunInSeparateThread = true;
                        runTask.TaskEnabled = true;
                        runTask.TaskData = $"{campaign.CampaignID}|{SiteContext.CurrentSiteID}";
                        SchedulingExecutor.ExecuteTask(runTask);
                    }
                    var users = UserInfoProvider.GetUsers();
                    if (users != null)
                    {
                        foreach (var user in users)
                        {
                            ProductEmailNotifications.CampaignEmail(campaign.DocumentName, user.Email, "CampaignCloseEmail", campaignURL: campaign.AbsoluteURL);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogException("Kadena_CMSWebParts_Kadena_Cart_FailedOrdersCheckout", "ProcessOrders", ex, SiteContext.CurrentSiteID, ex.Message);
            }
        }
        /// <summary>
        /// update available sku quantity
        /// </summary>
        /// <param name="inventoryType"></param>
        /// <returns></returns>
        public static void UpdateAvailableSKUQuantity(ShoppingCartInfo cart)
        {
            try
            {
                var product = DIContainer.Resolve<IKenticoProductsProvider>();
                cart.CartItems.ForEach(cartItem =>
                {
                    product.SetSkuAvailableQty(cartItem.SKUID, cartItem.CartItemUnits);
                });
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "UpdateAvailableSKUQuantity", ex.Message);
            }
        }
        /// <summary>
        /// update available sku quantity
        /// </summary>
        /// <param name="inventoryType"></param>
        /// <returns></returns>
        public static void UpdateAllocatedProductQuantity(ShoppingCartInfo cart,int userID)
        {
            try
            {
                var productProvider = DIContainer.Resolve<IKenticoProductsProvider>();
                cart.CartItems.ForEach(cartItem =>
                {
                    var campProduct = CampaignsProductProvider.GetCampaignsProducts().WhereEquals("NodeSKUID", cartItem?.SKUID).Columns("CampaignsProductID,EstimatedPrice").FirstOrDefault();
                    if(campProduct!=null)
                    productProvider.UpdateAllocatedProductQuantityForUser(campProduct.GetIntegerValue("CampaignsProductID",0), userID,cartItem.CartItemUnits);
                });
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "UpdateAvailableSKUQuantity", ex.Message);
            }
        }

        /// <summary>
        /// returns open campaign
        /// </summary>
        /// <param name="inventoryType"></param>
        /// <returns></returns>
        public static Campaign GetOpenCampaign()
        {
            try
            {
                return CampaignProvider.GetCampaigns().Columns("CampaignID,Name,StartDate,EndDate")
                                    .WhereEquals("OpenCampaign", true)
                                    .Where(new WhereCondition().WhereEquals("CloseCampaign", false).Or()
                                    .WhereEquals("CloseCampaign", null))
                                    .WhereEquals("NodeSiteID", SiteContext.CurrentSiteID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "GetOrderTotal", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Updates remaining budget for every order placed.
        /// </summary>
        /// <param name="orderDetails"></param>
        /// <returns></returns>
        public static void UpdateRemainingBudget(OrderDTO orderDetails, int userID)
        {
            try
            {
                var campaignFiscalYear = DIContainer.Resolve<IKenticoCampaignsProvider>().GetCampaignFiscalYear(orderDetails.Campaign.ID);
                var totalToBeDeducted = orderDetails.TotalPrice + orderDetails.TotalShipping;
                var fiscalYear = orderDetails.Type == OrderType.generalInventory ?
                                 ValidationHelper.GetString(orderDetails.OrderDate.Year, string.Empty) :
                                 orderDetails.Type == OrderType.prebuy ? campaignFiscalYear : string.Empty;
                DIContainer.Resolve<IkenticoUserBudgetProvider>().UpdateUserBudgetAllocationRecords(userID, fiscalYear, totalToBeDeducted);
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "UpdateRemainingBudget", ex.Message);
            }
        }
    }
}