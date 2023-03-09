using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NPKalc_v3.Model
{
    public class DatabaseConnection
    {
        static SqlConnection conn = new SqlConnection();

        //DataTable dt = new DataTable();
        static string connString = null;
        static string server, dbname, dbuser, dbpass;

        //constructor
        public DatabaseConnection()
        {

        }


        //open connection to db
        public static void connOpen()
        {
            //use if connection authentication is sql authenticate

            server = "192.168.1.200";
            dbname = "BarangayDB";
            dbuser = "sa";
            dbpass = "Hybrain2018";

            //server
            //connString = "Data Source=" + server + ";Initial Catalog =" + dbname + ";User ID =" + dbuser +
            //                ";Password=" + dbpass + "";

            //local 
            //connString = "Data Source=DESKTOP-CJCI25N;Initial Catalog=BarangayDB;Integrated Security=True";
            connString = "Data Source=RUDY;Initial Catalog=NPKalc_v1;Integrated Security=True";

            conn = new SqlConnection(connString);
            conn.Open();

        }


        /// <summary>
        /// close connection to db
        ///</summary>
        private static void connClose()
        {
            conn.Close();
        }



        /// <summary>
        /// execute non query (to insert or update data for db)
        /// </summary>
        /// <param name="cmd">partial sql command from controller</param>
        /// <returns></returns>
        public static bool ExecNonQuery(SqlCommand cmd)
        {
            try
            {
                bool result;
                //connection.Open();
                connOpen();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                result = Convert.ToBoolean(cmd.ExecuteNonQuery());
                //connection.Close();
                connClose();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// execute non scalar (to insert and get the primary key or return value.)
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public int ExecuteScalar(SqlCommand cmd)
        {
            try
            {
                int result;
                connOpen();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                result = Convert.ToInt32(cmd.ExecuteScalar());
                connClose();
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return 0;
            }
        }


        /// <summary>
        /// to read all the data with parameters
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public DataTable ExecuteReader(SqlCommand cmd)
        {
            DataTable dt = new DataTable();
            try
            {
                connOpen();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader sdr;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);

                connClose();
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                //MessageBox.Show(ex.Message.ToString());
                return dt;
            }
        }


        /// <summary>
        /// to read data without parameters
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public DataTable ExecuteReaderNoParams(SqlCommand cmd)
        {
            DataTable dt = new DataTable();
            try
            {
                connOpen();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                SqlDataReader sdr;
                sdr = cmd.ExecuteReader();
                dt.Load(sdr);

                connClose();
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return dt;
            }
        }
    }
}
