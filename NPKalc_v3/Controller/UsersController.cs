using NPKalc_v3.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NPKalc_v3.Controller
{
    public class UsersController : UsersModel
    {
        public DataTable GetUser(string username)
        {
            try
            {
                cmd = new SqlCommand("usp_GetUser");
                cmd.Parameters.AddWithValue("@username", username);
                return ExecuteReader(cmd);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
                return null;
            }
        }
        public bool UserInsert(UsersController uCtrl)
        {
            try
            {
                cmd = new SqlCommand("usp_UserInsert");
                cmd.Parameters.AddWithValue("@FullName", uCtrl.FullName);
                cmd.Parameters.AddWithValue("@Username", uCtrl.Username);
                cmd.Parameters.AddWithValue("@UserPassword", uCtrl.UserPassword);
                return ExecNonQuery(cmd);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
                return false;
            }
        }
    }
}
