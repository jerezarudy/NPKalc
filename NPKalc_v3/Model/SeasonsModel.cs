using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPKalc_v3.Model
{
    public class SeasonsModel : DatabaseConnection
    {
        public static SqlCommand cmd;
        public int SeasonsID { get; set; }
        public string Description { get; set; }
        public bool isActive { get; set; }
    }
}
