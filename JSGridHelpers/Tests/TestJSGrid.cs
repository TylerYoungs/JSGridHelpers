using System;
using System.Collections.Generic;
using JSGridModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Models;

namespace Tests
{
    [TestClass]
    public class TestJSGrid
    {
        private IList<TestClass> _testClasses;

        private string _testTitle;
        private bool _allowEditing;
        private int _headersCount;
        private int _recordCount;

        private JSGridTable _jsGridTable;

        [TestInitialize]
        public void TestSetup()
        {
            SetDefaultValues();
        }

        [TestMethod]
        public void ItWorksForAnEmptyList()
        {
            CreateJSGridTable();
        }

        [TestMethod]
        public void ItWorksForANullList()
        {
            _testClasses = null;
            CreateJSGridTable();
        }

        [TestMethod]
        public void ItTestsListWithMultipleItems()
        {
            var currentDate = DateTime.Now;
            _recordCount = 3;

            for (int i = 0; i < _recordCount; i++)
            {
                _testClasses.Add(new TestClass { DateTime = currentDate, NullableDateTime = currentDate, Number = i, String = "Test" + i});
            }

            CreateJSGridTable();
        }

        [TestCleanup]
        public void VerifyJSGridTableHasCorrectValues()
        {
            Assert.AreEqual(_jsGridTable.editing, _allowEditing);
            Assert.AreEqual(_jsGridTable.Name, _testTitle);
            Assert.AreEqual(_jsGridTable.data.Count, _recordCount);

            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            _testClasses = new List<TestClass>();

            _recordCount = 0;

            _testTitle = "TestTitle";
            _allowEditing = false;

            SetHeadersCount<TestClass>();
        }

        private void SetHeadersCount<TestClassType>() where TestClassType : class
        {
            _headersCount = typeof(TestClassType).GetProperties().Length;
        }

        private void CreateJSGridTable()
        {
            _jsGridTable = JSGridTable.GetJSGridTableFromIEnumerableOfData(_testClasses, _testTitle, _allowEditing);
        }
    }
}
