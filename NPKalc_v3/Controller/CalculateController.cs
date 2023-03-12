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
            DataTable dtCloned;

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
                dtCloned = dt2.Clone();
                dtCloned.Columns.Add("ProjectedAmount", typeof(decimal));
                dtCloned.Columns.Add("ProjectedPercentage", typeof(decimal));
                var sum1 = dt2.AsEnumerable().Sum(x => x.Field<int>("Test1Reference"));
                var sum2 = dt2.AsEnumerable().Sum(x => x.Field<int>("Test2Reference"));
                var sum3 = dt2.AsEnumerable().Sum(x => x.Field<int>("Test3Reference"));

                var sum1Kg = dt2.AsEnumerable().Sum(x => x.Field<decimal>("TEST1"));
                var sum2Kg = dt2.AsEnumerable().Sum(x => x.Field<decimal>("TEST2"));
                var sum3Kg = dt2.AsEnumerable().Sum(x => x.Field<decimal>("TEST3"));

                var sum100Kg = dt2.AsEnumerable().Sum(x => x.Field<decimal>("NoOfBags"));

                if (dt1.Rows.Count == 2)
                {
                    if (sum1 == 2)
                    {
                        foreach (DataRow item in dt2.Rows)
                        {
                            decimal FertilizerPercentage = Convert.ToDecimal(item["NoOfBags"]) / sum100Kg * 100;
                            decimal ProjectedPercentage = Convert.ToDecimal(item["NoOfBags"]) > 0 ? Convert.ToDecimal(item["TEST1"]) / Convert.ToDecimal(item["NoOfBags"]) * FertilizerPercentage : Convert.ToDecimal(0);
                            string ppString = ProjectedPercentage == 0 ? "0.00" : ProjectedPercentage.ToString("##.##");
                            dtCloned.Rows.Add(
                                Convert.ToDecimal(item["NoOfBags"]),
                                item["FertilizerName"].ToString(),
                                Convert.ToDecimal(item["N_Output"]),
                                Convert.ToDecimal(item["P_Output"]),
                                Convert.ToDecimal(item["K_Output"]),
                                Convert.ToDecimal(item["NoOfKgs"]),
                                Convert.ToDecimal(item["NoTotalOfKgs"]),
                                Convert.ToDecimal(item["N"]),
                                Convert.ToDecimal(item["P"]),
                                Convert.ToDecimal(item["K"]),
                                Convert.ToDecimal(item["N_Percentage"]),
                                Convert.ToDecimal(item["P_Percentage"]),
                                Convert.ToDecimal(item["K_Percentage"]),
                                Convert.ToDecimal(item["TEST1"]),
                                Convert.ToDecimal(item["TEST2"]),
                                Convert.ToDecimal(item["TEST3"]),
                                Convert.ToDecimal(item["Test1Reference"]),
                                Convert.ToDecimal(item["Test2Reference"]),
                                Convert.ToDecimal(item["Test3Reference"]),
                                Convert.ToDecimal(item["TEST1"]), // as ProjectedAmount
                                ppString
                                );
                        }
                    }
                    else if (sum2 == 2)
                    {
                        foreach (DataRow item in dt2.Rows)
                        {
                            decimal FertilizerPercentage = Convert.ToDecimal(item["NoOfBags"]) / sum100Kg * 100;
                            decimal ProjectedPercentage = Convert.ToDecimal(item["NoOfBags"]) > 0 ? Convert.ToDecimal(item["TEST2"]) / Convert.ToDecimal(item["NoOfBags"]) * FertilizerPercentage : Convert.ToDecimal(0);
                            string ppString = ProjectedPercentage == 0 ? "0.00" : ProjectedPercentage.ToString("##.##");
                            dtCloned.Rows.Add(
                                Convert.ToDecimal(item["NoOfBags"]),
                                item["FertilizerName"].ToString(),
                                Convert.ToDecimal(item["N_Output"]),
                                Convert.ToDecimal(item["P_Output"]),
                                Convert.ToDecimal(item["K_Output"]),
                                Convert.ToDecimal(item["NoOfKgs"]),
                                Convert.ToDecimal(item["TotalNoOfKgs"]),
                                Convert.ToDecimal(item["N"]),
                                Convert.ToDecimal(item["P"]),
                                Convert.ToDecimal(item["K"]),
                                Convert.ToDecimal(item["N_Percentage"]),
                                Convert.ToDecimal(item["P_Percentage"]),
                                Convert.ToDecimal(item["K_Percentage"]),
                                Convert.ToDecimal(item["TEST1"]),
                                Convert.ToDecimal(item["TEST2"]),
                                Convert.ToDecimal(item["TEST3"]),
                                Convert.ToDecimal(item["Test1Reference"]),
                                Convert.ToDecimal(item["Test2Reference"]),
                                Convert.ToDecimal(item["Test3Reference"]),
                                Convert.ToDecimal(item["TEST2"]), // as ProjectedAmount
                                ppString
                                );
                        }

                    }
                    else if (sum3 == 2)
                    {
                        foreach (DataRow item in dt2.Rows)
                        {
                            decimal FertilizerPercentage = Convert.ToDecimal(item["NoOfBags"]) / sum100Kg * 100;
                            decimal ProjectedPercentage = Convert.ToDecimal(item["NoOfBags"]) > 0 ? Convert.ToDecimal(item["TEST3"]) / Convert.ToDecimal(item["NoOfBags"]) * FertilizerPercentage : Convert.ToDecimal(0);
                            string ppString = ProjectedPercentage == 0 ? "0.00" : ProjectedPercentage.ToString("##.##");
                            dtCloned.Rows.Add(
                                Convert.ToDecimal(item["NoOfBags"]),
                                item["FertilizerName"].ToString(),
                                Convert.ToDecimal(item["N_Output"]),
                                Convert.ToDecimal(item["P_Output"]),
                                Convert.ToDecimal(item["K_Output"]),
                                Convert.ToDecimal(item["NoOfKgs"]),
                                Convert.ToDecimal(item["NoTotalOfKgs"]),
                                Convert.ToDecimal(item["N"]),
                                Convert.ToDecimal(item["P"]),
                                Convert.ToDecimal(item["K"]),
                                Convert.ToDecimal(item["N_Percentage"]),
                                Convert.ToDecimal(item["P_Percentage"]),
                                Convert.ToDecimal(item["K_Percentage"]),
                                Convert.ToDecimal(item["TEST1"]),
                                Convert.ToDecimal(item["TEST2"]),
                                Convert.ToDecimal(item["TEST3"]),
                                Convert.ToDecimal(item["Test1Reference"]),
                                Convert.ToDecimal(item["Test2Reference"]),
                                Convert.ToDecimal(item["Test3Reference"]),
                                Convert.ToDecimal(item["TEST3"]), // as ProjectedAmount
                                ppString
                                );
                        }
                    }

                }
                else if (dt1.Rows.Count == 3)
                {
                    if (sum1 == 3)
                    {
                        foreach (DataRow item in dt2.Rows)
                        {
                            decimal FertilizerPercentage = Convert.ToDecimal(item["NoOfBags"]) / sum100Kg * 100;
                            decimal ProjectedPercentage = Convert.ToDecimal(item["NoOfBags"]) > 0 ? Convert.ToDecimal(item["TEST1"]) / Convert.ToDecimal(item["NoOfBags"]) * FertilizerPercentage : Convert.ToDecimal(0);
                            string ppString = ProjectedPercentage == 0 ? "0.00" : ProjectedPercentage.ToString("##.##");
                            dtCloned.Rows.Add(
                                Convert.ToDecimal(item["NoOfBags"]),
                                item["FertilizerName"].ToString(),
                                Convert.ToDecimal(item["N_Output"]),
                                Convert.ToDecimal(item["P_Output"]),
                                Convert.ToDecimal(item["K_Output"]),
                                Convert.ToDecimal(item["NoOfKgs"]),
                                Convert.ToDecimal(item["NoTotalOfKgs"]),
                                Convert.ToDecimal(item["N"]),
                                Convert.ToDecimal(item["P"]),
                                Convert.ToDecimal(item["K"]),
                                Convert.ToDecimal(item["N_Percentage"]),
                                Convert.ToDecimal(item["P_Percentage"]),
                                Convert.ToDecimal(item["K_Percentage"]),
                                Convert.ToDecimal(item["TEST1"]),
                                Convert.ToDecimal(item["TEST2"]),
                                Convert.ToDecimal(item["TEST3"]),
                                Convert.ToDecimal(item["Test1Reference"]),
                                Convert.ToDecimal(item["Test2Reference"]),
                                Convert.ToDecimal(item["Test3Reference"]),
                                Convert.ToDecimal(item["TEST1"]), // as ProjectedAmount
                                ppString
                                );
                        }
                    }
                    else if (sum2 == 3)
                    {
                        foreach (DataRow item in dt2.Rows)
                        {
                            decimal FertilizerPercentage = Convert.ToDecimal(item["NoOfBags"]) / sum100Kg * 100;
                            decimal ProjectedPercentage = Convert.ToDecimal(item["NoOfBags"]) > 0 ? Convert.ToDecimal(item["TEST2"]) / Convert.ToDecimal(item["NoOfBags"]) * FertilizerPercentage : Convert.ToDecimal(0);
                            string ppString = ProjectedPercentage == 0 ? "0.00" : ProjectedPercentage.ToString("##.##");
                            dtCloned.Rows.Add(
                                Convert.ToDecimal(item["NoOfBags"]),
                                item["FertilizerName"].ToString(),
                                Convert.ToDecimal(item["N_Output"]),
                                Convert.ToDecimal(item["P_Output"]),
                                Convert.ToDecimal(item["K_Output"]),
                                Convert.ToDecimal(item["NoOfKgs"]),
                                Convert.ToDecimal(item["TotalNoOfKgs"]),
                                Convert.ToDecimal(item["N"]),
                                Convert.ToDecimal(item["P"]),
                                Convert.ToDecimal(item["K"]),
                                Convert.ToDecimal(item["N_Percentage"]),
                                Convert.ToDecimal(item["P_Percentage"]),
                                Convert.ToDecimal(item["K_Percentage"]),
                                Convert.ToDecimal(item["TEST1"]),
                                Convert.ToDecimal(item["TEST2"]),
                                Convert.ToDecimal(item["TEST3"]),
                                Convert.ToDecimal(item["Test1Reference"]),
                                Convert.ToDecimal(item["Test2Reference"]),
                                Convert.ToDecimal(item["Test3Reference"]),
                                Convert.ToDecimal(item["TEST2"]), // as ProjectedAmount
                                ppString
                                );
                        }

                    }
                    else if (sum3 == 3)
                    {
                        foreach (DataRow item in dt2.Rows)
                        {
                            decimal FertilizerPercentage = Convert.ToDecimal(item["NoOfBags"]) / sum100Kg * 100;
                            decimal ProjectedPercentage = Convert.ToDecimal(item["NoOfBags"]) > 0 ? Convert.ToDecimal(item["TEST3"]) / Convert.ToDecimal(item["NoOfBags"]) * FertilizerPercentage: Convert.ToDecimal(0);
                            string ppString = ProjectedPercentage == 0 ? "0.00" : ProjectedPercentage.ToString("##.##");
                            dtCloned.Rows.Add(
                                Convert.ToDecimal(item["NoOfBags"]),
                                item["FertilizerName"].ToString(),
                                Convert.ToDecimal(item["N_Output"]),
                                Convert.ToDecimal(item["P_Output"]),
                                Convert.ToDecimal(item["K_Output"]),
                                Convert.ToDecimal(item["NoOfKgs"]),
                                Convert.ToDecimal(item["NoTotalOfKgs"]),
                                Convert.ToDecimal(item["N"]),
                                Convert.ToDecimal(item["P"]),
                                Convert.ToDecimal(item["K"]),
                                Convert.ToDecimal(item["N_Percentage"]),
                                Convert.ToDecimal(item["P_Percentage"]),
                                Convert.ToDecimal(item["K_Percentage"]),
                                Convert.ToDecimal(item["TEST1"]),
                                Convert.ToDecimal(item["TEST2"]),
                                Convert.ToDecimal(item["TEST3"]),
                                Convert.ToDecimal(item["Test1Reference"]),
                                Convert.ToDecimal(item["Test2Reference"]),
                                Convert.ToDecimal(item["Test3Reference"]),
                                Convert.ToDecimal(item["TEST3"]), // as ProjectedAmount
                                ppString
                                );
                        }
                    }

                }

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

            return new Tuple<DataTable, DataTable>(dt1, dtCloned);
        }
        public bool CalculationInsert(CalculateController cCtrl)
        {
            try
            {
                cmd = new SqlCommand("usp_CalculationInsert");
                cmd.Parameters.AddWithValue("@TownCity", cCtrl.TownCity);
                cmd.Parameters.AddWithValue("@Barangay", cCtrl.Barangay);
                cmd.Parameters.AddWithValue("@NameOfFarmer", cCtrl.NameOfFarmer);
                cmd.Parameters.AddWithValue("@LandArea", cCtrl.LandArea);
                cmd.Parameters.AddWithValue("@SoilType", cCtrl.SoilType);
                cmd.Parameters.AddWithValue("@Season", cCtrl.Season);
                cmd.Parameters.AddWithValue("@Nitrogen", cCtrl.Nitrogen);
                cmd.Parameters.AddWithValue("@Phosphorous", cCtrl.Phosphorous);
                cmd.Parameters.AddWithValue("@Potassium", cCtrl.Potassium);
                cmd.Parameters.AddWithValue("@N", cCtrl.N);
                cmd.Parameters.AddWithValue("@P", cCtrl.P);
                cmd.Parameters.AddWithValue("@K", cCtrl.K);
                cmd.Parameters.AddWithValue("@dtFertilizers", cCtrl.dtFertilizers);
                cmd.Parameters.AddWithValue("@dtFor100Yield", cCtrl.dtFor100Yield);
                cmd.Parameters.AddWithValue("@dtForProjectedYield", cCtrl.dtForProjectedYield);
                cmd.Parameters.AddWithValue("@Total", cCtrl.Total);
                return ExecNonQuery(cmd);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
                return false;
            }
        }
        public DataTable GetAllCalculations(bool isActive = true)
        {
            try
            {
                cmd = new SqlCommand("usp_GetAllCalculations");
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
