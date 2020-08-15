using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSim.App.Controls
{
    class SetupControl
    {
        private static string sqlConn = @"Server=tcp:WAMPA,49172\SQLEXPRESS;Database=GarMonDB;Trusted_Connection=True;User Id=albinodyno;Password=thelivingshitouttame";

        public static bool SetupSqlConn()
        {
            try
            {
                sqlConn = @"Server=tcp:WAMPA,49172\SQLEXPRESS;Database=BarringtonDB;Trusted_Connection=True;User Id=albinodyno;Password=thelivingshitouttame";

                SqlConnection connection = new SqlConnection(sqlConn);
                SqlCommand cmd = new SqlCommand("AppTestConnection", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@App", "GarMon");

                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
