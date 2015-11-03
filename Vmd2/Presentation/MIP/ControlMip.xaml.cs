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
using Vmd2.Processing.MIP;

namespace Vmd2.Presentation.MIP
{
    /// <summary>
    /// Interaction logic for ControlMip.xaml
    /// </summary>
    [ProcessingControl(typeof(MipRenderer))]
    public partial class ControlMip : UserControl
    {
        public ControlMip()
        {
            InitializeComponent();

            sliderCenter.Minimum = 0;
            sliderCenter.Maximum = 1000;

            sliderWidth.Minimum = 1;
            sliderWidth.Maximum = 1000;
        }
    }
}
