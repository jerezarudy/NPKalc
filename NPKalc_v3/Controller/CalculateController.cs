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
    public class CalculateController : CalculateModel
    {

        public Tuple<DataTable, DataTable> CalculateNPK(CalculateModel cModel, int calculationType = 1, int N_Counter = 0, int P_Counter = 0, int K_Counter = 0)
         {
            DataTable dt1;
            DataTable dt2;

            try
            {
                cmd = new SqlCommand("usp_CalculateNPK_100YieldV2");
                cmd.Parameters.AddWithValue("@dtFertilizers", cModel.dtFertilizers);
                cmd.Parameters.AddWithValue("@LandArea", cModel.LandArea);
                cmd.Parameters.AddWithValue("@N", cModel.N);
                cmd.Parameters.AddWithValue("@P", cModel.P);
                cmd.Parameters.AddWithValue("@K", cModel.K);
                cmd.Parameters.AddWithValue("@calculationType", calculationType);
                cmd.Parameters.AddWithValue("@N_Counter", N_Counter);
                cmd.Parameters.AddWithValue("@P_Counter", P_Counter);
                cmd.Parameters.AddWithValue("@K_Counter", K_Counter);
                dt1 = ExecuteReaderV2(cmd);


                cmd = new SqlCommand("usp_CalculateNPK_ProjectedYield");
                cmd.Parameters.AddWithValue("@dtFertilizers", cModel.dtFertilizers);
                cmd.Parameters.AddWithValue("@LandArea", cModel.LandArea);
                cmd.Parameters.AddWithValue("@N", cModel.N);
                cmd.Parameters.AddWithValue("@P", cModel.P);
                cmd.Parameters.AddWithValue("@K", cModel.K);
                cmd.Parameters.AddWithValue("@calculationType", calculationType);
                cmd.Parameters.AddWithValue("@N_Counter", N_Counter);
                cmd.Parameters.AddWithValue("@P_Counter", P_Counter);
                cmd.Parameters.AddWithValue("@K_Counter", K_Counter);
                dt2 = ExecuteReaderV2(cmd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                //MessageBox.Show(ex.Message.ToString());
                return new Tuple<DataTable, DataTable>(new DataTable(), new DataTable());
            }

            //try
            //{
            //    cmd = new SqlCommand("usp_CalculateNPK_ProjectedYield");
            //    cmd.Parameters.AddWithValue("@dtFertilizers", cModel.dtFertilizers);
            //    cmd.Parameters.AddWithValue("@LandArea", cModel.LandArea);
            //    cmd.Parameters.AddWithValue("@N", cModel.N);
            //    cmd.Parameters.AddWithValue("@P", cModel.P);
            //    cmd.Parameters.AddWithValue("@K", cModel.K);
            //    cmd.Parameters.AddWithValue("@calculationType", calculationType);
            //    cmd.Parameters.AddWithValue("@N_Counter", N_Counter);
            //    cmd.Parameters.AddWithValue("@P_Counter", P_Counter);
            //    cmd.Parameters.AddWithValue("@K_Counter", K_Counter);
            //    dt2 = ExecuteReader(cmd);
            //}
            //catch (Exception err)
            //{
            //    MessageBox.Show(err.ToString());
            //    return null;
            //}

            return new Tuple<DataTable, DataTable>(dt1, dt2);
        }
    }
}
