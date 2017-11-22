using CMS;
using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.DocumentEngine.Types.KDA;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.SiteProvider;
using System.Linq;

[assembly: RegisterExtension(typeof(KadenaMacroMethods), typeof(string))]
public class KadenaMacroMethods : MacroMethodContainer
{
    [MacroMethod(typeof(string), "Returns Division name based on Division ID", 1)]
    [MacroMethodParam(0, "DivisionID", typeof(int), "DivisionID")]
    public static object GetDivisionName(EvaluationContext context, params object[] parameters)
    {
        int DivisionID = ValidationHelper.GetInteger(parameters[0], 0);
        string DivisionName = string.Empty;

        DivisionItem Division = CustomTableItemProvider.GetItem<DivisionItem>(DivisionID);
        if (Division != null)
            DivisionName = Division.DivisionName;

        return DivisionName;
    }

    [MacroMethod(typeof(string), "Returns Program name based on Program ID", 1)]
    [MacroMethodParam(0, "ProgramID", typeof(int), "ProgramID")]
    public static object GetProgramName(EvaluationContext context, params object[] parameters)
    {
        int ProgramID = ValidationHelper.GetInteger(parameters[0], 0);
        string ProgramName = string.Empty;
        Program program = ProgramProvider.GetPrograms().WhereEquals("NodeSiteID",SiteContext.CurrentSite.SiteID).WhereEquals("ProgramID", ProgramID).Columns("ProgramName").FirstObject;
        if (program != null)
            ProgramName = program.ProgramName;
        return ProgramName;
    }

    [MacroMethod(typeof(string), "Returns Category name based on Category ID", 1)]
    [MacroMethodParam(0, "CategoryID", typeof(int), "CategoryID")]
    public static object GetCategoryName(EvaluationContext context, params object[] parameters)
    {
        int CategoryID = ValidationHelper.GetInteger(parameters[0], 0);
        string CategoryName = string.Empty;
        ProductCategory category = ProductCategoryProvider.GetProductCategories().WhereEquals("NodeSiteID", SiteContext.CurrentSite.SiteID).WhereEquals("ProductCategoryID", CategoryID).Columns("ProductCategoryTitle").FirstObject;
        if (category != null)
            CategoryName = category.ProductCategoryTitle;

        return CategoryName;
    }

    [MacroMethod(typeof(string), "Returns Currently opened campaign name", 1)]
    
    public static object GetCampaignName(EvaluationContext context, params object[] parameters)
    {
        var campaign = CampaignProvider.GetCampaigns().Columns("Name").WhereEquals("OpenCampaign", 1).WhereEquals("NodeSiteID", SiteContext.CurrentSite.SiteID).FirstOrDefault();
        return ValidationHelper.GetString(campaign.GetValue("Name"), string.Empty);
    }
}
