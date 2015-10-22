using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vmd2.DataAccess;
using Vmd2.Logging;
using Vmd2.Processing;
using Vmd2.Processing.TransferFunctions;

namespace Vmd2.Presentation
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            Log.Control = controlLog1;
        }

        private void FormMain_Load(object sender, EventArgs e)
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
            tf.Add(0, Color.Black);
            tf.Add(max * 0.2, Color.Blue);
            tf.Add(max * 0.6, Color.Red);
            tf.Add(max, Color.Yellow);

            renderer = new TransferFunctionRenderer(image, display, tf);

            scrollBarSlice.Maximum = image.LengthZ - 1;
            pictureBoxDisplay.Image = display.GetBitmap();

            //Render();
        }

        private void Render()
        {
            using (var progress = Log.P("Render slice " + renderer.Slice))
            {
                renderer.Slice = scrollBarSlice.Value;

                renderer.Render();
                display.Update();
                pictureBoxDisplay.Invalidate();
            }
        }

        private DisplayImage display;
        private TransferFunctionRenderer renderer;

        private void scrollBarSlice_Scroll(object sender, ScrollEventArgs e)
        {
            Render();
        }
    }
}
