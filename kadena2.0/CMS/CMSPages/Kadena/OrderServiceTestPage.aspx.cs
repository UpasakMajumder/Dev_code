using CMS.DataEngine;
using CMS.SiteProvider;
using Kadena.Models.Common;
using System;
using System.IO;
using System.Net;

namespace CMSApp.CMSPages.Kadena
{
  public partial class OrderServiceTestPage : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {       
        lblEndpointUrl.Text = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_OrderServiceEndpoint");

        txtValue.InnerText = @"{
               ""OrderID"": ""519439c0-42d8-4bcf-b3f3-4fa5e724c901"",
               ""BillingAddress"": {
                              ""KenticoAddressID"": 324,
                              ""AddressLine1"": ""Address line 1"",
                              ""AddressLine2"": ""Address line 2"",
                              ""City"": ""Billing City"",
                              ""Zip"": ""34512"",
                              ""Phone"": ""723 3525 3435"",
                              ""KenticoCountryID"": 12,
                              ""Country"": ""Billing Country"",
                              ""KenticoStateID"": 34,
                              ""State"": ""WEST - 2"",
                              ""AddressPersonalName"": ""Person for billing""
               },
               ""ShippingAddress"": {
                              ""KenticoAddressID"": 124,
                              ""AddressLine1"": ""Address line 1 shipping"",
                              ""AddressLine2"": ""Address line 2 shipping"",
                              ""City"": ""Shipping City"",
                              ""Zip"": ""34512"",
                              ""Phone"": ""723 3525 3439"",
                              ""KenticoCountryID"": 129,
                              ""Country"": ""Billing Country"",
                              ""KenticoStateID"": 364,
                              ""State"": ""WEST - 3"",
                              ""AddressPersonalName"": ""Person for billing""
               },
               ""ShippingOption"": {
                              ""KenticoShippingOptionID"": 1,
                              ""ShippingOptionName"": ""shipping option""
               },
               ""TotalShipping"": 1200.24,
               ""TotalPrice"": 890890.89,
               ""TotalTax"": 789.9,
               ""OrderDate"": ""2017-03-25T19:23:31.8786281+01:00"",
               ""OrderStatus"": {
                              ""KenticoOrderStatusID"": 4,
                              ""OrderStatusName"": ""PENDING""
               },
               ""OrderCurrency"": {
                              ""KenticoCurrencyID"": 1,
                              ""CurrencyName"": ""USD""
               },
               ""Customer"": {
                              ""KenticoCustomerID"": 12,
                              ""FirstName"": ""Aron"",
                              ""LastName"": ""Cruden"",
                              ""Email"": ""customer@hotmail.com"",
                              ""Phone"": ""+64 23141 4536"",
                              ""KenticoUserID"": 456767
               },
               ""KenticoOrderCreatedByUserID"": 13,
               ""OrderNote"": ""Test Order Note"",
               ""Site"": {
                              ""KenticoSiteID"": 57,
                              ""KenticoSiteName"": ""Kentico Site Name""
               },
               ""PaymentOption"": {
                              ""KenticoPaymentOptionID"": 9023131,
                              ""PaymentOptionName"": ""Kentico payment option""
               },
               ""OrderTracking"": {
                              ""OrderTrackingNumber"": ""75373527572534721""
               },
               ""PaymentResult"": {
                              ""IsPaid"": true,
                              ""Text"": ""sample text for payment result""
               },
               ""LastModified"": ""2017-03-25T19:23:31.8796299+01:00"",
               ""Items"": [{
                              ""SKU"": {
                                             ""KenticoSKUID"": 5,
                                             ""SKUNumber"": ""number 1"",
                                             ""Name"": ""SKU name 1""
                              },
                              ""Type"": 2,
                              ""UnitPrice"": 23.9,
                              ""UnitCount"": 2,
                              ""TotalPrice"": 567.9,
                              ""TotalTax"": 54.9,
                              ""MailingList"": {
                                             ""MailingListID"": ""c93eec27-7a93-4e43-bf25-7edca07f7cc2""
                              },
                              ""DesignFilePath"": ""file path 1""
               },
               {
                              ""SKU"": {
                                             ""KenticoSKUID"": 7,
                                             ""SKUNumber"": ""number 2"",
                                             ""Name"": ""SKU name 2""
                              },
                              ""Type"": 2,
                              ""UnitPrice"": 2.9,
                              ""UnitCount"": 20,
                              ""TotalPrice"": 57.9,
                              ""TotalTax"": 5.9,
                              ""MailingList"": {
                                             ""MailingListID"": ""129dcf25-bf2f-40b2-a2f9-9504b5794058""
                              },
                              ""DesignFilePath"": ""file path 2""
               }]
}";
      }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
      var request = (HttpWebRequest)WebRequest.Create(SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_OrderServiceEndpoint"));
      request.ContentType = ContentTypes.Json;
      request.Method = "POST";

      using (var streamWriter = new StreamWriter(request.GetRequestStream()))
      {
        streamWriter.Write(txtValue.InnerText);
      }

      var response = (HttpWebResponse)request.GetResponse();
      string result = String.Empty;

      using (var streamReader = new StreamReader(response.GetResponseStream()))
      {
        result = streamReader.ReadToEnd();
      }
      ltlResultMessage.Text = response.StatusCode + " " + result;
    }
  }
}