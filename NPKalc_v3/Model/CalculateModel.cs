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
        public string TownCity { get; set; }
        public string Barangay { get; set; }
        public string NameOfFarmer { get; set; }
        public int LandArea { get; set; }
        public string SoilType { get; set; }
        public string Season { get; set; }
        public int Nitrogen { get; set; }
        public int Phosphorous { get; set; }
        public int Potassium { get; set; }
        public int N { get; set; }
        public int P { get; set; }
        public int K { get; set; }
        public string Total { get; set; }
        public DataTable dtFor100Yield { get; set; }
        public DataTable dtForProjectedYield { get; set; }
    }
}
