using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using PasteToFile.MVVM.Model;
using PasteToFile.MVVM.ViewModel;

namespace PasteToFile.MVVM.View
{
    /// <summary>
    /// Interaction logic for ClipboardView.xaml
    /// </summary>
    public partial class ClipboardView : UserControl
    {
        public ClipboardView()
        {
            InitializeComponent();

        }

        private void clipboardContentDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("UI loaded");
        }


        private bool IsClickInsideButton(object originalSource)
        {
            // Walk up the visual tree to see if the click came from a Button
            DependencyObject dep = originalSource as DependencyObject;
            while (dep != null)
            {
                if (dep is Button) return true;
                dep = VisualTreeHelper.GetParent(dep);
            }
            return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
