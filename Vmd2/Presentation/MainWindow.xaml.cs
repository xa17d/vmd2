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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Log.Control = controlLog;

            pipelines = new PipelineExamples();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private PipelineExamples pipelines;
        private ProcessingPipeline __pipeline;
        public object Pipeline
        {
            get { return __pipeline; }
            set
            {
                var newValue = (value as ProcessingPipeline);

                if (newValue != __pipeline)
                {
                    if (__pipeline != null)
                    {
                        __pipeline.PipelineChanged -= Pipeline_PipelineChanged;
                    }

                    __pipeline = newValue;
                    processingElements.ItemsSource = newValue;

                    if (__pipeline != null)
                    {
                        foreach (var item in __pipeline)
                        {
                            if (item is INeedDisplay)
                            {
                                ((INeedDisplay)item).Display = displayControl;
                            }
                        }

                        newValue.PipelineChanged += Pipeline_PipelineChanged;

                        Pipeline_PipelineChanged(__pipeline, EventArgs.Empty);
                    }
                }
            }
        }

        private bool pipelineDirty = false;
        private Thread processThread = null;

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
            var p = (ProcessingPipeline)Pipeline;

            Log.H("Pipeline Start \""+p.Name+"\"");

            if (p != null) { p.Process(); }

            displayControl.Update();

            Log.I("Done.");
        }

        private void Pipeline_PipelineChanged(object sender, EventArgs e)
        {
            pipelineDirty = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Pipeline = pipelines.Examples[0];

            comboBoxPipelines.DataContext = this;
            comboBoxPipelines.ItemsSource = pipelines.Examples;

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

            var elementWithDisplay = (element as INeedDisplay);
            if (elementWithDisplay != null)
            {
                elementWithDisplay.Display = displayControl;
            }

            ((ProcessingPipeline)Pipeline).Add(element);
        }

        private void buttonProcessPipeline_Click(object sender, RoutedEventArgs e)
        {
            ProcessPipelineAsync();
        }
    }
}
