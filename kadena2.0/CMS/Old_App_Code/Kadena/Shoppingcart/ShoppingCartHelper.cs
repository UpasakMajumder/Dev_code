using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Localization;
using CMS.SiteProvider;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceResponses;
using Kadena.Dto.General;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Helpers;
using Kadena.Old_App_Code.Kadena.Enums;
using Kadena.Old_App_Code.Kadena.PDFHelpers;
using Kadena.WebAPI.KenticoProviders;
using Kadena2.MicroserviceClients.Clients;
using Kadena2.WebAPI.KenticoProviders;
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
                    ProviderService = Cart.ShippingOption.ShippingOptionCarrierServiceName
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
        public static OrderDTO CreateOrdersDTO(ShoppingCartInfo cart, int userID, string type)
        {
            try
            {
                Cart = cart;
                return new OrderDTO
                {
                    Type = type,
                    Campaign = GetCampaign(),
                    BillingAddress = GetBillingAddress(),
                    ShippingAddress = GetBillingAddress(),
                    ShippingOption = ShippingOption(),
                    Customer = GetCustomer(),
                    Site = GetSite(),
                    NotificationsData = GetNotification(),
                    Items = GetCartItems(),
                    KenticoOrderCreatedByUserID = userID,
                    LastModified = DateTime.Now,
                    OrderDate = DateTime.Now,
                    TotalPrice = GetOrderTotal(ProductType.GeneralInventory),
                    TotalShipping = GetOrderShippingTotal(ProductType.GeneralInventory)
                };
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("ShoppingCartHelper", "CreateOrdersDTO", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Returns logged in user Cart IDs based on product type
        /// </summary>
        /// <returns></returns>
        public static List<int> GetLoggeedInUserCarts(int userID, ProductType type)
        {
            try
            {
                return CartPDFHelper.GetLoggedInUserCartData(Convert.ToInt32(ProductType.GeneralInventory), userID).AsEnumerable().Select(x => x.Field<int>("ShoppingCartID")).Distinct().ToList();
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
                var microserviceClient = new ShippingCostServiceClient(ProviderFactory.MicroProperties);
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
                var microserviceClient = new OrderSubmitClient(ProviderFactory.MicroProperties);
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
        /// getting total weight
        /// </summary>
        /// <returns></returns>
        private static WeightDto GetWeight()
        {
            try
            {
                //We are keeping constant values at present,this will be done in sprint-4 once sku weight is added
                // var weight = Cart.CartItems.Sum(x => (x.CartItemUnits * x.UnitWeight));
                return new WeightDto { Unit = "Lb", Value = 0.5 };
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
                    ID = Cart.GetIntegerValue("ShoppingCartCampaignID", 0),
                    ProgramID = Cart.GetIntegerValue("ShoppingCartProgramID", 0),
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
                    State = state.StateName,
                    Zip = distributorAddress.GetStringValue("AddressZip", string.Empty),
                    KenticoCountryID = distributorAddress.AddressCountryID,
                    Country = country.CountryName,
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
                return new ShippingOptionDTO
                {
                    KenticoShippingOptionID = Cart.ShoppingCartShippingOptionID,
                    ShippingService = Cart.ShippingOption.ShippingOptionCarrierServiceName,
                    ShippingCompany = Cart.ShippingOption.ShippingOptionName,
                    CarrierCode = Cart.ShippingOption.ShippingOptionCarrierServiceName
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
                var distributorID = Cart.GetIntegerValue("ShoppingCartDistributorID", default(int));
                var distributorAddress = AddressInfoProvider.GetAddresses().WhereEquals("AddressID", distributorID).FirstOrDefault();
                var customer = CustomerInfoProvider.GetCustomerInfo(distributorAddress.AddressCustomerID);
                return new CustomerDTO
                {
                    FirstName = customer.CustomerFirstName,
                    LastName = customer.CustomerLastName,
                    KenticoCustomerID = customer.CustomerID,
                    Email = customer.CustomerEmail,
                    CustomerNumber = customer.CustomerFirstName,
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
            return new SiteDTO
            {
                KenticoSiteID = SiteContext.CurrentSiteID,
                KenticoSiteName = SiteContext.CurrentSiteName
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
                        //We are keeping constant values at present,this will be done in sprint 4 once sku weight is added
                        UnitOfMeasure = "Lb",
                        UnitPrice = ValidationHelper.GetDecimal(item.UnitPrice, default(decimal))
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
        private static decimal GetOrderTotal(ProductType inventoryType)
        {
            try
            {
                if (inventoryType == ProductType.PreBuy)
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
        private static decimal GetOrderShippingTotal(ProductType inventoryType)
        {
            try
            {
                if (inventoryType == ProductType.GeneralInventory)
                {
                    EstimateDeliveryPriceRequestDto estimationdto = GetEstimationDTO(Cart);
                    var estimation = CallEstimationService(estimationdto);
                    return ValidationHelper.GetDecimal(estimation?.Payload?.Cost, default(decimal));
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
    }
}