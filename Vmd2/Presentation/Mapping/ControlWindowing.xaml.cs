using System;
using System.Windows.Controls;

namespace Vmd2.Presentation.Mapping
{
    /// <summary>
    /// Interaction logic for ControlWindowing.xaml
    /// </summary>
    public partial class ControlWindowing : UserControl
    {
        public ControlWindowing()
        {
            InitializeComponent();

            sliderCenter.Minimum = 0;
            sliderCenter.Maximum = 1000;

            sliderWidth.Minimum = 1;
            sliderWidth.Maximum = 1000;

            /*
            double min = 0;
            double max = 1000;
            double WindowCenter = 500;
            double WindowWidth = 300;

            sliderCenter.Minimum = min + 1;
            sliderCenter.Maximum = max - 1;

            sliderWidth.Minimum = 1;

            double lower = Math.Abs(WindowCenter - min);
            double upper = Math.Abs(WindowCenter - max);
            sliderWidth.Maximum = Math.Min(lower, upper);
            */
        }
    }
}
