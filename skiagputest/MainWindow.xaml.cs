using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SkiaSharp;
using ValidationResult = System.Windows.Controls.ValidationResult;

namespace skiagputest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                AppBorder.Child = new AngleControl(AppBorder.Width - 2*(AppBorder.BorderThickness.Left),AppBorder.Height - 2*AppBorder.BorderThickness.Top);
            };
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            AppBorder.Child.InvalidateVisual();
        }
    }
}
