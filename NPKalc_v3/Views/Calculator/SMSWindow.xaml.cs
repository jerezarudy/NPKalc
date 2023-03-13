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
using System.Net;
using System.IO;
using NPKalc_v3.Controller;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace NPKalc_v3.Views.Calculator
{
    /// <summary>
    /// Interaction logic for SMSWindow.xaml
    /// </summary>
    public partial class SMSWindow : Window
    {
        CalculateController cCtrl = new CalculateController();
        public SMSWindow(CalculateController cCtrl)
        {
            InitializeComponent();
            this.cCtrl = cCtrl;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string fertilizers = "";
            string fertilizers100 = "";
            string fertilizersProjected = "";
            foreach (DataRow drf in cCtrl.dtFertilizers.Rows)
            {
                fertilizers += "\n     " + drf["FertilizerName"].ToString() + " - " + drf["NoOfBags"].ToString() + "bag/s";
            }
            foreach (DataRow dr100 in cCtrl.dtFor100Yield.Rows)
            {
                fertilizers100 += "\n     " + dr100["FertilizerName"].ToString() + " - " + dr100["NoOfBags"].ToString() + " kgs";
            }
            foreach (DataRow drProjected in cCtrl.dtForProjectedYield.Rows)
            {
                fertilizersProjected += "\n     " + drProjected["FertilizerName"].ToString() + " - " + drProjected["ProjectedAmount"].ToString() + " kgs";
            }

            txtMessage.Text =
                "Name of Farmer - "
                + cCtrl.NameOfFarmer
                +"\nCity - "
                + cCtrl.TownCity
                + "\nBarangay - "
                + cCtrl.Barangay
                + "\nLand Area - " + cCtrl.LandArea + " ha"
                + "\n \nSoil Type - "
                + cCtrl.SoilType
                + "\nSeason - "
                + cCtrl.Season
                +"\n \nRecommended NPK Ratio " +
                "\n    (" + cCtrl.N + " - " + cCtrl.P + " - " + cCtrl.K + ")"
                + "\n\nAvailable Fertilizers "
                + fertilizers
                + "\n\nAmount (kgs) For 100% Yield"
                + fertilizers100
                + "\n\nAmount (kgs) For " + cCtrl.Total + "Projected yield"
                + fertilizersProjected;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPhoneNumber.Text))
            {
                MessageBox.Show("Please enter Phone Number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

            //// TUP
            string apiUrl = "https://nexmo-v1-jerezarudy22-gmailcom.vercel.app/nexmo/sendsms/";
            string apiKey = "9565f6bb";
            string apiSecret = "WXvx7MtRVVnMS7JA";
            //// TUP

            // Test
            //string apiUrl = "http://127.0.0.1:3000/nexmo/sendsms/";
            //string apiKey = "ccf0903b";
            //string apiSecret = "toMFfyzLaguF53I2";
            // Test
            string to = txtPhoneNumber.Text;
            string message = txtMessage.Text;

            //string postData = "{\"apiKey\":\"" + apiKey + "\", \"apiSecret\":\"" + apiSecret + "\", \"to\":\"" + to + "\", \"message\":\"" + message + "\"}";
            //JObject postData = new
            //{
            //    apiKey = apiKey,
            //    apiSecret = apiSecret,
            //    to = to,
            //    message = message
            //}; 
            JObject postData = new JObject(
             new JProperty("apiKey", apiKey),
             new JProperty("apiSecret", apiSecret),
             new JProperty("to", to),
             new JProperty("message", message)
 );

            string responseString = PostRequest(apiUrl, postData);
            if (responseString == "Message sent successfully.")
            {
                MessageBox.Show("Message sent.", "Success");
                this.DialogResult = true;
            }

        }
        public string PostRequest(string apiUrl, string postData)
        {
            string responseString = null;

            try
            {
                // Create the request object
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 60000;

                // Convert the post data to a byte array
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                // Set the content length of the request
                request.ContentLength = byteArray.Length;

                // Write the post data to the request stream
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                // Get the response from the server
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Read the response string
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    responseString = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine(ex.Message);
            }

            return responseString;
        }

        public static string PostRequest(string apiUrl, JObject postData)
        {
            string responseString = null;

            try
            {
                // Create the request object
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = 60000;

                // Serialize the post data to a JSON string
                string json = JsonConvert.SerializeObject(postData);

                // Convert the JSON string to a byte array
                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(json);

                // Set the content length of the request
                request.ContentLength = byteArray.Length;

                // Write the post data to the request stream
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                // Get the response from the server
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // Read the response string
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    responseString = streamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine(ex.Message);
            }

            return responseString;
        }
    }
}
