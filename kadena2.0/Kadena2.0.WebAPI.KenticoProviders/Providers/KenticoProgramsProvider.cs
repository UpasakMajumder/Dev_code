using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Membership;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoProgramsProvider : IKenticoProgramsProvider
    {
        private readonly string PageTypeClassName = "KDA.Program";

        public void DeleteProgram(int programID)
        {
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            TreeNode program = tree.SelectNodes(PageTypeClassName)
                                    .Where("ProgramID", QueryOperator.Equals, programID)
                                    .OnCurrentSite();
            if (program != null)
            {
                program.Delete();
            }
        }
    }
}
