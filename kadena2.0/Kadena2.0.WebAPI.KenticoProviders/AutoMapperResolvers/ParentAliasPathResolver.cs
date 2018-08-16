using AutoMapper;
using CMS.DocumentEngine;
using Kadena.Models.Product;

namespace Kadena.WebAPI.KenticoProviders.AutoMapperResolvers
{
    public class ParentAliasPathResolver : IValueResolver<TreeNode, ProductLink, string>
    {
        public string Resolve(TreeNode node, ProductLink product, string member, ResolutionContext context)
        {
            return node.Parent?.NodeAliasPath;
        }
    }
}
