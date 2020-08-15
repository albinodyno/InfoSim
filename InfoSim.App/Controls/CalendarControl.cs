using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using InfoSim.App.Models;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Data;

namespace InfoSim.App.Controls
{
    class CalendarControl
    {
        private static string sqlConn = @"Server=tcp:WAMPA,49172\SQLEXPRESS;Database=BarringtonDB;Trusted_Connection=True;User Id=albinodyno;Password=thelivingshitouttame";
        static List<EventModel> calItems = new List<EventModel>();

        public static List<EventModel> GetCalItems()
        {
            //Run sql command to retrieve calendar items
            try
            {
                SqlConnection connection = new SqlConnection(sqlConn);
                SqlCommand cmd = new SqlCommand("GCEventRetrieval", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EventModel tempCal = new EventModel();
                        tempCal.description = reader["Description"].ToString();
                        tempCal.eventID = reader["EventID"].ToString();
                        tempCal.startDate = reader["StartDate"].ToString();

                        calItems.Add(tempCal);
                    }
                }
                connection.Close();

                return calItems;
            }
            catch (Exception ex)
            {
                return calItems;
            }

        }
    }
}

