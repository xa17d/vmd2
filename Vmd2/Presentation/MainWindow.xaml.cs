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
using Vmd2.DataAccess;
using Vmd2.Logging;
using Vmd2.Presentation.ViewModels;
using Vmd2.Processing;
using Vmd2.Processing.DVR;
using Vmd2.Processing.Helper;
using Vmd2.Processing.TransferFunctions;

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
        }

        private DisplayImage display;
        private RenderDvr renderDvr;
        private RenderSlice renderSlice;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            renderSlice = new RenderSlice();
            renderDvr = new RenderDvr();
            
            tabItemDvr.DataContext = renderDvr;
            tabItemSlice.DataContext = renderSlice;

            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadImage), null);
        }

        private void LoadImage(object state)
        {
            Image3D image;

            //string testPath = @"MANIX\CER-CT\ANGIO CT";
            //string testPath = @"BRAINIX\SOUS - 702";
            //string testPath = @"BRAINIX\T2W-FE-EPI - 501";
            string testPath = @"vtkBrain";

            double min, max;
            using (var reader = new DicomReader(TestData.GetPath(testPath)))
            {
                image = reader.ReadImage3D();

                min = reader.MinValue;
                max = reader.MaxValue;
            }


            var tf = new TransferFunction1D();
            tf.Add(-1, ColorHelper.Alpha(0, Colors.Black));
            tf.Add(max * 0.2, ColorHelper.Alpha(200, Colors.Blue));
            tf.Add(max * 0.6, ColorHelper.Alpha(200, Colors.Red));
            tf.Add(max, Colors.Yellow);

            UpdateVms(image, tf);
        }

        private void UpdateVms(Image3D image, TransferFunction1D tf)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                display = new DisplayImage(image.LengthX, image.LengthY);

                renderDvr.Image = image;
                renderDvr.Renderer = new DvrRenderer(image, display, tf) { ThreadCount = 8 };

                renderSlice.Image = image;
                renderSlice.Renderer = new TransferFunction1DRenderer(image, display, tf);

                slider.Maximum = image.LengthZ - 1;
                this.image.Source = display.GetBitmap();
            }));

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                var vm = SelectedVm;
                if (vm != null)
                {
                    vm.Render();
                }
            }
        }

        private RenderVm SelectedVm
        {
            get
            {
                var tabItem = (tabControl.SelectedItem as FrameworkElement);
                if (tabItem != null)
                {
                    var vm = (tabItem.DataContext as RenderVm);
                    if (vm != null)
                    {
                        return vm;
                    }
                }
                return null;
            }
        }
    }
}
