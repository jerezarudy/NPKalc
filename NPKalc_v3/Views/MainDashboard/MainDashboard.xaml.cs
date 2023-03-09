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

namespace NPKalc_v3.Views.MainDashboard
{
    /// <summary>
    /// Interaction logic for MainDashboard.xaml
    /// </summary>
    public partial class MainDashboard : Window
    {
        public MainDashboard()
        {
            InitializeComponent();
            MainFrame.Navigate(new Calculator.NPKalcPage());
        }
        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        } 
        private void lvNav_MouseUp(object sender, MouseButtonEventArgs e)
        {

            if (sender == lvNPKalc)
            {
                MainFrame.Navigate(new Calculator.NPKalcPage());

            }
            else if (sender == lvHistory)

                MainFrame.Navigate(new History.HistoryPage());

        }

        private void lvSubNav_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender == lvLogout)
            {

                Window login = new Login();
                this.Close();
                login.Show();
            }

        }
    }
}
