using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using CMS.Helpers;

namespace Kadena
{
    public partial class UserHierarchyInfoProvider
    {
        /// <summary>
        /// Gets all childs in the tree for specified user on specified site.
        /// </summary>
        /// <param name="siteId">id of the site to look on.</param>
        /// <param name="userId">id of the user to search childs for.</param>
        /// <returns>Set of distinct Ids of child users.</returns>
        public static IEnumerable<int> GetAllChilds(int siteId, int userId)
        {
            var result = new HashSet<int>();

            var queue = new Queue<int>();
            queue.Enqueue(userId);
            while (queue.Count > 0)
            {
                var parent = queue.Dequeue();
                DataSet ds = GetUserHierarchies(siteId).WhereEquals("ParentUserId", parent);
                var childs = DataHelper.GetIntegerValues(ds.Tables[0], "ChildUserId");
                foreach (var c in childs)
                {
                    if (result.Contains(c) || c == userId)
                        // Skip step for user that already exists in result set or the one that was passed to method as parameter.
                        continue;
                    else
                    {
                        result.Add(c);
                        queue.Enqueue(c);
                    }
                }
            }
            return result.AsEnumerable();
        }
    }
}