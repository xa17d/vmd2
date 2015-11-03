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
using Vmd2.Processing.TransferFunctions;

namespace Vmd2.Presentation.TransferFunctions
{
    /// <summary>
    /// Interaction logic for ControlTransferFunction1DRenderer.xaml
    /// </summary>
    [ProcessingControl(typeof(TransferFunction1DRenderer))]
    public partial class ControlTransferFunction1DRenderer : UserControl
    {
        public ControlTransferFunction1DRenderer()
        {
            InitializeComponent();
        }

        private TransferFunction1DRenderer Element { get { return (TransferFunction1DRenderer)DataContext; } }

        private void TransferFunctionChanged(object sender, EventArgs e)
        {
            Element.TF = controlTf.CreateTransferFunction();
        }
    }
}
