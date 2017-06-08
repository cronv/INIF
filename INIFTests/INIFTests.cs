using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace INIF.Tests
{
    [TestClass()]
    public class INIFTests
    {
        [TestMethod()]
        public void INIFTest()
        {
            INIF ini = INIF.getInstance("test_write.ini");
            Assert.IsFalse(File.Exists(ini.Path));
        }

        [TestMethod()]
        public void WriteTest()
        {
            INIF ini = INIF.getInstance("test_write.ini");
            ini.Write("SectionTestNameTest", "VersionStringTest", "v0.1-test");
            Assert.IsTrue(File.Exists(ini.Path));

            var GetStringValueByKey = ini.Get("SectionTestNameTest", "VersionStringTest");
            Assert.AreEqual(GetStringValueByKey, "v0.1-test");
        }

        [TestMethod()]
        public void GetTest()
        {
            INIF ini = INIF.getInstance("test.ini");
            var GetStringValueByKey = ini.Get("SectionTestNameTest", "VersionStringTest");
            var GetIntValueByKey = ini.Get("SectionTestNameTest", "VersionIntTest");
            var GetFloatValueByKey = ini.Get("SectionTestNameTest", "VersionFloatTest");

            if (!File.Exists(ini.Path))
            {
                Assert.Inconclusive();
            }

            Assert.AreEqual(GetStringValueByKey, "v0.1-test");
            Assert.AreEqual(GetIntValueByKey, "1234567654321");
            Assert.AreEqual(GetFloatValueByKey, "3.14159265358979323846264338327950288419716939937510582097494459230781640628620899862803482534211706798214808651328230664709384460955058223172535940812848111745028410270193852110555964462294895493038196442");
        }

        [TestMethod()]
        public void DeleteSectionTest()
        {
            INIF ini = INIF.getInstance("test_write.ini");
            ini.DeleteSection("SectionTestNameTest");

            var stringEmpty = File.ReadAllText(ini.Path);
            Assert.IsTrue(string.IsNullOrEmpty(stringEmpty));
            File.Delete(INIF.getInstance("test_write.ini").Path);
        }

        private void backupFileTest()
        {
            INIF ini = INIF.getInstance("test.ini");
            ini.Write("SectionTestNameTest", "VersionStringTest", "v0.1-test");
            ini.Write("SectionTestNameTest", "VersionIntTest", "1234567654321");
            ini.Write("SectionTestNameTest", "VersionFloatTest", "3.14159265358979323846264338327950288419716939937510582097494459230781640628620899862803482534211706798214808651328230664709384460955058223172535940812848111745028410270193852110555964462294895493038196442");
        }

        ~INIFTests()
        {
            this.backupFileTest();
        }
    }
}