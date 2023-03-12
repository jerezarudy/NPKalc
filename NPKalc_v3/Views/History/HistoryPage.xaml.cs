using NPKalc_v3.Controller;
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

namespace NPKalc_v3.Views.History
{
    /// <summary>
    /// Interaction logic for HistoryPage.xaml
    /// </summary>
    public partial class HistoryPage : Page
    {
        CalculateController cCtrl = new CalculateController();
        public HistoryPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dgHistory.ItemsSource = cCtrl.GetAllCalculations().DefaultView;
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            HistoryViewWindow hvw = new HistoryViewWindow();
            hvw.ShowDialog();
        }
    }
}
