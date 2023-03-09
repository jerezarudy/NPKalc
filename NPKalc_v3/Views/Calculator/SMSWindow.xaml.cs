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

namespace NPKalc_v3.Views.Calculator
{
    /// <summary>
    /// Interaction logic for SMSWindow.xaml
    /// </summary>
    public partial class SMSWindow : Window
    {
        public SMSWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtMessage.Text = "City: \nBarangay:\nName of Farmer: \nLand Area: \nSoil Type:\nSeason:\nN:\nP:\nK:\nFertilizers:\n\t10 bags of ....\n\t10 bags of ....\n\t1 bag of ....\nFor 100 % Yield:\n\t7 bags of ....\n\t4 bags of ....\n\t1 bag of ....\nFor Projected yield:\n\t7 bags of ....\n\t4 bags of ....\n\t1 bag of ....";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
