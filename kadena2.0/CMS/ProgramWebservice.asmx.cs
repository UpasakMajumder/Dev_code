using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KDA;
using CMS.Membership;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace CMSApp
{
    /// <summary>
    /// Summary description for ProgramWebservice
    /// </summary>
    [WebService]
    [ScriptService]
    public class ProgramWebservice : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        /// <summary>
        /// Delete Programs.
        /// </summary>
        /// <param name="programID"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod]
        public bool DeleteProgram(int programID)
        {
            if (programID != 0)
            {
                Program program = ProgramProvider.GetPrograms().WhereEquals("ProgramID", programID).TopN(1).FirstOrDefault();
                program.Delete();
                return true;
            }
            return false;
        }
    }
}
