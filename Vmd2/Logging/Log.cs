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
            WriteMessage(message, null, MessageType.Info);
        }

        public static Progress P(string message)
        {
            var progress = new Progress();
            WriteMessage(message, progress, MessageType.Info);
            return progress;
        }

        private static void WriteMessage(string message, Progress progress, MessageType type)
        {
            var c = Control;
            if (c!= null)
            {
                c.Write(message, progress, type);
            }
        }

        public static ControlLog Control { get; set; }

        public static void E(string message)
        {
            WriteMessage(message, null, MessageType.Error);
        }
    }
}
