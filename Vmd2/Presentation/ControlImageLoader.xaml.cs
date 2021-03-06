﻿using System;
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
    /// Interaction logic for ControlImageLoader.xaml
    /// </summary>
    [ProcessingControl(typeof(ImageLoader))]
    public partial class ControlImageLoader : UserControl
    {
        public ControlImageLoader()
        {
            InitializeComponent();
        }

        private ImageLoader Element { get { return (ImageLoader)DataContext; } }

        private System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();

        private void Button_SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            dialog.SelectedPath = Element.Path;
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Element.Path = dialog.SelectedPath;
            }
        }
    }
}
