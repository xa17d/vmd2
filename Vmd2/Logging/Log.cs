using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vmd2.Presentation;

namespace Vmd2.Logging
{
    /// <summary>
    /// Logging Class
    /// </summary>
    static class Log
    {
        /// <summary>
        /// Log Info
        /// </summary>
        /// <param name="message">Message to show</param>
        public static void I(string message)
        {
            WriteMessage(message, null, MessageType.Info);
        }

        /// <summary>
        /// Show info message with a progress
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <returns>Progress-Object</returns>
        public static Progress P(string message)
        {
            var progress = new Progress();
            WriteMessage(message, progress, MessageType.Info);
            return progress;
        }

        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="message">Message to show</param>
        public static void E(string message)
        {
            WriteMessage(message, null, MessageType.Error);
        }

        /// <summary>
        /// Log a header
        /// </summary>
        /// <param name="message">Header message</param>
        public static void H(string message)
        {
            WriteMessage(message, null, MessageType.Header);
        }

        private static void WriteMessage(string message, Progress progress, MessageType type)
        {
            var c = Control;
            if (c!= null)
            {
                c.Write(message, progress, type);
            }
        }

        /// <summary>
        /// Control which is used to show the log messages
        /// </summary>
        public static ControlLog Control { get; set; }
    }
}
