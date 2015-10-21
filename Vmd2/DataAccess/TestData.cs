using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.DataAccess
{
    static class TestData
    {
        private static string ExeDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        public static string GetPath(string dataDirectoryName)
        {
            return System.IO.Path.Combine(ExeDirectory, "..", "..", "..", "..", "TestData", dataDirectoryName);
        }
    }
}
