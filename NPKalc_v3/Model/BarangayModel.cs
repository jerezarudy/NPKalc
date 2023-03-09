using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPKalc_v3.Model
{
    public class BarangayModel : DatabaseConnection
    {

        public static SqlCommand cmd;
        public int BarangayID { get; set; }
        public int TownCityID { get; set; }
        public string BarangayName { get; set; }
        public bool isActive { get; set; }
    }
}
