using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Vmd2.Logging;

namespace Vmd2.Presentation
{
    /// <summary>
    /// Interaction logic for ControlLog.xaml
    /// </summary>
    public partial class ControlLog : UserControl
    {
        public ControlLog()
        {
            InitializeComponent();

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 250);
            dispatcherTimer.Start();
        }

        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            UpdateProgress();
        }

        internal void Write(string message, Progress progress, MessageType type)
        {
            Dispatcher.Invoke(new WriteDelegate(UiWrite), TimeSpan.FromSeconds(5), message, progress, type);
        }

        private delegate void WriteDelegate(string message, Progress progress, MessageType type);

        private void UiWrite(string message, Progress progress, MessageType type)
        {
            UpdateProgress();

            if (progress == null)
            {
                var textBlock = new TextBlock();
                textBlock.Text = message;
                stack.Children.Add(textBlock);

                if (type == MessageType.Error)
                {
                    textBlock.Foreground = Brushes.OrangeRed;
                    textBlock.FontWeight = FontWeights.Bold;
                }
            }
            else
            {
                var s = new StackPanel();
                s.Orientation = Orientation.Horizontal;
                var textBlockMessage = new TextBlock();
                var textBlockProgress = new TextBlock();
                textBlockMessage.Text = message;
                textBlockProgress.Foreground = Brushes.Blue;

                s.Children.Add(textBlockMessage);
                s.Children.Add(textBlockProgress);

                stack.Children.Add(s);

                var info = new ProgressInfo();
                info.Progress = progress;
                info.Block = textBlockProgress;
                info.Update();
                ongoingProgresses.Add(info);
            }
            scrollViewer.ScrollToEnd();
        }

        public void UpdateProgress()
        {
            for (int i = 0; i < ongoingProgresses.Count; i++)
            {
                var info = ongoingProgresses[i];
                if (info.Progress.Status != ProgressStatus.Running)
                {
                    ongoingProgresses.RemoveAt(i);
                    i--;
                }

                // also update if done
                info.Update();
            }
        }

        private class ProgressInfo
        {
            public TextBlock Block;
            public Progress Progress;

            public void Update()
            {
                Block.Text = "   " + Progress.ToString();
                if (Progress.Status == ProgressStatus.Done) { Block.Foreground = Brushes.Green; }
                else if (Progress.Status == ProgressStatus.Aborted) { Block.Foreground = Brushes.OrangeRed; }
            }
        }

        private List<ProgressInfo> ongoingProgresses = new List<ProgressInfo>();
    }
}
