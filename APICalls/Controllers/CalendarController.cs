using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using APICalls.Models;
using Google.Apis.Auth.OAuth2;
using System.IO;
using Google.Apis.Calendar.v3;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using Google.Apis.Calendar.v3.Data;
using Newtonsoft.Json;

namespace APICalls.Controllers
{


    [Route("Calendar")]
    class CalendarController : Controller
    {
        static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
        static string ApplicationName = "Google Calendar API .NET Quickstart";
        private static List<CalendarItem> calItems = new List<CalendarItem>();


        [HttpGet]
        [Route("List/Next3Months")]
        public IEnumerable<CalendarItem> CalendarItems()
        {
            try
            {
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
                request.MaxResults = 15;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                // List events.
                Events events = request.Execute();
                Console.WriteLine("Upcoming events:");
                if (events.Items != null && events.Items.Count > 0)
                {
                    foreach (var eventItem in events.Items)
                    {
                        CalendarItem tmpCal = new CalendarItem();
                        tmpCal.summary = eventItem.Summary;
                        tmpCal.startDate = eventItem.Start.Date;
                        tmpCal.startTime = eventItem.Start.DateTime;
                        tmpCal.endDate = eventItem.End.Date;
                        tmpCal.endTime = eventItem.End.DateTime;

                        calItems.Add(tmpCal);
                    }
                }
                string jsonResult = JsonConvert.SerializeObject(calItems);
                //string path = "C:/json/calendar.Json";

                //if (File.Exists(path))
                //    File.Delete(path);

                return calItems;
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
