using NPKalc_v3.Controller;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AddFiltilizerWindow.xaml
    /// </summary>
    public partial class AddFiltilizerWindow : Window
    {
        FertilizersController fCtrl = new FertilizersController();
        private static readonly Regex _regex = new Regex("[^0-9.-]+");
        DataTable dtFirtilizers = new DataTable();
        DataTable dtcboFirtilizers = new DataTable();

        public AddFiltilizerWindow(DataTable dtFertilizers)
        {
            InitializeComponent();
            this.dtFirtilizers = dtFertilizers;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeCombobox();
        }

        private void InitializeCombobox()
        {
            txtNoOfBags.Text = "1"; 
            dtcboFirtilizers = fCtrl.GetAllFertilizers();
            cboFertilizer.ItemsSource = dtcboFirtilizers.DefaultView;
            cboFertilizer.SelectedValuePath = "FertilizerID";
            cboFertilizer.DisplayMemberPath = "cboDisplay";
            cboFertilizer.SelectedIndex = 0;

        }

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void txtNoOfBags_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void txtNoOfBags_Pasting(object sender, DataObjectPastingEventArgs e)
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool contains = dtFirtilizers.AsEnumerable().Any(row => Convert.ToInt32(cboFertilizer.SelectedValue) == row.Field<int>("FertilizerID"));
            if (contains)
            {
                MessageBox.Show("Fertilizer already added","WARNING",MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(txtNoOfBags.Text))
            {
                MessageBox.Show("Please input No of Bags", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (dtFirtilizers.Rows.Count >= 3)
            {
                MessageBox.Show("Maximum of 3 fertilizers only", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var results = from myRow in dtcboFirtilizers.AsEnumerable()
                          where myRow.Field<int>("FertilizerID") == Convert.ToInt32(cboFertilizer.SelectedValue)
                          select myRow;
            DataTable dtResult = results.CopyToDataTable();
            foreach (DataRow item in dtResult.Rows)
            {
                dtFirtilizers.Rows.Add(
                    Convert.ToInt32(item["FertilizerID"]),
                    Convert.ToInt32(txtNoOfBags.Text),
                    item["FertilizerName"],
                    item["Fertilizer_N"],
                    item["Fertilizer_P"],
                    item["Fertilizer_K"],
                    item["cboDisplay"],
                    item["npkRatio"],
                    Convert.ToBoolean(item["isActive"]),
                    Convert.ToInt32(item["FertilizerCategory"])
                    );

            }
            DialogResult = true;
            //dtFirtilizers.Rows.Add(results);

        }
    }
}
