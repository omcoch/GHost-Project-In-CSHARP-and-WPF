using Microsoft.VisualStudio.TestTools.UnitTesting;
using BE;
using BL;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Host h = new Host()
            {
                ChargeAmount = 4,
                FamilyName = "moshe"
            };
            Host g = Cloning.Clone(h);
            g.FamilyName = "2";
            g.FamilyName = "2";
            
        }
    }
}
