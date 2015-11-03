﻿using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Vmd2.DataAccess;
using Vmd2.Logging;
using Vmd2.Presentation.ViewModels;
using Vmd2.Processing;
using Vmd2.Processing.DVR;
using Vmd2.Processing.Helper;
using Vmd2.Processing.TransferFunctions;
using Vmd2.Processing.Mapping;
using Vmd2.Processing.MIP;
using Vmd2.Processing.Filter;

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
        private RenderWindowing renderWindowing;
        private Windowing window;
        private RenderMip renderMip;
        private Image3D image;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            renderSlice = new RenderSlice();
            renderDvr = new RenderDvr();
            renderWindowing = new RenderWindowing();
            renderMip = new RenderMip();

            tabItemDvr.DataContext = renderDvr;
            tabItemSlice.DataContext = renderSlice;
            tabItemWindowing.DataContext = renderWindowing;
            tabItemMip.DataContext = renderMip;

            ThreadPool.QueueUserWorkItem(new WaitCallback(LoadImage), null);

            int max = 1100;
            controlTfSlice.AddItem(max * 0.0, ColorHelper.Alpha(0, Colors.Black));
            controlTfSlice.AddItem(max * 0.2, ColorHelper.Alpha(200, Colors.Blue));
            controlTfSlice.AddItem(max * 0.4, ColorHelper.Alpha(200, Colors.Red));
            controlTfSlice.AddItem(max * 0.6, ColorHelper.Alpha(200, Colors.Yellow));
            controlTfSlice.AddItem(max * 0.8, ColorHelper.Alpha(200, Colors.Violet));
            controlTfSlice.AddItem(max * 1.0, ColorHelper.Alpha(200, Colors.Lime));
        }

        private void LoadImage(object state)
        {
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
            this.window = new Windowing();

            tf.Add(max * 0.0, ColorHelper.Alpha(0, Colors.Black));
            tf.Add(max * 0.2, ColorHelper.Alpha(200, Colors.Blue));
            tf.Add(max * 0.4, ColorHelper.Alpha(200, Colors.Red));
            tf.Add(max * 0.6, ColorHelper.Alpha(200, Colors.Yellow));
            tf.Add(max * 0.8, ColorHelper.Alpha(200, Colors.Violet));
            tf.Add(max * 1.0, ColorHelper.Alpha(200, Colors.Lime));

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

                //TODO integrate in ui
                //FilterRenderer filter = new GaussianFilter3x3();
                FilterRenderer filter = new GaussianFilter7x7();
                //FilterRenderer filter = new GradientFilter3x3();
                //filter.Activated = false;

                renderWindowing.Image = image;
                renderWindowing.Renderer = new WindowingRenderer(image, display, window, filter);

                renderMip.Image = image;
                renderMip.Renderer = new MipRenderer(image, display, window) { ThreadCount = 8 };

                sliderTf.Maximum = image.LengthZ - 1;
                sliderWindowing.Maximum = image.LengthZ - 1;

                this.displayImage.Source = display.GetBitmap();
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

        private void Button_Render_Click(object sender, RoutedEventArgs e)
        {
            var vm = SelectedVm;
            if (vm != null)
            {
                UpdateVms(image, controlTfSlice.CreateTransferFunction());
                vm.Render();
            }
        }
    }
}
