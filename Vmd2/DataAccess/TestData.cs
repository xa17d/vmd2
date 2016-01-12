using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.DataAccess
{
    /// <summary>
    /// Helper to find paths relative to the EXE Directory
    /// </summary>
    static class TestData
    {
        private static string ExeDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }

        public static string GetPath(string dataDirectoryName)
        {
            string path;
            DirectoryInfo dir = new DirectoryInfo(ExeDirectory);
            do
            {
                path = System.IO.Path.Combine(dir.FullName, "TestData", dataDirectoryName);
                dir = dir.Parent;

            } while (!Directory.Exists(path) && dir != null);

            return path;
        }
    }
}
