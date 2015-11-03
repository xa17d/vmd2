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

namespace Vmd2.Presentation
{
    /// <summary>
    /// Interaction logic for PeContainer.xaml
    /// </summary>
    public partial class PeContainer : UserControl
    {
        public PeContainer()
        {
            InitializeComponent();

            DataContextChanged += PeContainer_DataContextChanged;
        }

        private void PeContainer_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var type = e.NewValue.GetType();
            var control = Activator.CreateInstance(ProcessingControl.GetControlFromProcessingElement(type));
            ((FrameworkElement)control).DataContext = e.NewValue;
            container.Content = control;
        }

        private ProcessingElement Element { get { return (ProcessingElement)DataContext; } }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            Element.Pipeline.Remove(Element);
        }
    }
}
