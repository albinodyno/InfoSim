using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace InfoSim.UI.Tools
{
    class SQLSetup
    {
        //private static string sqlConn = @"Server=tcp:WAMPA,49172\SQLEXPRESS;Database=BarringtonDB;Trusted_Connection=True;User Id=albinodyno;Password=thelivingshitouttame";

        public static bool SetupSqlConn()
        {
            try
            {
                string sqlConn = @"Server=tcp:WAMPA,49172\SQLEXPRESS;Database=BarringtonDB;Trusted_Connection=False;User Id=albinodyno;Password=thelivingshitouttame";

                SqlConnection connection = new SqlConnection(sqlConn);
                SqlCommand cmd = new SqlCommand("AppTestConnection", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@App", "InfoSim");

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
