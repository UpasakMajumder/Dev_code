using System;
using System.Linq;
using System.Web.UI.WebControls;
using CMS.Ecommerce.Web.UI;
using CMS.PortalEngine;
using CMS.MacroEngine;
using CMS.Ecommerce;

public partial class CMSWebParts_Ecommerce_Checkout_Forms_CustomerShipToAddress : CMSCheckoutWebPart
{
    private IAddress CurrentCartAddress
    {
        get
        {
            
            return ShoppingCart.ShoppingCartShippingAddress;
        }
        set
        {
            ShoppingCart.ShoppingCartShippingAddress = value;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        this.InitializeAddress();
    }

    private void InitializeAddress()
    {
        if (Customer == null)
        {
            return;
        }

        var addresses = AddressInfoProvider.GetAddresses(Customer.CustomerID).ToArray();
        
        foreach (AddressInfo addr in addresses)
        {
            if (!this.IsPostBack)
            {
                var li = new ListItem(addr.AddressID.ToString());
                li.Value = addr.AddressID.ToString();
                li.Selected = CurrentCartAddress.AddressID == addr.AddressID;
                this.hiddenAddressesList.Items.Add(li);
            }
                
            var transformation = TransformationInfoProvider.GetTransformation("kda.checkoutpage.ShippingAddress");
                
            var resolver = MacroResolver.GetInstance();
            resolver.SetNamedSourceData("ShippingAddress", addr);
            resolver.SetNamedSourceData("StateCode", addr.GetStateCode());
            resolver.SetNamedSourceData("Checked", CurrentCartAddress.AddressID == addr.AddressID?"checked":"");
            htmlContent.Text += resolver.ResolveMacros(transformation.TransformationCode);
        }
    }

    protected void UpdateCartAddress(object sender, EventArgs e)
    {
        var list = sender as RadioButtonList;
        int addressId = 0;

        if (list == null || list.SelectedItem == null || !int.TryParse(list.SelectedItem.Value, out addressId))
        {
            return;
        }

        var address = AddressInfoProvider.GetAddressInfo(addressId);
        CurrentCartAddress = address;
    }
}