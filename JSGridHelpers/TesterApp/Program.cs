using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSGridModels;

namespace TesterApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var testClasses = new List<TestClass>();
            var jsGridTable = JSGridTable.GetJSGridTableFromIEnumerableOfData(testClasses, false);

        }
    }
}
