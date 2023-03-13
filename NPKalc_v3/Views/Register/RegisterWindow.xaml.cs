using NPKalc_v3.Controller;
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

namespace NPKalc_v3.Views.Register
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        UsersController uCtrl = new UsersController();
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {

            DataTable dtUser = uCtrl.GetUser(txtUsername.Text);
            string hashPassword = checkPassword(txtPassword.Password);
            string hashConfirmPassword = checkPassword(txtConfirmPassword.Password);


            if (string.IsNullOrEmpty(txtFullname.Text) || string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Password) || string.IsNullOrEmpty(txtConfirmPassword.Password))
            {
                MessageBox.Show("Please fill all fields.", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (dtUser.Rows.Count > 0)
            {
                MessageBox.Show("Username already exist.", "Invalid Username", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (hashPassword == hashConfirmPassword)
            {
                uCtrl.FullName = txtFullname.Text;
                uCtrl.Username = txtUsername.Text;
                uCtrl.UserPassword = hashPassword;

                if (uCtrl.UserInsert(uCtrl))
                {
                    MessageBox.Show("User registerd", "Success", MessageBoxButton.OK);
                    this.DialogResult = true;
                }
            }
            else
            {
                MessageBox.Show("Password did not match.", "Password Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
    }
}
