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
using Vmd2.Processing.Filters;

namespace Vmd2.Presentation.Filter
{
    /// <summary>
    /// Interaction logic for ControlFilter.xaml
    /// </summary>
    [ProcessingControl(typeof(Gaussian2DFilter3x3))]
    [ProcessingControl(typeof(Gaussian2DFilter7x7))]
    [ProcessingControl(typeof(Laplace2DFilter3x3))]
    [ProcessingControl(typeof(Hildreth2DFilter7x7))]
    [ProcessingControl(typeof(Gaussian3DFilter3x3))]
    [ProcessingControl(typeof(EdgeDFilter3x3))]
    [ProcessingControl(typeof(TransformYView))]
    public partial class ControlFilter : UserControl
    {
        public ControlFilter()
        {
            InitializeComponent();
        }
    }
}
