using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPKalc_v3.Model
{
    public class CalculateModel : DatabaseConnection
    {

        public static SqlCommand cmd;
        public int CalculateID { get; set; }
        public DataTable dtFertilizers { get; set; }
        public int LandArea { get; set; }
        public int N { get; set; }
        public int P { get; set; }
        public int K { get; set; }
    }
}
