using NUnit.Framework;

namespace MainApp.RegressionTests
{
    [TestFixture]
    public partial class RegressionTests
    {

        [Test]
        public void Test_DoStringsMatch_ShouldPass()
        {
            string expected = "Hello";
            string actual = "Hello";
            Assert.AreEqual(expected, actual, "Input strings don't match");
        }

        [Test]
        public void Test_DoStringsMatch_ShouldFail()
        {
            string expected = "Hello";
            string actual = "World";
            Assert.AreEqual(expected, actual, "Input strings don't match");
        }


    }
}