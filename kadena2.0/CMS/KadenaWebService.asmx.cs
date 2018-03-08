namespace CMSApp
{
    using CMS.DataEngine;
    using CMS.EmailEngine;
    using CMS.Helpers;
    using CMS.Localization;
    using CMS.MacroEngine;
    using CMS.Membership;
    using CMS.OnlineForms;
    using CMS.SiteProvider;
    using Kadena.Dto.General;
    using System;
    using System.Web.Script.Services;
    using System.Web.Services;
    using CMS.CustomTables;
    using CMS.EventLog;
    using Kadena.Helpers;

    [WebService]
    [ScriptService]
    public class KadenaWebService : System.Web.Services.WebService
    {
        #region Static members

        public const string _ForgottenPasswordFormCodeName = "KDA_ForgottenPasswordForm";
        public const string _RequestAccessFormCodeName = "KDA_RequestAccessForm";
        public const string _NewKitRequestFormCodeName = "KDA_NewKitRequest";

        #endregion

        #region Public methods

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public void SignOut()
        {
            AuthenticationHelper.SignOut();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public GeneralResultDTO InitialPasswordSetting(string password, string confirmPassword, Guid userGUID)
        {
            #region Validation

            if (string.IsNullOrWhiteSpace(password))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.InitialPasswordSetting.PasswordIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.InitialPasswordSetting.PasswordIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (password.Contains(" "))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.InitialPasswordSetting.PasswordContainsWhitespace", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (confirmPassword.Contains(" "))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.InitialPasswordSetting.PasswordContainsWhitespace", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (password != confirmPassword)
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.InitialPasswordSetting.PasswordsAreNotTheSame", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (!SecurityHelper.CheckPasswordPolicy(password, SiteContext.CurrentSiteName))
            {
                return new GeneralResultDTO { success = false, errorMessage = AuthenticationHelper.GetPolicyViolationMessage(SiteContext.CurrentSiteName) };
            }

            #endregion

            return InitialPasswordSettingInternal(password, confirmPassword, userGUID);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public GeneralResultDTO ContactPersonDetailsChange(Guid userGUID, string firstName, string lastName, string mobile, string phoneNumber)
        {
            return ContactPersonDetailsChangeInternal(userGUID, firstName, lastName, mobile, phoneNumber);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public GeneralResultDTO ChangePassword(Guid userGUID, string oldPassword, string newPassword, string confirmPassword)
        {
            #region Validation

            if (string.IsNullOrEmpty(oldPassword))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.OldPasswordIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (oldPassword.Contains(" "))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.OldPasswordContainsWhiteSpaces", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.NewPasswordIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (newPassword.Contains(" "))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.NewPasswordContainsWhiteSpaces", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (string.IsNullOrEmpty(confirmPassword))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.ConfirmPasswordIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (confirmPassword.Contains(" "))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.ConfirmPasswordContainsWhiteSpaces", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (newPassword != confirmPassword)
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.PasswordsDontMatch", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (!SecurityHelper.CheckPasswordPolicy(newPassword, SiteContext.CurrentSiteName))
            {
                var errorMessage = string.Empty;
                var customMessage = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSPolicyViolationMessage");
                if (!string.IsNullOrEmpty(customMessage))
                {
                    errorMessage = ResHelper.LocalizeString(customMessage, LocalizationContext.CurrentCulture.CultureCode);
                }
                return new GeneralResultDTO { success = false, errorMessage = errorMessage };
            }

            #endregion

            return ChangePasswordInternal(userGUID, oldPassword, newPassword);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public GeneralResultDTO SubmitForgottenPasswordForm(string email)
        {
            #region Validation

            if (string.IsNullOrWhiteSpace(email))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.ForgottenPassword.EmailIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (!MailValidator.IsValid(email))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.ForgottenPassword.EmailIsNotValid", LocalizationContext.CurrentCulture.CultureCode) };
            }

            #endregion

            return SubmitForgottenPasswordFormInternal(email);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public GeneralResultDTO SubmitRequestAccessForm(string email)
        {
            #region Validation

            if (string.IsNullOrWhiteSpace(email))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.RequestAccess.EmailIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (!MailValidator.IsValid(email))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.RequestAccess.EmailIsNotValid", LocalizationContext.CurrentCulture.CultureCode) };
            }

            #endregion

            return SubmitRequestAccessFormInternal(email);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public GeneralResultDTO SubmitNewKitRequest(string name, string description, int[] productIDs, string[] productNames)
        {
            #region Validation

            if (string.IsNullOrWhiteSpace(name))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewKitRequest.NameIsMandatory", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewKitRequest.DescriptionIsMandatory", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (productIDs == null || productIDs.Length < 2)
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewKitRequest.NoProductSelected", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (productIDs.Length > 6)
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewKitRequest.ToManyProductsSelected", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (productNames == null || productNames.Length < 2)
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewKitRequest.NoProductSelected", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (productNames.Length > 6)
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewKitRequest.ToManyProductsSelected", LocalizationContext.CurrentCulture.CultureCode) };
            }

            #endregion

            return SubmitNewKitRequestInternal(name, description, productIDs, productNames);
        }

        #endregion

        #region Private methods
        private GeneralResultDTO InitialPasswordSettingInternal(string password, string confirmPassword, Guid userGUID)
        {
            var ui = UserInfoProvider.GetUserInfoByGUID(userGUID);
            if (ui == null)
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.InitialPasswordSetting.PasswordCantBeSetUp", LocalizationContext.CurrentCulture.CultureCode) };
            }
            UserInfoProvider.SetPassword(ui, password);
            ui.Enabled = true;
            ui.Update();

            return new GeneralResultDTO { success = true };
        }

        private GeneralResultDTO ContactPersonDetailsChangeInternal(Guid userGUID, string firstName, string lastName, string mobile, string phoneNumber)
        {
            var ui = UserInfoProvider.GetUserInfoByGUID(userGUID);
            if (ui == null)
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.ContanctDetailsChange.UserNotFound", LocalizationContext.CurrentCulture.CultureCode) };
            }
            ui.FirstName = firstName;
            ui.LastName = lastName;
            ui.FullName = firstName + " " + lastName;
            ui.SetValue("UserMobile", mobile);
            ui.SetValue("UserPhone", phoneNumber);

            ui.Update();

            return new GeneralResultDTO { success = true };
        }

        private GeneralResultDTO ChangePasswordInternal(Guid userGUID, string oldPassword, string newPassword)
        {
            var ui = UserInfoProvider.GetUserInfoByGUID(userGUID);
            if (ui == null)
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.UserNotFound", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (!UserInfoProvider.ValidateUserPassword(ui, oldPassword))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.OldPasswordIsNotValid", LocalizationContext.CurrentCulture.CultureCode) };
            }
            UserInfoProvider.SetPassword(ui, newPassword);
            ui.Update();

            return new GeneralResultDTO { success = true };
        }

        private GeneralResultDTO SubmitForgottenPasswordFormInternal(string email)
        {
            var forgottenPasswordForm = BizFormInfoProvider.GetBizFormInfo(_ForgottenPasswordFormCodeName, SiteContext.CurrentSiteID);
            if (forgottenPasswordForm != null)
            {
                var forgottenPasswordFormClass = DataClassInfoProvider.GetDataClassInfo(forgottenPasswordForm.FormClassID);
                string forgottenPasswordFormClassName = forgottenPasswordFormClass.ClassName;

                BizFormItem newFormItem = BizFormItem.New(forgottenPasswordFormClassName);

                newFormItem.SetValue("Email", email);
                newFormItem.SetValue("Site", SiteContext.CurrentSite.DisplayName);
                newFormItem.SetValue("FormInserted", DateTime.Now);
                newFormItem.SetValue("FormUpdated", DateTime.Now);

                newFormItem.Insert();

                SendFormEmail(newFormItem);

                return new GeneralResultDTO { success = true };
            }
            else
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.ForgottenPassword.ForgottenPasswordRepositoryNotFound", LocalizationContext.CurrentCulture.CultureCode) };
            }
        }

        private GeneralResultDTO SubmitRequestAccessFormInternal(string email)
        {
            var requestAccessForm = BizFormInfoProvider.GetBizFormInfo(_RequestAccessFormCodeName, SiteContext.CurrentSiteID);
            if (requestAccessForm != null)
            {
                var requestAccessFormClass = DataClassInfoProvider.GetDataClassInfo(requestAccessForm.FormClassID);
                string requestAccessFormClassName = requestAccessFormClass.ClassName;

                BizFormItem newFormItem = BizFormItem.New(requestAccessFormClassName);

                newFormItem.SetValue("Email", email);
                newFormItem.SetValue("Site", SiteContext.CurrentSite.DisplayName);
                newFormItem.SetValue("FormInserted", DateTime.Now);
                newFormItem.SetValue("FormUpdated", DateTime.Now);

                newFormItem.Insert();

                SendFormEmail(newFormItem);

                return new GeneralResultDTO { success = true };
            }
            else
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.ForgottenPassword.ForgottenPasswordRepositoryNotFound", LocalizationContext.CurrentCulture.CultureCode) };
            }
        }

        private GeneralResultDTO SubmitNewKitRequestInternal(string name, string description, int[] productIDs, string[] productNames)
        {
            var newKitRequestForm = BizFormInfoProvider.GetBizFormInfo(_NewKitRequestFormCodeName, SiteContext.CurrentSiteID);
            if (newKitRequestForm != null)
            {
                var newKitRequestFormClass = DataClassInfoProvider.GetDataClassInfo(newKitRequestForm.FormClassID);
                string newKitRequestFormClassName = newKitRequestFormClass.ClassName;

                BizFormItem newFormItem = BizFormItem.New(newKitRequestFormClassName);

                newFormItem.SetValue("Name", name);
                newFormItem.SetValue("Description", description);
                newFormItem.SetValue("ProductNames", string.Join("|", productNames));
                newFormItem.SetValue("ProductNodeIDs", string.Join("|", productIDs));

                newFormItem.SetValue("Site", SiteContext.CurrentSite.DisplayName);
                newFormItem.SetValue("User", MembershipContext.AuthenticatedUser.UserName);

                newFormItem.SetValue("FormInserted", DateTime.Now);
                newFormItem.SetValue("FormUpdated", DateTime.Now);

                newFormItem.Insert();

                SendFormEmail(newFormItem);

                return new GeneralResultDTO { success = true };
            }
            else
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.NewKitRequest.ContactFormRepositoryNotFound", LocalizationContext.CurrentCulture.CultureCode) };
            }
        }

        private void SendFormEmail(BizFormItem item)
        {
            if (item.BizFormInfo != null)
            {
                MacroResolver resolver = MacroContext.CurrentResolver.CreateChild();
                resolver.SetAnonymousSourceData(item);
                resolver.Settings.EncodeResolvedValues = true;
                resolver.Culture = CultureHelper.GetPreferredCulture();

                string body = DataHelper.GetNotEmpty(item.BizFormInfo.FormEmailTemplate, string.Empty);
                body = resolver.ResolveMacros(body);

                EmailMessage message = new EmailMessage();
                message.From = item.BizFormInfo.FormSendFromEmail;
                message.Recipients = resolver.ResolveMacros(item.BizFormInfo.FormSendToEmail);
                message.Subject = resolver.ResolveMacros(item.BizFormInfo.FormEmailSubject);
                message.Body = URLHelper.MakeLinksAbsolute(body);
                message.EmailFormat = EmailFormatEnum.Html;

                EmailSender.SendEmail(message);
            }
        }

        //Method for deleteting the product category
        [WebMethod(EnableSession = true)]
        public bool DeleteCategory(int CategoryID)
        {
            bool status = false;
            if (CategoryID > 0)
            {
                CMS.DocumentEngine.TreeProvider tree = new CMS.DocumentEngine.TreeProvider(CMS.Membership.MembershipContext.AuthenticatedUser);

                // Gets the culture version of the page that will be deleted
                CMS.DocumentEngine.TreeNode page = tree.SelectNodes("KDA.ProductCategory").Where("ProductCategoryID", CMS.DataEngine.QueryOperator.Equals, CategoryID).OnCurrentSite();
                if (page != null)
                {
                    // Deletes the page and moves it to the recycle bin (only the specified culture version)
                    status = page.Delete();

                    //  Creates search tasks that remove the deleted page from the content of related search indexes
                    if (CMS.Search.SearchIndexInfoProvider.SearchEnabled)
                    {
                        CMS.Search.SearchTaskInfoProvider.CreateTask(CMS.Search.SearchTaskTypeEnum.Delete, CMS.DocumentEngine.TreeNode.OBJECT_TYPE, CMS.DataEngine.SearchFieldsConstants.ID, page.GetSearchID(), page.DocumentID);
                    }
                }
            }
            return status;

        }

        //Method for deleteting the campaign
        [WebMethod(EnableSession = true)]
        public bool DeleteCampaign(int CampaignID)
        {
            bool status = false;
            if (CampaignID > 0)
            {
                CMS.DocumentEngine.TreeProvider tree = new CMS.DocumentEngine.TreeProvider(CMS.Membership.MembershipContext.AuthenticatedUser);

                // Gets the culture version of the page that will be deleted
                CMS.DocumentEngine.TreeNode page = tree.SelectNodes("KDA.Campaign").Where("CampaignID", CMS.DataEngine.QueryOperator.Equals, CampaignID).OnCurrentSite();
                if (page != null)
                {
                    // Deletes the page and moves it to the recycle bin (only the specified culture version)
                    status = page.Delete();

                    //  Creates search tasks that remove the deleted page from the content of related search indexes
                    if (CMS.Search.SearchIndexInfoProvider.SearchEnabled)
                    {
                        CMS.Search.SearchTaskInfoProvider.CreateTask(CMS.Search.SearchTaskTypeEnum.Delete, CMS.DocumentEngine.TreeNode.OBJECT_TYPE, CMS.DataEngine.SearchFieldsConstants.ID, page.GetSearchID(), page.DocumentID);
                    }
                }
            }
            return status;

        }
        [WebMethod(EnableSession = true)]
        public void EditPOSNumber(int posId, bool status)
        {
            string customTableClassName = "KDA.POSNumber";
            try
            {
                // Gets the custom table
                DataClassInfo brandTable = DataClassInfoProvider.GetDataClassInfo(customTableClassName);
                if (brandTable != null)
                {
                    // Gets all data records from the POS table whose 'ItemId' field value equal to PosId
                    CustomTableItem customTableData = CustomTableItemProvider.GetItem(posId, customTableClassName);
                    if (customTableData != null)
                    {
                        customTableData.SetValue("Enable", ValidationHelper.GetBoolean(status, false));
                        customTableData.Update();

                    }
                }
            }
            catch (Exception ex)
            {
                EventLogProvider.LogInformation("CMSWebParts_Kadena_POS_POSForm", "GetBrands", ex.Message);
            }
        }
        #endregion


        #region "TWE"
        [WebMethod(EnableSession = true)]
        public string GetUserBusinessUnitData(int userID)
        {
            QueryDataParameters parameters = new QueryDataParameters();
            GeneralConnection cn = ConnectionHelper.GetConnection();
            parameters.Add("@UserID", userID);
            parameters.Add("@SiteID", SiteContext.CurrentSiteID);
            var query = "select BusinessUnitName,BusinessUnitNumber,b.ItemID from KDA_UserBusinessUnits ub inner join KDA_BusinessUnit b on ub.BusinessUnitID = b.ItemID  where ub.UserID = @UserID and b.SiteID=@SiteID";
            QueryParameters qp = new QueryParameters(query, parameters, QueryTypeEnum.SQLQuery);
            var userBUData = cn.ExecuteQuery(qp);
            if (!DataHelper.DataSourceIsEmpty(userBUData))
            {
                var buData = userBUData.Tables[0];
                var JSONString = ConvertDataTbaleToJson(buData);
                return JSONString.ToString();
            }
            return string.Empty;
        }


        public System.Text.StringBuilder ConvertDataTbaleToJson(System.Data.DataTable table)
        {
            var JSONString = new System.Text.StringBuilder();
            if (!DataHelper.DataSourceIsEmpty(table))
            {
                if (table.Rows.Count > 0)
                {
                    JSONString.Append("[");
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        JSONString.Append("{");
                        for (int j = 0; j < table.Columns.Count; j++)
                        {
                            if (j < table.Columns.Count - 1)
                            {
                                JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                            }
                            else if (j == table.Columns.Count - 1)
                            {
                                JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                            }
                        }
                        if (i == table.Rows.Count - 1)
                        {
                            JSONString.Append("}");
                        }
                        else
                        {
                            JSONString.Append("},");
                        }
                    }
                    JSONString.Append("]");
                }
            }
            return JSONString;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetAllActiveBusienssUnits()
        {
            QueryDataParameters parameters = new QueryDataParameters();
            GeneralConnection cn = ConnectionHelper.GetConnection();
            parameters.Add("@Status", 1);
            parameters.Add("@SiteID", SiteContext.CurrentSiteID);
            var query = "select BusinessUnitName,BusinessUnitNumber,ItemID from KDA_BusinessUnit where  Status = @Status and SiteID=@SiteID";
            QueryParameters qp = new QueryParameters(query, parameters, QueryTypeEnum.SQLQuery);
            var userBUData = cn.ExecuteQuery(qp);
            if (!DataHelper.DataSourceIsEmpty(userBUData))
            {
                var buData = userBUData.Tables[0];
                var JSONString = ConvertDataTbaleToJson(buData);
                return JSONString.ToString();
            }
            return string.Empty;
        }
        #endregion


    }
}
