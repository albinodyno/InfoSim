using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InfoSim.App.Controls
{
    class GarMonControl
    {
        private static string sqlConn = @"Server=tcp:WAMPA,49172\SQLEXPRESS;Database=GarMonDB;Trusted_Connection=True;User Id=albinodyno;Password=thelivingshitouttame";

        public static string GetStatus()
        {
            try
            {
                SqlConnection connection = new SqlConnection(sqlConn);
                SqlCommand cmd = new SqlCommand("GMStatusRetrieval", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //Get return command
                SqlParameter output = cmd.Parameters.Add("@status", SqlDbType.NVarChar);
                output.Direction = ParameterDirection.ReturnValue;

                connection.Open();
                int i = cmd.ExecuteNonQuery();
                var gmStatus = output.Value.ToString();
                connection.Close();

                return gmStatus;
            }
            catch(Exception ex)
            {
                return "Unknown";
            }
        }
    }
}
