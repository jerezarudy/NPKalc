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
using System.Windows.Shapes;

namespace NPKalc_v3.Views.History
{
    /// <summary>
    /// Interaction logic for HistoryViewWindow.xaml
    /// </summary>
    public partial class HistoryViewWindow : Window
    {
        CalculateController cCtrl = new CalculateController();
        int calculationID = 0;
        public HistoryViewWindow(int calculationID)
        {
            InitializeComponent();
            this.calculationID = calculationID;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cCtrl = cCtrl.GetCalculation(calculationID);

            //populate fields
            cboCity.Text = cCtrl.TownCity;
            cboBarangay.Text = cCtrl.Barangay;
            txtNameOfFarmer.Text = cCtrl.NameOfFarmer;
            txtLandArea.Text = cCtrl.LandArea.ToString();
            cboSoilType.Text = cCtrl.SoilType;
            cboSeason.Text = cCtrl.Season;

            // Nitrogen
            if (cCtrl.Nitrogen == 1)
            {
                rbN1.IsChecked = true;
            }
            else if (cCtrl.Nitrogen == 2)
            {
                rbN2.IsChecked = true;
            }
            else if (cCtrl.Nitrogen == 3)
            {
                rbN3.IsChecked = true;
            }
            else if (cCtrl.Nitrogen == 4)
            {
                rbN4.IsChecked = true;
            }
            else if (cCtrl.Nitrogen == 5)
            {
                rbN5.IsChecked = true;
            }

            // Phosphorous
            if (cCtrl.Phosphorous == 1)
            {
                rbP1.IsChecked = true;
            }
            else if (cCtrl.Phosphorous == 2)
            {
                rbP2.IsChecked = true;
            }
            else if (cCtrl.Phosphorous == 3)
            {
                rbP3.IsChecked = true;
            }
            else if (cCtrl.Phosphorous == 4)
            {
                rbP4.IsChecked = true;
            }
            else if (cCtrl.Phosphorous == 5)
            {
                rbP5.IsChecked = true;
            }

            // Potassium
            if (cCtrl.Potassium == 1)
            {
                rbK1.IsChecked = true;
            }
            else if (cCtrl.Potassium == 2)
            {
                rbK2.IsChecked = true;
            }
            else if (cCtrl.Potassium == 3)
            {
                rbK3.IsChecked = true;
            }
            else if (cCtrl.Potassium == 4)
            {
                rbK4.IsChecked = true;
            }
            else if (cCtrl.Potassium == 5)
            {
                rbK5.IsChecked = true;
            }

            tbN.Text = cCtrl.N.ToString();
            tbP.Text = cCtrl.P.ToString();
            tbK.Text = cCtrl.K.ToString();

            dgFertilizer.ItemsSource = cCtrl.dtFertilizers.DefaultView;
            dg100Yield.ItemsSource = cCtrl.dtFor100Yield.DefaultView;
            dgProjectedYield.ItemsSource = cCtrl.dtForProjectedYield.DefaultView;

            txtTotal.Text = $"FOR {cCtrl.Total} PROJECTED YIELD";
        }
    }
}
