using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPKalc_v3.Model
{
    public class FertilizersModel : DatabaseConnection
    {

        public static SqlCommand cmd;
        public int FertilizerID { get; set; }
        public string FertilizerName { get; set; }
        public int Fertilizer_N { get; set; }
        public int Fertilizer_P { get; set; }
        public int Fertilizer_K { get; set; }
        public bool isActive { get; set; }
    }
}
