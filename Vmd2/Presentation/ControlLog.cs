using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vmd2.Logging;

namespace Vmd2.Presentation
{
    class ControlLog : TextBox
    {
        public ControlLog()
        {
            Text = "";
            Multiline = true;

            Font = new System.Drawing.Font("Consolas", 10);

            ScrollBars = ScrollBars.Both;

            timer = new Timer();
            timer.Interval = 500;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        private Timer timer;

        public void Write(string message, Progress progress)
        {
            Invoke(new LogWriteDelegate(WriteSafe), message, progress);
        }

        private delegate void LogWriteDelegate(string message, Progress progress);

        private void WriteSafe(string message, Progress progress)
        {
            UpdateProgress();
            AppendText(Environment.NewLine + message);

            if (progress != null)
            {AppendText("".PadLeft(50 - message.Length));

                var info = new ProgressInfo();
                info.Progress = progress;
                info.Start = SelectionStart;
                AppendText("".PadLeft(ProgressInfo.MaxTextLength));
                info.End = SelectionStart;
                info.Update(this);
                ongoingProgresses.Add(info);
            }
        }

        public void UpdateProgress()
        {
            for (int i = 0; i < ongoingProgresses.Count; i++)
            {
                var info = ongoingProgresses[i];
                if (info.Progress.IsDone)
                {
                    ongoingProgresses.RemoveAt(i);
                    i--;
                }

                // also update if done
                info.Update(this);
            }
        }

        private class ProgressInfo
        {
            public const int MaxTextLength = 30;

            public int Start;
            public int End;

            public Progress Progress;

            public void Update(ControlLog controlLog)
            {
                var backupSelectionStart = controlLog.SelectionStart;

                controlLog.SelectionStart = Start;
                controlLog.SelectionLength = End - Start;

                var text = Progress.ToString();
                text = text.PadLeft(MaxTextLength).Substring(0, MaxTextLength);
                controlLog.SelectedText = text;

                controlLog.SelectionStart = backupSelectionStart;
                controlLog.SelectionLength = 0;
            }
        }

        private List<ProgressInfo> ongoingProgresses = new List<ProgressInfo>();
    }
}
