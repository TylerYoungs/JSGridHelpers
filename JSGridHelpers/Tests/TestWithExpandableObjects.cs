using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSGridModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class TestWithExpandableObjects
    {
        private int _headersCount;
        private int _recordCount;

        [TestInitialize]
        public void TestSetup()
        {
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
        }

        [TestMethod]
        public void VerifyJSGridTableHasHeadersAndRowsForExpandableObjects()
        {
            var i = 0;

            var expandos = new List<ExpandoObject>();

            expandos.Add(GetExpandoObject(i.ToString()));
            i++;
            expandos.Add(GetExpandoObject(i.ToString()));

            var jsGridTable = JSGridTable.GetJSGridTableFromIEnumerableOfData(expandos, "Test", false);

            Assert.AreEqual(jsGridTable.data.Count(), 2);
            Assert.AreEqual(jsGridTable.fields.Count(), 4);
        }

        public ExpandoObject GetExpandoObject(string i)
        {
            var expandoObject = new ExpandoObject() as IDictionary<string, object>;

            expandoObject.Add($"TransferExtension{i}", $"TestTransferExtension{i}");
            expandoObject.Add($"Description{i}", $"TestDescription{i}");
            expandoObject.Add(DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy"), "110");
            expandoObject.Add(DateTime.Now.Day.ToString("MM/dd/yyyy"), "10");

            return expandoObject as ExpandoObject;
        }

        [TestCleanup]
        public void VerifyJSGridTableHasCorrectValues()
        {

        }
    }
}
