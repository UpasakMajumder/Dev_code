using System;

using CMS.Helpers;
using CMS.PortalEngine.Web.UI;
using CMS.SiteProvider;
using CMS.Membership;
using CMS.DataEngine;

public partial class CMSWebParts_Kadena_Membership_KadenaChangePassword : CMSAbstractWebPart
{
    #region "Public properties"

    /// <summary>
    /// Gets or sets the value that indicates whether this webpart is displayed only when user is authenticated.
    /// </summary>
    public bool ShowOnlyWhenAuthenticated
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowOnlyWhenAuthenticated"), true);
        }
        set
        {
            SetValue("ShowOnlyWhenAuthenticated", value);
            Visible = (!value || AuthenticationHelper.IsAuthenticated());
        }
    }


    /// <summary>
    /// Gets or sets the maximal new password length.
    /// </summary>
    public int MaximalPasswordLength
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("MaximalPasswordLength"), 0);
        }
        set
        {
            SetValue("MaximalPasswordLength", value);
            passStrength.MaxLength = value;
            txtConfirmPassword.MaxLength = value;
        }
    }


    /// <summary>
    /// Gets or sets to show the lables
    /// </summary>
    public bool ShowLables
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("ShowLables"), false);
        }
        set
        {
            SetValue("ShowLables", value);
        }
    }



    #endregion


    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Reloads data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (!StopProcessing)
        {
            successMessage.Visible = false;
            Visible = (!ShowOnlyWhenAuthenticated || AuthenticationHelper.IsAuthenticated());
            passStrength.MaxLength = MaximalPasswordLength;
            txtConfirmPassword.MaxLength = MaximalPasswordLength;
            lblOldPassword.Visible = ShowLables;
            lblNewPassword.Visible = ShowLables;
            lblConfirmPassword.Visible = ShowLables;
            lblOldPassword.Text = ResHelper.GetString("Kadena.ChangePassword.OldPasswordText");
            lblNewPassword.Text = ResHelper.GetString("Kadena.ChangePassword.NewPasswordText");
            lblConfirmPassword.Text = ResHelper.GetString("Kadena.Common.ConfirmPasswordText");
            txtOldPassword.Attributes.Add("Placeholder", ResHelper.GetString("Kadena.ChangePassword.OldPasswordWatermarkText"));
            passStrength.PlaceholderText = ResHelper.GetString("Kadena.ChangePassword.NewPasswordWatermarkText");
            txtConfirmPassword.Attributes.Add("Placeholder", ResHelper.GetString("Kadena.ChangePassword.ConfirmPasswordWatermarkText"));
            rvOldPassword.ErrorMessage = ResHelper.GetString("Kadena.ChangePassword.OldPwdRequired");
            rvConfirmPassword.ErrorMessage = ResHelper.GetString("Kadena.ChangePassword.ConfirmPwdRequired");
            cvConfirmPassword.ErrorMessage = ResHelper.GetString("Kadena.ChangePassword.ConfirmPwdComapreError");
            lblNewPassword.AssociatedControlClientID = passStrength.InputClientID;
        }
    }


    /// <summary>
    /// OnClick handler (Set password).
    /// </summary>
    protected void btnOk_Click(object sender, EventArgs e)
    {
        var ui = MembershipContext.AuthenticatedUser;
        SiteInfo si = SiteContext.CurrentSite;
        if ((ui != null) && (si != null))
        {
            string userName = ui.UserName;
            if (passStrength.IsValid())
            {
                if (!UserInfoProvider.IsUserPasswordDifferent(ui, txtOldPassword.Text.Trim()))
                {
                    UserInfoProvider.SetPassword(userName, passStrength.Text.Trim());

                    if (SettingsKeyInfoProvider.GetBoolValue(SiteContext.CurrentSiteName + ".CMSSendPasswordResetConfirmation"))
                    {
                        AuthenticationHelper.SendPasswordResetConfirmation(ui, SiteContext.CurrentSiteName, "Change password web part", "Membership.PasswordResetConfirmation");
                    }
                }
                else
                {
                    successMessage.Visible = true;
                }
                
            }
        }
    }
}