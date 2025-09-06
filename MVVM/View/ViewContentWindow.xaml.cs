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
using System.Windows.Shapes;

namespace PasteToFile.MVVM.View
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class ViewContentWindow : Window
    {
        public ViewContentWindow()
        {
            InitializeComponent();
            this.SizeChanged += MainWindow_SizeChanged;
            //this.LocationChanged += MainWindow_LocationChanged;
            CenterWindow();
        }
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CenterWindow();
        }

        private void MainWindow_LocationChanged(object sender, EventArgs e)
        {
            CenterWindow();
        }
        private void CenterWindow()
        {
            var workingArea = SystemParameters.WorkArea;


            this.Left = (workingArea.Width - this.Width) / 2;
            this.Top = (workingArea.Height - this.Height) / 2;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
