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
    public class TownCityController : TownCityModel
    {

        public DataTable GetAllTownCity(bool isActive = true)
        {
            try
            {
                cmd = new SqlCommand("usp_GetAllTownCity");
                cmd.Parameters.AddWithValue("@isActive", isActive);
                return ExecuteReader(cmd);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
                return null;
            }
        }
    }
}
