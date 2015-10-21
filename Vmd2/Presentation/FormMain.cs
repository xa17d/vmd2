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
using Vmd2.Processing;
using Vmd2.Processing.TransferFunctions;

namespace Vmd2.Presentation
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Image3D image;

            using (var reader = new DicomReader(TestData.GetPath("vtkBrain")))
            {
                image = reader.ReadImage3D();
            }

            display = new DisplayImage(image.LengthX, image.LengthY);

            var tf = new TransferFunction1D();
            tf.Add(0, Color.Black);
            tf.Add(200, Color.Blue);
            tf.Add(600, Color.Red);
            tf.Add(1100, Color.Yellow);

            renderer = new TransferFunctionRenderer(image, display, tf);

            scrollBarSlice.Maximum = image.LengthZ - 1;
            pictureBoxDisplay.Image = display.GetBitmap();

            Render();
        }

        private void Render()
        {
            renderer.Slice = scrollBarSlice.Value;

            DateTime start = DateTime.Now;

            renderer.Render();
            display.Update();
            pictureBoxDisplay.Invalidate();

            DateTime end = DateTime.Now;
            Debug.WriteLine("Rendered in " + (end - start).TotalMilliseconds + "ms");
        }

        private DisplayImage display;
        private TransferFunctionRenderer renderer;

        private void scrollBarSlice_Scroll(object sender, ScrollEventArgs e)
        {
            Render();
        }
    }
}
