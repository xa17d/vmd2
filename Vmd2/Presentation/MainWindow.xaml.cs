using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Vmd2.DataAccess;
using Vmd2.Logging;
using Vmd2.Processing;
using Vmd2.Processing.DVR;
using Vmd2.Processing.Helper;
using Vmd2.Processing.TransferFunctions;
using Vmd2.Processing.Mapping;
using Vmd2.Processing.MIP;
using System.Windows.Threading;

namespace Vmd2.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisplay
    {
        public MainWindow()
        {
            InitializeComponent();
            Log.Control = controlLog;
            pipeline = new ProcessingPipeline();
            pipeline.Add(new ImageLoader());
            pipeline.Add(new Slice());
            pipeline.Add(new TransferFunction1DRenderer() { Display = this });
            pipeline.PipelineChanged += Pipeline_PipelineChanged;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private ProcessingPipeline pipeline;
        private bool pipelineDirty = false;
        private Thread processThread = null;

        private DisplayImage display;

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (pipelineDirty)
            {
                pipelineDirty = false;
                ProcessPipelineAsync();
            }
        }

        private void ProcessPipelineAsync()
        {
            var thread = processThread;
            if (thread != null)
            {
                if (thread.IsAlive)
                {
                    thread.Abort();
                }
            }

            thread = new Thread(new ThreadStart(ProcessPipeline));
            processThread = thread;
            thread.IsBackground = true;
            thread.Start();
        }

        private void ProcessPipeline()
        {
            Log.I("===== Pipeline Start =====");
            pipeline.Process();

            if (display != null)
            {
                Dispatcher.Invoke(display.Update);
            }
            Log.I("===== Done =====");
        }

        private void Pipeline_PipelineChanged(object sender, EventArgs e)
        {
            pipelineDirty = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            processingElements.ItemsSource = pipeline;

            // Show ProcessingControls:
            foreach (var item in ProcessingControl.FindAllControls())
            {
                var button = new Button();
                button.Content = item.Name;
                button.Tag = item;
                button.Click += Button_Add_Click;
                panelAdd.Children.Add(button);
            }

            // Inital Process
            ProcessPipelineAsync();
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            var processingElementType = (Type)((Button)sender).Tag;

            var element = (ProcessingElement)Activator.CreateInstance(processingElementType);

            var renderer = (element as Renderer);
            if (renderer != null)
            {
                renderer.Display = this;
            }

            pipeline.Add(element);
        }

        private void buttonProcessPipeline_Click(object sender, RoutedEventArgs e)
        {
            ProcessPipelineAsync();
        }

        DisplayImage IDisplay.GetDisplay(int width, int height)
        {
            if (display == null || display.Width != width || display.Height != height)
            {
                Dispatcher.Invoke(
                    () =>
                    {
                        display = new DisplayImage(width, height);
                        displayImage.Source = display.GetBitmap();
                    }
                );
            }

            return display;
        }
    }
}
