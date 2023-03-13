using NPKalc_v3.Controller;
using NPKalc_v3.Model;
using NPKalc_v3.Views.Register;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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

namespace NPKalc_v3.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        UsersController uCtrl = new UsersController();
        public Login()
        {
            InitializeComponent();
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

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            login();
        }

        public void login()
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Password))
            {
                MessageBox.Show("Please input username and password", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DataTable dtUser = uCtrl.GetUser(txtUsername.Text);
            string hashedPassword = checkPassword(txtPassword.Password);

            if (dtUser.Rows.Count > 0)
            {
                if (dtUser.Rows[0]["UserPassword"].ToString() == hashedPassword)
                {
                    Globals.UserID = Convert.ToInt32(dtUser.Rows[0]["UserID"]);
                    Globals.FullName = dtUser.Rows[0]["FullName"].ToString();
                    Globals.UserName = dtUser.Rows[0]["UserName"].ToString();
                    Globals.UserPassword = dtUser.Rows[0]["UserPassword"].ToString();
                    Window md = new Views.MainDashboard.MainDashboard();
                    this.Close();
                    md.Show();

                }
                else
                {
                    MessageBox.Show("Password did not match.", "Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Username not found. Please register an account.", "Username Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string checkPassword(string password)
        {
            string hashedString = "";
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(inputBytes);
                hashedString = BitConverter.ToString(hashedBytes).Replace("-", "");
                //Console.WriteLine(hashedString);
            }
            return hashedString;
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.Key == Key.Enter)
                {
                    login();
                }
            }
            catch { }
        }

        private void btnNewRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow rw = new RegisterWindow();
            rw.ShowDialog();
        }
    }
}
