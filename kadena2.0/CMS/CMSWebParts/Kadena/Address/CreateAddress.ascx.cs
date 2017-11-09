using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.PortalEngine.Web.UI;
using CMS.Helpers;
using CMS.Ecommerce;
using CMS.CustomTables.Types.KDA;
using CMS.Globalization;
using System.Linq;
using CMS.Membership;
using CMS.CustomTables;

public partial class CMSWebParts_Kadena_Address_CreateAddress : CMSAbstractWebPart
{
    #region "Properties"
    public string AddressListPath
    {
        get
        {
            return ValidationHelper.GetString("AddressListPath", string.Empty);
        }
        set
        {
            SetValue("AddressListPath", value);
        }
    }


    #endregion


    #region "Methods"

    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (this.StopProcessing)
        {
            // Do not process
        }
        else
        {
            if (AuthenticationHelper.IsAuthenticated())
            {
                BindResourceStrings();
                int itemID = Request.QueryString["itemID"] != null ? ValidationHelper.GetInteger(Request.QueryString["itemID"], default(int)) : default(int);
                if (itemID != default(int))
                {
                    headerAddress.InnerText = "Edit Address";
                    BindAddressData(itemID);
                    // UpdateAddressData(itemID);
                }
                else
                {

                }
            }


        }
    }

    private void BindResourceStrings()
    {
        lblName.InnerText = ResHelper.GetString("Kadena.Address.Name");
        lblAddressType.InnerText = ResHelper.GetString("Kadena.Address.AddressType");
        lblCompany.InnerText = ResHelper.GetString("Kadena.Address.Company");
        lblAddressLine1.InnerText = ResHelper.GetString("Kadena.Address.AddressLine1");
        lblAddressLine2.InnerText = ResHelper.GetString("Kadena.Address.AddressLine2");
        lblCity.InnerText = ResHelper.GetString("Kadena.Address.City");
        lblCountry.InnerText = ResHelper.GetString("Kadena.Address.Country");
        //ResHelper.GetString("Kadena.Address.State");
        lblZipcode.InnerText = ResHelper.GetString("Kadena.Address.Zipcode");
        lblTelephone.InnerText = ResHelper.GetString("Kadena.Address.Telephone");
        lblEmail.InnerText = ResHelper.GetString("Kadena.Address.Email");
    }

    private void UpdateAddressData(int itemID)
    {
        var addressObj = BindAddressObject(itemID);
        AddressInfoProvider.SetAddressInfo(addressObj);

        var shippingObj = BindShippingAddressObject(addressObj);
        shippingObj.Update();

    }

    private void BindAddressData(int itemID)
    {
        var addressData = AddressInfoProvider.GetAddresses().WhereEquals("AddressID", itemID).TopN(1).FirstOrDefault();
        if (!DataHelper.DataSourceIsEmpty(addressData))
        {
            txtAddressLine1.Text = addressData.AddressLine1;
            txtAddressLine2.Text = addressData.AddressLine2;
            txtCity.Text = addressData.AddressCity;
            txtZipcode.Text = addressData.AddressZip;
            txtName.Text = addressData.AddressPersonalName;
            txtTelephone.Text = addressData.AddressPhone;
            ddlCountry.Value = GetCountryName(addressData.AddressCountryID) + ";" + GetStateName(addressData.AddressStateID);

            ddlAddressType.Value = addressData.GetValue("AddressType", string.Empty);

            var shippingData = CustomTableItemProvider.GetItems<ShippingAddressItem>().WhereEquals("COM_AddressID", addressData.AddressID).TopN(1).FirstOrDefault();
            if (!DataHelper.DataSourceIsEmpty(shippingData))
            {
                txtEmail.Text = shippingData.GetStringValue("Email", string.Empty);
                txtComapnyName.Text = shippingData.GetStringValue("CompanyName", string.Empty);
            }


        }
    }

    private void CreateNewAddress(int customerID)
    {

        var objAddress = BindAddressObject(customerID);
        AddressInfoProvider.SetAddressInfo(objAddress);

        var objShipping = BindShippingAddressObject(objAddress);
        objShipping.Insert();


    }

    private ShippingAddressItem BindShippingAddressObject(AddressInfo objAddress)
    {
        ShippingAddressItem item = new ShippingAddressItem();
        item.AddressTypeID = ValidationHelper.GetInteger(ddlAddressType.Value, default(int));
        item.COM_AddressID = objAddress.AddressID;
        item.UserID = CurrentUser.UserID;
        item.Email = ValidationHelper.GetString(txtEmail.Text, string.Empty);
        item.CompanyName = ValidationHelper.GetString(txtComapnyName.Text, string.Empty);
        int itemID = Request.QueryString["itemID"] != null ? ValidationHelper.GetInteger(Request.QueryString["itemID"], default(int)) : default(int);
        if (itemID != default(int))
        {
            var shippingData = CustomTableItemProvider.GetItems<ShippingAddressItem>().WhereEquals("COM_AddressID", objAddress.AddressID).TopN(1).FirstOrDefault();
            item.ItemID = shippingData.ItemID;
        }
        return item;
    }

    private AddressInfo BindAddressObject(int customerID)
    {
        AddressInfo objAddress = new AddressInfo();
        objAddress.AddressLine1 = ValidationHelper.GetString(txtAddressLine1.Text, string.Empty);
        objAddress.AddressLine2 = ValidationHelper.GetString(txtAddressLine2.Text, string.Empty);
        objAddress.AddressCity = ValidationHelper.GetString(txtCity.Text, string.Empty);
        objAddress.AddressZip = ValidationHelper.GetString(txtZipcode.Text, string.Empty);

        objAddress.AddressName = string.Format("{0},{1},{2}", objAddress.AddressLine1, objAddress.AddressLine2, objAddress.AddressCity);

        objAddress.AddressPhone = ValidationHelper.GetString(txtTelephone.Text, string.Empty);
        objAddress.AddressCustomerID = customerID;
        objAddress.AddressPersonalName = ValidationHelper.GetString(txtName.Text, string.Empty);

        var country = ddlCountry.Value != null ? ddlCountry.Value.ToString() : string.Empty;
        objAddress.AddressCountryID = !string.IsNullOrEmpty(country) ? GetCountryID(country.Split(';').First()) : default(int);
        objAddress.AddressStateID = !string.IsNullOrEmpty(country) ? GetStateID(country.Split(';').Last()) : default(int);
        objAddress.SetValue("AddressTypeID", ddlAddressType.Value);

        int itemID = Request.QueryString["itemID"] != null ? ValidationHelper.GetInteger(Request.QueryString["itemID"], default(int)) : default(int);
        if (itemID != default(int))
        {
            objAddress.AddressID = itemID;
        }
        return objAddress;
    }


    /// <summary>
    /// Reloads the control data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();

        SetupControl();
    }

    #endregion

    #region methods

    private int GetCountryID(string countryName)
    {
        var countryData = CountryInfoProvider.GetCountries().WhereEquals("CountryDisplayName", countryName).TopN(1).FirstOrDefault();
        if (!DataHelper.DataSourceIsEmpty(countryData)) return countryData.CountryID;
        return default(int);
    }

    private int GetStateID(string stateName)
    {
        var stateData = StateInfoProvider.GetStates().WhereEquals("StateDisplayName", stateName).TopN(1).FirstOrDefault();
        if (!DataHelper.DataSourceIsEmpty(stateData)) return stateData.StateID;
        return default(int);
    }

    private string GetCountryName(int countryID)
    {
        var countryData = CountryInfoProvider.GetCountries().WhereEquals("CountryID", countryID).Column("CountryDisplayName").TopN(1).FirstOrDefault();
        if (!DataHelper.DataSourceIsEmpty(countryData)) return countryData.CountryDisplayName;
        return string.Empty;
    }

    private string GetStateName(int stateID)
    {
        var stateData = StateInfoProvider.GetStates().WhereEquals("StateID", stateID).Column("StateDisplayName").TopN(1).FirstOrDefault();
        if (!DataHelper.DataSourceIsEmpty(stateData)) return stateData.StateDisplayName;
        return string.Empty;
    }



    private int CreateCustomer()
    {
        CustomerInfo objCustomer = new CustomerInfo();
        objCustomer.CustomerUserID = CurrentUser.UserID;
        objCustomer.CustomerEmail = CurrentUser.Email;
        objCustomer.CustomerFirstName = CurrentUser.FirstName;
        objCustomer.CustomerLastName = CurrentUser.FirstName;
        objCustomer.CustomerSiteID = CurrentSite.SiteID;
        CustomerInfoProvider.SetCustomerInfo(objCustomer);
        return objCustomer.CustomerID;
    }

    private int IsUserCustomer(int userID)
    {
        CustomerInfo customer = CustomerInfoProvider.GetCustomers()
                                                .WhereEquals("CustomerUserID", userID).FirstOrDefault();
        if (!DataHelper.DataSourceIsEmpty(customer)) return customer.CustomerID;
        else return default(int);
    }

    #endregion

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int itemID = Request.QueryString["itemID"] != null ? ValidationHelper.GetInteger(Request.QueryString["itemID"], default(int)) : default(int);
        var customerID = IsUserCustomer(CurrentUser.UserID);
        if (itemID != default(int))
        {
            UpdateAddressData(itemID);
            Response.Redirect(AddressListPath);
        }
        else
        {
            if (customerID != default(int))
            {
                CreateNewAddress(customerID);
                Response.Redirect(AddressListPath);
            }
            else
            {
                customerID = CreateCustomer();
                CreateNewAddress(customerID);
                Response.Redirect(AddressListPath);
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(AddressListPath);
    }
}



