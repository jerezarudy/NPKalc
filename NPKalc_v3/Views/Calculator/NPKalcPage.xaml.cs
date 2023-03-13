using NPKalc_v3.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

namespace NPKalc_v3.Views.Calculator
{
    /// <summary>
    /// Interaction logic for NPKalcPage.xaml
    /// </summary>
    public partial class NPKalcPage : Page
    {
        TownCityController tcCtrl = new TownCityController();
        BarangayController bCtrl = new BarangayController();
        SoilTypesController stCtrl = new SoilTypesController();
        SeasonsController sCtrl = new SeasonsController();
        CalculateController cCtrl = new CalculateController();
        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        DataTable dtFirtilizers = new DataTable();
        DataTable dt100Yield = new DataTable();
        DataTable dtProjectedYield = new DataTable();

        int Nitrogen = 0;
        int Phosphorous = 0;
        int Potassium = 0;
        public NPKalcPage()
        {
            InitializeComponent();
        }

        public enum calculationType
        {
            single = 1,
            incomplete = 2,
            complete = 3
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeFields();
        }

        private void InitializeFields()
        {
            dtFirtilizers.Columns.Add("FertilizerID", typeof(int));
            dtFirtilizers.Columns.Add("NoOfBags", typeof(decimal));
            dtFirtilizers.Columns.Add("FertilizerName");
            dtFirtilizers.Columns.Add("Fertilizer_N");
            dtFirtilizers.Columns.Add("Fertilizer_P");
            dtFirtilizers.Columns.Add("Fertilizer_K");
            dtFirtilizers.Columns.Add("cboDisplay");
            dtFirtilizers.Columns.Add("npkRatio");
            dtFirtilizers.Columns.Add("isActive", typeof(bool));
            dtFirtilizers.Columns.Add("FertilizerCategory", typeof(int));

            //dtFirtilizers.Rows.Add(1,1,"test","1","2","3","4","5",true);    // For Testing
            //dgFertilizer.ItemsSource = dtFirtilizers.DefaultView;           // For Testing

            cboCity.ItemsSource = tcCtrl.GetAllTownCity().DefaultView;
            cboCity.SelectedValuePath = "TownCityID";
            cboCity.DisplayMemberPath = "TownCityName";

            cboSoilType.ItemsSource = stCtrl.GetAllSoilTypes().DefaultView;
            cboSoilType.SelectedValuePath = "SoilTypeID";
            cboSoilType.DisplayMemberPath = "Description";
            cboSoilType.SelectedIndex = 0;

            cboSeason.ItemsSource = sCtrl.GetAllSeasons().DefaultView;
            cboSeason.SelectedValuePath = "SeasonID";
            cboSeason.DisplayMemberPath = "Description";
            cboSeason.SelectedIndex = 0;

            rbN1.IsChecked = true;
            rbP1.IsChecked = true;
            rbK1.IsChecked = true;
        }

        private void cboCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            int TownCityID = Convert.ToInt32(cboCity.SelectedValue);

            cboBarangay.ItemsSource = bCtrl.GetBarangay(TownCityID).DefaultView;
            cboBarangay.SelectedValuePath = "BarangayID";
            cboBarangay.DisplayMemberPath = "BarangayName";
            cboBarangay.SelectedIndex = 0;
        }

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void txtLandArea_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void txtLandArea_Pasting(object sender, DataObjectPastingEventArgs e)
        {

            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void rbN_Checked(object sender, RoutedEventArgs e)
        {
            if (Convert.ToBoolean(rbN1.IsChecked))
            {
                Nitrogen = 1;
            }
            else if (Convert.ToBoolean(rbN2.IsChecked))
            {
                Nitrogen = 2;
            }
            else if (Convert.ToBoolean(rbN3.IsChecked))
            {
                Nitrogen = 3;
            }
            else if (Convert.ToBoolean(rbN4.IsChecked))
            {
                Nitrogen = 4;
            }
            else if (Convert.ToBoolean(rbN5.IsChecked))
            {
                Nitrogen = 5;
            }

            CalculateN();
            CalculateP();
            CalculateK();
        }


        private void rbP_Checked(object sender, RoutedEventArgs e)
        {
            if (Convert.ToBoolean(rbP1.IsChecked))
            {
                Phosphorous = 1;
            }
            else if (Convert.ToBoolean(rbP2.IsChecked))
            {
                Phosphorous = 2;
            }
            else if (Convert.ToBoolean(rbP3.IsChecked))
            {
                Phosphorous = 3;
            }
            else if (Convert.ToBoolean(rbP4.IsChecked))
            {
                Phosphorous = 4;
            }
            else if (Convert.ToBoolean(rbP5.IsChecked))
            {
                Phosphorous = 5;
            }

            CalculateN();
            CalculateP();
            CalculateK();

        }
        private void rbK_Checked(object sender, RoutedEventArgs e)
        {
            if (Convert.ToBoolean(rbK1.IsChecked))
            {
                Potassium = 1;
            }
            else if (Convert.ToBoolean(rbK2.IsChecked))
            {
                Potassium = 2;
            }
            else if (Convert.ToBoolean(rbK3.IsChecked))
            {
                Potassium = 3;
            }
            else if (Convert.ToBoolean(rbK4.IsChecked))
            {
                Potassium = 4;
            }
            else if (Convert.ToBoolean(rbK5.IsChecked))
            {
                Potassium = 5;
            }

            CalculateN();
            CalculateP();
            CalculateK();

        }

        private void cboSoilType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateN();
            CalculateP();
            CalculateK();
        }
        private void cboSeason_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalculateN();
            CalculateP();
            CalculateK();
        }

        private void CalculateN()
        {
            #region
            int N = 0;
            int soiltypeID = Convert.ToInt32(cboSoilType.SelectedValue);
            int seasonID = Convert.ToInt32(cboSeason.SelectedValue);

            /// soil 1 season 1
            if (soiltypeID == 1 && seasonID == 1 && Nitrogen == 1)
            {
                N = 100;
            }
            else if (soiltypeID == 1 && seasonID == 1 && Nitrogen == 2)
            {
                N = 80;
            }
            else if (soiltypeID == 1 && seasonID == 1 && Nitrogen == 3)
            {
                N = 60;
            }
            else if (soiltypeID == 1 && seasonID == 1 && Nitrogen == 4)
            {
                N = 40;
            }
            else if (soiltypeID == 1 && seasonID == 1 && Nitrogen == 5)
            {
                N = 23;
            }
            /// soil 2 season 1
            else if (soiltypeID == 2 && seasonID == 1 && Nitrogen == 1)
            {
                N = 120;
            }
            else if (soiltypeID == 2 && seasonID == 1 && Nitrogen == 2)
            {
                N = 100;
            }
            else if (soiltypeID == 2 && seasonID == 1 && Nitrogen == 3)
            {
                N = 80;
            }
            else if (soiltypeID == 2 && seasonID == 1 && Nitrogen == 4)
            {
                N = 60;
            }
            else if (soiltypeID == 2 && seasonID == 1 && Nitrogen == 5)
            {
                N = 23;
            }
            /// soil 3 season 1
            else if (soiltypeID == 3 && seasonID == 1 && Nitrogen == 1)
            {
                N = 45;
            }
            else if (soiltypeID == 3 && seasonID == 1 && Nitrogen == 2)
            {
                N = 40;
            }
            else if (soiltypeID == 3 && seasonID == 1 && Nitrogen == 3)
            {
                N = 35;
            }
            else if (soiltypeID == 3 && seasonID == 1 && Nitrogen == 4)
            {
                N = 30;
            }
            else if (soiltypeID == 3 && seasonID == 1 && Nitrogen == 5)
            {
                N = 0;
            }
            /// soil 4 season 1
            else if (soiltypeID == 4 && seasonID == 1 && Nitrogen == 1)
            {
                N = 90;
            }
            else if (soiltypeID == 4 && seasonID == 1 && Nitrogen == 2)
            {
                N = 70;
            }
            else if (soiltypeID == 4 && seasonID == 1 && Nitrogen == 3)
            {
                N = 50;
            }
            else if (soiltypeID == 4 && seasonID == 1 && Nitrogen == 4)
            {
                N = 30;
            }
            else if (soiltypeID == 4 && seasonID == 1 && Nitrogen == 5)
            {
                N = 23;
            }
            /// soil 5 season 1
            else if (soiltypeID == 5 && seasonID == 1 && Nitrogen == 1)
            {
                N = 110;
            }
            else if (soiltypeID == 5 && seasonID == 1 && Nitrogen == 2)
            {
                N = 90;
            }
            else if (soiltypeID == 5 && seasonID == 1 && Nitrogen == 3)
            {
                N = 70;
            }
            else if (soiltypeID == 5 && seasonID == 1 && Nitrogen == 4)
            {
                N = 50;
            }
            else if (soiltypeID == 5 && seasonID == 1 && Nitrogen == 5)
            {
                N = 23;
            }
            /// soil 6 season 1
            else if (soiltypeID == 6 && seasonID == 1 && Nitrogen == 1)
            {
                N = 45;
            }
            else if (soiltypeID == 6 && seasonID == 1 && Nitrogen == 2)
            {
                N = 40;
            }
            else if (soiltypeID == 6 && seasonID == 1 && Nitrogen == 3)
            {
                N = 35;
            }
            else if (soiltypeID == 6 && seasonID == 1 && Nitrogen == 4)
            {
                N = 30;
            }
            else if (soiltypeID == 6 && seasonID == 1 && Nitrogen == 5)
            {
                N = 0;
            }
            //////

            /// soil 1 season 2
            if (soiltypeID == 1 && seasonID == 2 && Nitrogen == 1)
            {
                N = 120;
            }
            else if (soiltypeID == 1 && seasonID == 2 && Nitrogen == 2)
            {
                N = 100;
            }
            else if (soiltypeID == 1 && seasonID == 2 && Nitrogen == 3)
            {
                N = 80;
            }
            else if (soiltypeID == 1 && seasonID == 2 && Nitrogen == 4)
            {
                N = 60;
            }
            else if (soiltypeID == 1 && seasonID == 2 && Nitrogen == 5)
            {
                N = 0;
            }
            /// soil 2 season 2
            else if (soiltypeID == 2 && seasonID == 2 && Nitrogen == 1)
            {
                N = 140;
            }
            else if (soiltypeID == 2 && seasonID == 2 && Nitrogen == 2)
            {
                N = 120;
            }
            else if (soiltypeID == 2 && seasonID == 2 && Nitrogen == 3)
            {
                N = 100;
            }
            else if (soiltypeID == 2 && seasonID == 2 && Nitrogen == 4)
            {
                N = 80;
            }
            else if (soiltypeID == 2 && seasonID == 2 && Nitrogen == 5)
            {
                N = 0;
            }
            /// soil 3 season 2
            else if (soiltypeID == 3 && seasonID == 2 && Nitrogen == 1)
            {
                N = 45;
            }
            else if (soiltypeID == 3 && seasonID == 2 && Nitrogen == 2)
            {
                N = 40;
            }
            else if (soiltypeID == 3 && seasonID == 2 && Nitrogen == 3)
            {
                N = 35;
            }
            else if (soiltypeID == 3 && seasonID == 2 && Nitrogen == 4)
            {
                N = 30;
            }
            else if (soiltypeID == 3 && seasonID == 2 && Nitrogen == 5)
            {
                N = 0;
            }
            /// soil 4 season 1
            else if (soiltypeID == 4 && seasonID == 2 && Nitrogen == 1)
            {
                N = 100;
            }
            else if (soiltypeID == 4 && seasonID == 2 && Nitrogen == 2)
            {
                N = 80;
            }
            else if (soiltypeID == 4 && seasonID == 2 && Nitrogen == 3)
            {
                N = 60;
            }
            else if (soiltypeID == 4 && seasonID == 2 && Nitrogen == 4)
            {
                N = 40;
            }
            else if (soiltypeID == 4 && seasonID == 2 && Nitrogen == 5)
            {
                N = 0;
            }
            /// soil 5 season 2
            else if (soiltypeID == 5 && seasonID == 2 && Nitrogen == 1)
            {
                N = 120;
            }
            else if (soiltypeID == 5 && seasonID == 2 && Nitrogen == 2)
            {
                N = 100;
            }
            else if (soiltypeID == 5 && seasonID == 2 && Nitrogen == 3)
            {
                N = 80;
            }
            else if (soiltypeID == 5 && seasonID == 2 && Nitrogen == 4)
            {
                N = 60;
            }
            else if (soiltypeID == 5 && seasonID == 2 && Nitrogen == 5)
            {
                N = 0;
            }
            /// soil 6 season 2
            else if (soiltypeID == 6 && seasonID == 2 && Nitrogen == 1)
            {
                N = 45;
            }
            else if (soiltypeID == 6 && seasonID == 2 && Nitrogen == 2)
            {
                N = 40;
            }
            else if (soiltypeID == 6 && seasonID == 2 && Nitrogen == 3)
            {
                N = 35;
            }
            else if (soiltypeID == 6 && seasonID == 2 && Nitrogen == 4)
            {
                N = 30;
            }
            else if (soiltypeID == 6 && seasonID == 2 && Nitrogen == 5)
            {
                N = 0;
            }

            tbN.Text = N.ToString();
            #endregion
        }

        private void CalculateP()
        {
            #region
            int P = 0;
            int soiltypeID = Convert.ToInt32(cboSoilType.SelectedValue);
            int seasonID = Convert.ToInt32(cboSeason.SelectedValue);

            /// soil 1 season 1
            if (soiltypeID == 1 && seasonID == 1 && Phosphorous == 1)
            {
                P = 60;
            }
            else if (soiltypeID == 1 && seasonID == 1 && Phosphorous == 2)
            {
                P = 40;
            }
            else if (soiltypeID == 1 && seasonID == 1 && Phosphorous == 3)
            {
                P = 20;
            }
            else if (soiltypeID == 1 && seasonID == 1 && Phosphorous == 4)
            {
                P = 7;
            }
            else if (soiltypeID == 1 && seasonID == 1 && Phosphorous == 5)
            {
                P = 0;
            }
            /// soil 2 season 1
            else if (soiltypeID == 2 && seasonID == 1 && Phosphorous == 1)
            {
                P = 70;
            }
            else if (soiltypeID == 2 && seasonID == 1 && Phosphorous == 2)
            {
                P = 50;
            }
            else if (soiltypeID == 2 && seasonID == 1 && Phosphorous == 3)
            {
                P = 30;
            }
            else if (soiltypeID == 2 && seasonID == 1 && Phosphorous == 4)
            {
                P = 7;
            }
            else if (soiltypeID == 2 && seasonID == 1 && Phosphorous == 5)
            {
                P = 0;
            }
            /// soil 3 season 1
            else if (soiltypeID == 3 && seasonID == 1 && Phosphorous == 1)
            {
                P = 60;
            }
            else if (soiltypeID == 3 && seasonID == 1 && Phosphorous == 2)
            {
                P = 40;
            }
            else if (soiltypeID == 3 && seasonID == 1 && Phosphorous == 3)
            {
                P = 30;
            }
            else if (soiltypeID == 3 && seasonID == 1 && Phosphorous == 4)
            {
                P = 20;
            }
            else if (soiltypeID == 3 && seasonID == 1 && Phosphorous == 5)
            {
                P = 0;
            }
            /// soil 4 season 1
            else if (soiltypeID == 4 && seasonID == 1 && Phosphorous == 1)
            {
                P = 60;
            }
            else if (soiltypeID == 4 && seasonID == 1 && Phosphorous == 2)
            {
                P = 40;
            }
            else if (soiltypeID == 4 && seasonID == 1 && Phosphorous == 3)
            {
                P = 20;
            }
            else if (soiltypeID == 4 && seasonID == 1 && Phosphorous == 4)
            {
                P = 7;
            }
            else if (soiltypeID == 4 && seasonID == 1 && Phosphorous == 5)
            {
                P = 0;
            }
            /// soil 5 season 1
            else if (soiltypeID == 5 && seasonID == 1 && Phosphorous == 1)
            {
                P = 70;
            }
            else if (soiltypeID == 5 && seasonID == 1 && Phosphorous == 2)
            {
                P = 50;
            }
            else if (soiltypeID == 5 && seasonID == 1 && Phosphorous == 3)
            {
                P = 30;
            }
            else if (soiltypeID == 5 && seasonID == 1 && Phosphorous == 4)
            {
                P = 7;
            }
            else if (soiltypeID == 5 && seasonID == 1 && Phosphorous == 5)
            {
                P = 0;
            }
            /// soil 6 season 1
            else if (soiltypeID == 6 && seasonID == 1 && Phosphorous == 1)
            {
                P = 60;
            }
            else if (soiltypeID == 6 && seasonID == 1 && Phosphorous == 2)
            {
                P = 40;
            }
            else if (soiltypeID == 6 && seasonID == 1 && Phosphorous == 3)
            {
                P = 30;
            }
            else if (soiltypeID == 6 && seasonID == 1 && Phosphorous == 4)
            {
                P = 20;
            }
            else if (soiltypeID == 6 && seasonID == 1 && Phosphorous == 5)
            {
                P = 0;
            }
            //////

            /// soil 1 season 2
            if (soiltypeID == 1 && seasonID == 2 && Phosphorous == 1)
            {
                P = 60;
            }
            else if (soiltypeID == 1 && seasonID == 2 && Phosphorous == 2)
            {
                P = 40;
            }
            else if (soiltypeID == 1 && seasonID == 2 && Phosphorous == 3)
            {
                P = 20;
            }
            else if (soiltypeID == 1 && seasonID == 2 && Phosphorous == 4)
            {
                P = 7;
            }
            else if (soiltypeID == 1 && seasonID == 2 && Phosphorous == 5)
            {
                P = 0;
            }
            /// soil 2 season 2
            else if (soiltypeID == 2 && seasonID == 2 && Phosphorous == 1)
            {
                P = 70;
            }
            else if (soiltypeID == 2 && seasonID == 2 && Phosphorous == 2)
            {
                P = 50;
            }
            else if (soiltypeID == 2 && seasonID == 2 && Phosphorous == 3)
            {
                P = 30;
            }
            else if (soiltypeID == 2 && seasonID == 2 && Phosphorous == 4)
            {
                P = 7;
            }
            else if (soiltypeID == 2 && seasonID == 2 && Phosphorous == 5)
            {
                P = 0;
            }
            /// soil 3 season 2
            else if (soiltypeID == 3 && seasonID == 2 && Phosphorous == 1)
            {
                P = 60;
            }
            else if (soiltypeID == 3 && seasonID == 2 && Phosphorous == 2)
            {
                P = 40;
            }
            else if (soiltypeID == 3 && seasonID == 2 && Phosphorous == 3)
            {
                P = 30;
            }
            else if (soiltypeID == 3 && seasonID == 2 && Phosphorous == 4)
            {
                P = 20;
            }
            else if (soiltypeID == 3 && seasonID == 2 && Phosphorous == 5)
            {
                P = 0;
            }
            /// soil 4 season 2
            else if (soiltypeID == 4 && seasonID == 2 && Phosphorous == 1)
            {
                P = 60;
            }
            else if (soiltypeID == 4 && seasonID == 2 && Phosphorous == 2)
            {
                P = 40;
            }
            else if (soiltypeID == 4 && seasonID == 2 && Phosphorous == 3)
            {
                P = 20;
            }
            else if (soiltypeID == 4 && seasonID == 2 && Phosphorous == 4)
            {
                P = 7;
            }
            else if (soiltypeID == 4 && seasonID == 2 && Phosphorous == 5)
            {
                P = 0;
            }
            /// soil 5 season 2
            else if (soiltypeID == 5 && seasonID == 2 && Phosphorous == 1)
            {
                P = 70;
            }
            else if (soiltypeID == 5 && seasonID == 2 && Phosphorous == 2)
            {
                P = 50;
            }
            else if (soiltypeID == 5 && seasonID == 2 && Phosphorous == 3)
            {
                P = 30;
            }
            else if (soiltypeID == 5 && seasonID == 2 && Phosphorous == 4)
            {
                P = 7;
            }
            else if (soiltypeID == 5 && seasonID == 2 && Phosphorous == 5)
            {
                P = 0;
            }
            /// soil 6 season 2
            else if (soiltypeID == 6 && seasonID == 2 && Phosphorous == 1)
            {
                P = 60;
            }
            else if (soiltypeID == 6 && seasonID == 2 && Phosphorous == 2)
            {
                P = 40;
            }
            else if (soiltypeID == 6 && seasonID == 2 && Phosphorous == 3)
            {
                P = 30;
            }
            else if (soiltypeID == 6 && seasonID == 2 && Phosphorous == 4)
            {
                P = 20;
            }
            else if (soiltypeID == 6 && seasonID == 2 && Phosphorous == 5)
            {
                P = 0;
            }

            tbP.Text = P.ToString();
            #endregion
        }

        private void CalculateK()
        {
            #region
            int K = 0;
            int soiltypeID = Convert.ToInt32(cboSoilType.SelectedValue);
            int seasonID = Convert.ToInt32(cboSeason.SelectedValue);

            /// soil 1 season 1
            if (soiltypeID == 1 && Potassium == 1)
            {
                K = 60;
            }
            else if (soiltypeID == 1 && Potassium == 2)
            {
                K = 40;
            }
            else if (soiltypeID == 1 && Potassium == 3)
            {
                K = 20;
            }
            else if (soiltypeID == 1 && Potassium == 4)
            {
                K = 7;
            }
            else if (soiltypeID == 1 && Potassium == 5)
            {
                K = 0;
            }
            /// soil 2 season 1
            else if (soiltypeID == 2 && Potassium == 1)
            {
                K = 70;
            }
            else if (soiltypeID == 2 && Potassium == 2)
            {
                K = 50;
            }
            else if (soiltypeID == 2 && Potassium == 3)
            {
                K = 30;
            }
            else if (soiltypeID == 2 && Potassium == 4)
            {
                K = 7;
            }
            else if (soiltypeID == 2 && Potassium == 5)
            {
                K = 0;
            }
            /// soil 3 season 1
            else if (soiltypeID == 3 && Potassium == 1)
            {
                K = 60;
            }
            else if (soiltypeID == 3 && Potassium == 2)
            {
                K = 45;
            }
            else if (soiltypeID == 3 && Potassium == 3)
            {
                K = 30;
            }
            else if (soiltypeID == 3 && Potassium == 4)
            {
                K = 0;
            }
            else if (soiltypeID == 3 && Potassium == 5)
            {
                K = 0;
            }
            /// soil 4 season 1
            else if (soiltypeID == 4 && Potassium == 1)
            {
                K = 60;
            }
            else if (soiltypeID == 4 && Potassium == 2)
            {
                K = 40;
            }
            else if (soiltypeID == 4 && Potassium == 3)
            {
                K = 20;
            }
            else if (soiltypeID == 4 && Potassium == 4)
            {
                K = 7;
            }
            else if (soiltypeID == 4 && Potassium == 5)
            {
                K = 0;
            }
            /// soil 5 season 1
            else if (soiltypeID == 5 && Potassium == 1)
            {
                K = 70;
            }
            else if (soiltypeID == 5 && Potassium == 2)
            {
                K = 50;
            }
            else if (soiltypeID == 5 && Potassium == 3)
            {
                K = 30;
            }
            else if (soiltypeID == 5 && Potassium == 4)
            {
                K = 7;
            }
            else if (soiltypeID == 5 && Potassium == 5)
            {
                K = 0;
            }
            /// soil 6 season 1
            else if (soiltypeID == 6 && Potassium == 1)
            {
                K = 60;
            }
            else if (soiltypeID == 6 && Potassium == 2)
            {
                K = 45;
            }
            else if (soiltypeID == 6 && Potassium == 3)
            {
                K = 30;
            }
            else if (soiltypeID == 6 && Potassium == 4)
            {
                K = 0;
            }
            else if (soiltypeID == 6 && Potassium == 5)
            {
                K = 0;
            }
            //////


            tbK.Text = K.ToString();
            #endregion
        }

        private void btnAddFertilizer_Click(object sender, RoutedEventArgs e)
        {
            AddFiltilizerWindow afw = new AddFiltilizerWindow(dtFirtilizers);
            afw.ShowDialog();
            if (afw.DialogResult == true)
            {
                dgFertilizer.ItemsSource = dtFirtilizers.DefaultView;
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                DataRowView drvItem = dgFertilizer.SelectedItem as DataRowView;
                var query = dtFirtilizers.AsEnumerable().Where(r => r.Field<int>("FertilizerID") == Convert.ToInt32(drvItem["FertilizerID"]));

                foreach (var row in query.ToList())
                    row.Delete();

            }
            catch (Exception)
            {

            }

        }
        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            //int soiltypeID = Convert.ToInt32(cboSoilType.SelectedValue);
            //int seasonID = Convert.ToInt32(cboSeason.SelectedValue);
            int hasComplete = 0;
            int hasIncomplete = 0;
            int hasSingle = 0;
            int N_Counter = 0;
            int P_Counter = 0;
            int K_Counter = 0;
            int calculationType = 1;

            int N_Sum = 0;
            int P_Sum = 0;
            int K_Sum = 0;

            decimal N_Percentage = 0;
            decimal P_Percentage = 0;
            decimal K_Percentage = 0;

            if (string.IsNullOrEmpty(txtLandArea.Text))
            {
                MessageBox.Show("Please input No of Land Area", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dtFirtilizers.Rows.Count <= 0)
            {
                MessageBox.Show("Please add Fertilizer.", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dtFirtilizers.Rows.Count <= 1)
            {
                MessageBox.Show("Please add at least 2 Fertilizers.", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            foreach (DataRow dr in dtFirtilizers.Rows)
            {
                if (Convert.ToInt32(dr["FertilizerCategory"]) == 3)
                {
                    hasComplete = hasComplete + 1;
                }
                if (Convert.ToInt32(dr["FertilizerCategory"]) == 2)
                {
                    hasIncomplete = hasIncomplete + 1;
                }
                if (Convert.ToInt32(dr["FertilizerCategory"]) == 1)
                {
                    hasSingle = hasSingle + 1;
                }


                if (Convert.ToInt32(dr["Fertilizer_N"]) > 0)
                {
                    N_Counter = N_Counter + 1;
                }
                if (Convert.ToInt32(dr["Fertilizer_P"]) > 0)
                {
                    P_Counter = P_Counter + 1;
                }
                if (Convert.ToInt32(dr["Fertilizer_K"]) > 0)
                {
                    K_Counter = K_Counter + 1;
                }

                N_Sum = N_Sum + Convert.ToInt32(dr["Fertilizer_N"]);
                P_Sum = P_Sum + Convert.ToInt32(dr["Fertilizer_P"]);
                K_Sum = K_Sum + Convert.ToInt32(dr["Fertilizer_K"]);

            }

            if (hasComplete > 1 || hasIncomplete > 1 || (hasComplete == 1 & hasIncomplete == 1))
            {
                MessageBox.Show("Invalid combination of fertilizer. (Combinations of: 3 single fertilizers, incomplete and single fertilizers, and complete and single fertilizers only)", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (hasComplete > 0)
            {
                calculationType = 3;
            }
            if (hasIncomplete > 0)
            {
                calculationType = 2;
            }

            cCtrl.dtFertilizers = dtFirtilizers;
            cCtrl.LandArea = Convert.ToInt32(txtLandArea.Text);
            cCtrl.N = Convert.ToInt32(tbN.Text);
            cCtrl.P = Convert.ToInt32(tbP.Text);
            cCtrl.K = Convert.ToInt32(tbK.Text);

            DataTable dtOrder = new DataTable();
            dtOrder.Columns.Add("Type");
            dtOrder.Columns.Add("Value", typeof(int));

            dtOrder.Rows.Add("N", cCtrl.N);
            dtOrder.Rows.Add("P", cCtrl.P);
            dtOrder.Rows.Add("K", cCtrl.K);

            DataView view = dtOrder.DefaultView;
            view.Sort = "Value DESC";
            DataTable sortedNPK = view.ToTable();

            DataTable dtOrderFertilizers = new DataTable();
            dtOrderFertilizers.Columns.Add("Type");
            dtOrderFertilizers.Columns.Add("Value", typeof(int));

            dtOrderFertilizers.Rows.Add("N", N_Sum);
            dtOrderFertilizers.Rows.Add("P", P_Sum);
            dtOrderFertilizers.Rows.Add("K", K_Sum);

            DataView viewFertilizers = dtOrderFertilizers.DefaultView;
            viewFertilizers.Sort = "Value DESC";
            DataTable sortedNPKFertilizers = viewFertilizers.ToTable();

            string errorMessage = "Ratio of added ferilizer's NPK must conform to the ratio of Recommended Fertilizer Rate.";

            bool hasError = false;
            //for (int i = 0; i < sortedNPK.Rows.Count; i++)
            //{
            //    if (sortedNPK.Rows[i]["Type"].ToString() != sortedNPKFertilizers.Rows[i]["Type"].ToString())
            //    {
            //        hasError = true;
            //    }

            //}


            //if (hasError)
            //{
            //    MessageBox.Show(errorMessage, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}


            Tuple<DataTable, DataTable> result;


            BackgroundWorker worker = new BackgroundWorker();
            biProgress.BusyContent = "Calculating...";
            biProgress.IsBusy = true;
            worker.DoWork += (o, ea) =>
            {
                Thread.Sleep(1200);
                result = cCtrl.CalculateNPK(cCtrl, calculationType, N_Counter, P_Counter, K_Counter);

                Dispatcher.Invoke((Action)(() => {
                    dt100Yield = result.Item1;
                    dtProjectedYield = result.Item2;

                    cCtrl.dtFor100Yield = result.Item1;
                    cCtrl.dtForProjectedYield = result.Item2;

                    dg100Yield.ItemsSource = result.Item1.DefaultView;
                    dgProjectedYield.ItemsSource = result.Item2.DefaultView;

                    txtTotal.Text = result.Item2.AsEnumerable().Sum(x => x.Field<decimal>("ProjectedPercentage")).ToString() + " %";
                    foreach (DataRow item in result.Item2.Rows)
                    {
                        N_Percentage += Convert.ToDecimal(item["N_Percentage"]);
                        P_Percentage += Convert.ToDecimal(item["P_Percentage"]);
                        K_Percentage += Convert.ToDecimal(item["K_Percentage"]);
                    }
                    btnSave.IsEnabled = true;
                }));
            };
            worker.RunWorkerCompleted += (o, ea) =>
            {
                biProgress.IsBusy = false;
            };
            worker.RunWorkerAsync();

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // populate object
            cCtrl.TownCity = cboCity.Text;
            cCtrl.Barangay = cboBarangay.Text;
            cCtrl.NameOfFarmer = txtNameOfFarmer.Text;
            cCtrl.LandArea = Convert.ToInt32(txtLandArea.Text);
            cCtrl.SoilType = cboSoilType.Text;
            cCtrl.Season = cboSeason.Text;
            cCtrl.Total = txtTotal.Text;
            cCtrl.Nitrogen = Nitrogen;
            cCtrl.Phosphorous = Phosphorous;
            cCtrl.Potassium = Potassium;

            MessageBoxResult mbr = MessageBox.Show("Are you sure you want to save calculations?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (mbr == MessageBoxResult.Yes)
            {
                if (cCtrl.CalculationInsert(cCtrl))
                {

                    MessageBox.Show("Calculation saved.", "Success", MessageBoxButton.OK);

                    SMSWindow sms = new SMSWindow(cCtrl);
                    sms.ShowDialog();

                }
                else
                {
                    MessageBox.Show("Error saving.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }

        }

        private void dgFertilizer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dg100Yield_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
