using CMS;
using CMS.CustomTables;
using CMS.CustomTables.Types.KDA;
using CMS.Helpers;
using CMS.MacroEngine;

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
}
