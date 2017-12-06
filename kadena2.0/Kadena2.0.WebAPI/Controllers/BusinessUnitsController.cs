using CMS.DataEngine;
using CMS.Helpers;
using CMS.SiteProvider;
using Kadena.WebAPI.Infrastructure;
using Newtonsoft.Json;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    public class BusinessUnitsController : ApiControllerBase
    {
        [HttpGet]
        [Route("api/businessunits")]
        public string GetAllActiveBusienssUnits()
        {
            QueryDataParameters parameters = new QueryDataParameters();
            GeneralConnection cn = ConnectionHelper.GetConnection();
            parameters.Add("@Status", 1);
            parameters.Add("@SiteID", SiteContext.CurrentSiteID);
            var query = "select BusinessUnitName,BusinessUnitNumber,ItemID from KDA_BusinessUnit where Status = @Status and SiteID=@SiteID";
            QueryParameters qp = new QueryParameters(query, parameters, QueryTypeEnum.SQLQuery);
            var userBUData = cn.ExecuteQuery(qp);
            if (!DataHelper.DataSourceIsEmpty(userBUData))
            {
                return JsonConvert.SerializeObject(userBUData.Tables[0], Formatting.Indented);
            }
            return string.Empty;
        }

        [HttpGet]
        [Route("api/businessunits/{userID}")]
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
                return JsonConvert.SerializeObject(userBUData.Tables[0], Formatting.Indented);
            }
            return string.Empty;
        }
    }
}
