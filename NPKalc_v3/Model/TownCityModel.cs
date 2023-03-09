using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPKalc_v3.Model
{
    public class TownCityModel : DatabaseConnection
    {

        public static SqlCommand cmd;
        public int TownCityID { get; set; }
        public int ProvinceID { get; set; }
        public string TownCityName { get; set; }
        public string ZIP { get; set; }
        public bool isActive { get; set; }
    }
}
