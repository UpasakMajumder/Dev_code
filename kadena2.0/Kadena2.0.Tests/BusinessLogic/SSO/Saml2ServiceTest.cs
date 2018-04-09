using Xunit;
using Kadena.BusinessLogic.Services.SSO;
using Kadena.BusinessLogic.Contracts.SSO;
using System.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Linq;
using System.Collections.Generic;

namespace Kadena.Tests.BusinessLogic.SSO
{
    public class Saml2ServiceTest : KadenaUnitTest<Saml2Service>
    {
        private class TestSaml2SecurityTokenHandler : Saml2SecurityTokenHandler
        {
            private Dictionary<string, IEnumerable<string>> attributes;

            public TestSaml2SecurityTokenHandler(Dictionary<string, IEnumerable<string>> attributes) : base()
            {
                this.attributes = attributes;
                this.Configuration = new SecurityTokenHandlerConfiguration();
            }

            public override ReadOnlyCollection<ClaimsIdentity> ValidateToken(SecurityToken token)
            {
                return new ReadOnlyCollection<ClaimsIdentity>(Enumerable.Empty<ClaimsIdentity>().ToList());
            }

            public override SecurityToken ReadToken(string tokenString)
            {
                var token = new Saml2SecurityToken(new Saml2Assertion(new Saml2NameIdentifier("TestIssuer")));
                if (attributes != null)
                {
                    var attributeStatement = new Saml2AttributeStatement();
                    foreach (var kv in attributes)
                    {
                        attributeStatement.Attributes.Add(new Saml2Attribute(kv.Key, kv.Value));
                    }
                    token.Assertion.Statements.Add(attributeStatement);
                }
                return token;
            }
        }

        public static IEnumerable<object[]> GetAttributes()
        {
            yield return new object[]
            {
                new Dictionary<string, IEnumerable<string>> ()
            };
            yield return new object[]
            {
                new Dictionary<string, IEnumerable<string>> {
                    { "Attr1", new List<string> { "val1" } }
                }
            };
            yield return new object[]
            {
                new Dictionary<string, IEnumerable<string>> {
                    { "Attr1", new List<string> { "val1" } },
                    { "Attr2", new List<string> { "val1" } }
                }
            };
        }

        [Fact(DisplayName = "Saml2Service.GetAttributes() | Token read exception")]
        public void GetAttributesTokenReadExeption()
        {
            var actualResult = Sut.GetAttributes(string.Empty);

            Assert.Null(actualResult);
        }

        [Fact(DisplayName = "Saml2Service.GetAttributes() | Null attribute statement")]
        public void GetAttributesNull()
        {
            Setup<ISaml2HandlerService, Saml2SecurityTokenHandler>(s => s.GetTokenHandler(), new TestSaml2SecurityTokenHandler(null));

            var actualResult = Sut.GetAttributes(string.Empty);

            Assert.Null(actualResult);
        }

        [Theory(DisplayName = "Saml2Service.GetAttributes() | Non null attribute statement")]
        [MemberData(nameof(GetAttributes))]
        public void GetAttributesNoNull(Dictionary<string, IEnumerable<string>> expectedResult)
        {
            Setup<ISaml2HandlerService, Saml2SecurityTokenHandler>(s => s.GetTokenHandler(), new TestSaml2SecurityTokenHandler(expectedResult));

            var actualResult = Sut.GetAttributes(string.Empty);

            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult.Count, actualResult.Count);
        }
    }
}
