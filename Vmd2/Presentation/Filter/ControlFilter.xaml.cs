using System.Windows.Controls;
using Vmd2.Processing;
using Vmd2.Processing.Filters;

namespace Vmd2.Presentation.Filter
{
    /// <summary>
    /// Interaction logic for ControlFilter.xaml
    /// </summary>
    [ProcessingControl(typeof(ContrastEnhancement2DFilter3x3))]
    [ProcessingControl(typeof(Gaussian2DFilter3x3))]
    [ProcessingControl(typeof(Gaussian2DFilter7x7))]
    [ProcessingControl(typeof(Hildreth2DFilter7x7))]
    [ProcessingControl(typeof(Laplace2DFilter3x3))]
    [ProcessingControl(typeof(ContrastEnhancement3DFilter3x3))]
    [ProcessingControl(typeof(Gaussian3DFilter3x3))]
    [ProcessingControl(typeof(TransformYView))]
    public partial class ControlFilter : UserControl
    {
        public ControlFilter()
        {
            InitializeComponent();
        }
    }
}
