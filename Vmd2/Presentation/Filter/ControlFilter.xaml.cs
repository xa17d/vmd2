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
using Vmd2.Processing.Filters;

namespace Vmd2.Presentation.Filter
{
    /// <summary>
    /// Interaction logic for ControlFilter.xaml
    /// </summary>
    [ProcessingControl(typeof(GaussianFilter3x3))]
    [ProcessingControl(typeof(GaussianFilter7x7))]
    [ProcessingControl(typeof(GradientFilter3x3))]
    public partial class ControlFilter : UserControl
    {
        public ControlFilter()
        {
            InitializeComponent();
        }
    }
}
