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
using Vmd2.Processing.Mapping;
using Vmd2.Processing.MIP;
using Vmd2.Processing.Segmentation;

namespace Vmd2.Presentation.MIP
{
    /// <summary>
    /// Interaction logic for ControlMip.xaml
    /// </summary>
    [ProcessingControl(typeof(MipRenderer))]
    [ProcessingControl(typeof(WindowingRenderer))]
    [ProcessingControl(typeof(RegionGrowing))]
    public partial class ControlMip : UserControl
    {
        public ControlMip()
        {
            InitializeComponent();
            
        }
    }
}
