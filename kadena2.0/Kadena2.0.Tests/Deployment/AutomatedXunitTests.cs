using System;
using Xunit;

namespace Kadena.Tests.Deployment
{
    /// <summary>
    /// These test are here just to verify Automated running of xunit tests during deployment.
    /// They NEED TO BE REMOVED or COMMENTED OUT after checking pipeline
    /// </summary>
    public class AutomatedXunitTests
    {
        //[Fact]
        //public void FailingTestAssert()
        //{
        //    Assert.True(false, "This Assert should fail just to test automated running of xunittests.");
        //}

        //[Fact]
        //public void FailingTestException()
        //{
        //    throw new Exception("This test should fail by this exception just to test automated running of xunittests.");
        //}
    }
}
