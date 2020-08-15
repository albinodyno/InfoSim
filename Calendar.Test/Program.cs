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
using Newtonsoft.Json;
using Calendar.Test;
using System.Data.SqlClient;

namespace CalendarQuickstart
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/calendar-dotnet-quickstart.json
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "Google Calendar API .NET Quickstart";

        static string sqlConn;
        static bool sqlOffline;
        private static List<EventModel> calItems = new List<EventModel>();

        static void Main(string[] args)
        {
            SetupSqlConn();

            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Calendar API service.
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            EventsResource.ListRequest request = service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.TimeMax = DateTime.Now.AddMonths(6);
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 30;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            // List events.
            Events events = request.Execute();
            Console.WriteLine("Upcoming events:");
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    EventModel tmpCal = new EventModel();
                    tmpCal.id = eventItem.Id;
                    tmpCal.summary = eventItem.Summary;

                    if (eventItem.Start.Date == null)
                    {
                        tmpCal.allDay = false;

                        tmpCal.startDate = (DateTime)eventItem.Start.DateTime;
                        tmpCal.startTime = tmpCal.startDate.TimeOfDay;
                        tmpCal.startDate = tmpCal.startDate.Date;

                        tmpCal.endDate = (DateTime)eventItem.End.DateTime;
                        tmpCal.endTime = tmpCal.endDate.TimeOfDay;
                        tmpCal.endDate = tmpCal.endDate.Date;
                    }
                    else
                    {
                        tmpCal.startDate = Convert.ToDateTime(eventItem.Start.Date);
                        tmpCal.endDate = Convert.ToDateTime(eventItem.End.Date);
                    }

                    DisplayEvents(tmpCal);
                    calItems.Add(tmpCal);
                }
            }
            //string jsonResult = JsonConvert.SerializeObject(calItems);
            //string path = "C:/json/calendar.Json";

            //if (File.Exists(path))
            //    File.Delete(path);

            //using (var tw = new StreamWriter(path, true))
            //{
            //    tw.WriteLine(jsonResult.ToString());
            //    tw.Close();
            //}
            
            //For prod
            //LogEvents();

            //To test
            Console.ReadLine();
        }

        private static void DisplayEvents(EventModel tmpCal)
        {
            if (tmpCal.allDay)
                Console.WriteLine($"{tmpCal.startDate.Date.ToShortDateString()} - {tmpCal.endDate.Date.ToShortDateString()} : {tmpCal.summary}");
            else
                Console.WriteLine($"{tmpCal.startDate.Date.ToShortDateString()}: {tmpCal.startTime} - {tmpCal.endTime} : {tmpCal.summary}");

        }

        private static void SetupSqlConn()
        {
            try
            {
                sqlConn = @"Server=tcp:WAMPA,49172\SQLEXPRESS;Database=BarringtonDB;Trusted_Connection=True;User Id=albinodyno;Password=thelivingshitouttame";

                SqlConnection connection = new SqlConnection(sqlConn);
                SqlCommand cmd = new SqlCommand("AppTestConnection", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@App", "CalendarApp");

                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();

                sqlOffline = false;
            }
            catch (Exception ex)
            {
                sqlOffline = true;
            }
        }

        private static void LogEvents()
        {
            try
            {
                if (!sqlOffline)
                {
                    SqlConnection connection = new SqlConnection(sqlConn);
                    SqlCommand cmd = new SqlCommand("GCEventEntry", connection);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue();

                    connection.Open();

                    foreach(EventModel e in calItems)
                    {
                        if (e.allDay)
                        {
                            cmd.Parameters.AddWithValue("@Description", $"{e.startDate.Date.ToShortDateString()} - {e.endDate.Date.ToShortDateString()} : {e.summary}");
                            cmd.Parameters.AddWithValue("@StartDate", e.startDate);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Description", $"{e.startDate.Date.ToShortDateString()}: {e.startTime} - {e.endTime} : {e.summary}");
                            cmd.Parameters.AddWithValue("@StartDate", e.startTime);
                        }

                        cmd.Parameters.AddWithValue("@EventID", e.id);

                        int i = cmd.ExecuteNonQuery();
                    }

                    connection.Close();
                }
                else
                    SetupSqlConn();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}