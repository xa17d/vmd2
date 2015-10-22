using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vmd2.Presentation;

namespace Vmd2.Logging
{
    static class Log
    {
        public static void I(string message)
        {
            WriteMessage(message, null);
        }

        public static Progress P(string message)
        {
            var progress = new Progress();
            WriteMessage(message, progress);
            return progress;
        }

        private static void WriteMessage(string message, Progress progress)
        {
            var c = Control;
            if (c!= null)
            {
                c.Write(message, progress);
            }
        }

        public static ControlLog Control { get; set; }
    }
}
