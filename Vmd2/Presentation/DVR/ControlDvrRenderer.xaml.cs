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
using Vmd2.Processing;
using Vmd2.Processing.DVR;

namespace Vmd2.Presentation.DVR
{
    /// <summary>
    /// Interaction logic for ControlDvrRenderer.xaml
    /// </summary>
    [ProcessingControl(typeof(DvrRenderer))]
    public partial class ControlDvrRenderer : UserControl
    {
        public ControlDvrRenderer()
        {
            InitializeComponent();

            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Element.PreProcessing += Element_PreProcessing;
        }

        private void Element_PreProcessing(object sender, ProcessEventArgs e)
        {
            controlTf.UpdateImage(e.Image);
        }

        private DvrRenderer Element { get { return (DvrRenderer)DataContext; } }

        private void TransferFunctionChanged(object sender, EventArgs e)
        {
            Element.TF = controlTf.CreateTransferFunction();
        }
    }
}
