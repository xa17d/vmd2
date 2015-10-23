using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Vmd2.Processing;
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

        private void image_Loaded(object sender, RoutedEventArgs e)
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

            display = new DisplayImage(image.LengthX, image.LengthY);

            var tf = new TransferFunction1D();
            tf.Add(0, Colors.Black);
            tf.Add(max * 0.2, Colors.Blue);
            tf.Add(max * 0.6, Colors.Red);
            tf.Add(max, Colors.Yellow);

            renderer = new TransferFunctionRenderer(image, display, tf);

            slider.Maximum = image.LengthZ - 1;
            this.image.Source = display.GetBitmap();

            Render();
        }

        private void Render()
        {
            renderer.Slice = (int)slider.Value;

            using (var progress = Log.P("Render slice " + renderer.Slice))
            {
                renderer.Render();
                display.Update();
                //pictureBoxDisplay.Invalidate();
            }
        }

        private DisplayImage display;
        private TransferFunctionRenderer renderer;

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Render();
        }
    }
}
