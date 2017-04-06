using Kadena;
using CMS.Tests;
using NUnit.Framework;
using System.Linq;

namespace Kadena.Tests
{
    [TestFixture]
    public class UserHierarchyTest : UnitTests
    {
        [SetUp]
        public void SetUp()
        {
            Fake<UserHierarchyInfo, UserHierarchyInfoProvider>()
                .WithData(
                new UserHierarchyInfo { ParentUserId = 1, ChildUserId = 2, SiteId = 1 },
                new UserHierarchyInfo { ParentUserId = 1, ChildUserId = 3, SiteId = 1 },
                new UserHierarchyInfo { ParentUserId = 2, ChildUserId = 4, SiteId = 1 },
                new UserHierarchyInfo { ParentUserId = 2, ChildUserId = 5, SiteId = 1 },
                new UserHierarchyInfo { ParentUserId = 3, ChildUserId = 6, SiteId = 1 },
                new UserHierarchyInfo { ParentUserId = 3, ChildUserId = 7, SiteId = 1 },
                new UserHierarchyInfo { ParentUserId = 3, ChildUserId = 8, SiteId = 1 },
                new UserHierarchyInfo { ParentUserId = 5, ChildUserId = 9, SiteId = 1 },
                new UserHierarchyInfo { ParentUserId = 5, ChildUserId = 10, SiteId = 1 },
                new UserHierarchyInfo { ParentUserId = 6, ChildUserId = 5, SiteId = 1 },
                new UserHierarchyInfo { ParentUserId = 7, ChildUserId = 11, SiteId = 1 },
                new UserHierarchyInfo { ParentUserId = 7, ChildUserId = 12, SiteId = 1 },
                new UserHierarchyInfo { ParentUserId = 7, ChildUserId = 13, SiteId = 1 },
                new UserHierarchyInfo { ParentUserId = 10, ChildUserId = 2, SiteId = 1 },
                new UserHierarchyInfo { ParentUserId = 11, ChildUserId = 2, SiteId = 1 },
                new UserHierarchyInfo { ParentUserId = 12, ChildUserId = 14, SiteId = 1 },
                new UserHierarchyInfo { ParentUserId = 12, ChildUserId = 15, SiteId = 1 },
                new UserHierarchyInfo { ParentUserId = 12, ChildUserId = 16, SiteId = 1 }
                );
        }

        [Test]
        public void NonNullResult()
        {
            var childs = UserHierarchyInfoProvider.GetAllChilds(2, 1);
            Assert.IsNotNull(childs);
        }

        [Test]
        public void UserWithoutChilds()
        {
            var childs = UserHierarchyInfoProvider.GetAllChilds(1, 8);
            Assert.IsEmpty(childs);
        }

        [Test]
        public void UserWithOneLevelChilds()
        {
            var childs = UserHierarchyInfoProvider.GetAllChilds(1, 12).ToList();
            Assert.AreEqual(3, childs.Count());
            Assert.Contains(14, childs);
            Assert.Contains(15, childs);
            Assert.Contains(16, childs);
        }

        [Test]
        public void UserWithMultipleLevelChilds()
        {
            var childs = UserHierarchyInfoProvider.GetAllChilds(1, 7).ToList();
            Assert.AreEqual(11, childs.Count());
            Assert.Contains(11, childs);
            Assert.Contains(12, childs);
            Assert.Contains(13, childs);
            Assert.Contains(14, childs);
            Assert.Contains(15, childs);
            Assert.Contains(16, childs);
            Assert.Contains(2, childs);
            Assert.Contains(4, childs);
            Assert.Contains(5, childs);
            Assert.Contains(9, childs);
            Assert.Contains(10, childs);
        }

        [Test]
        public void UserWithCycledChilds()
        {
            var childs = UserHierarchyInfoProvider.GetAllChilds(1, 5).ToList();
            Assert.AreEqual(4, childs.Count());
            if (childs.Count() > 0)
            {
                var grouped = childs.GroupBy(v => v, v => v).Where(s => s.Count() > 1);
                Assert.AreEqual(0, grouped.Count());
            }
            else
                Assert.Fail();
            Assert.Contains(9, childs);
            Assert.Contains(10, childs);
            Assert.Contains(2, childs);
            Assert.Contains(4, childs);
        }
    }
}
