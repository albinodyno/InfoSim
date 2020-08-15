using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Test
{
    class EventModel
    {
        public string id;
        public string summary;
        public DateTime startDate;
        public TimeSpan startTime;
        public DateTime endDate;
        public TimeSpan endTime;
        public bool allDay = true;
        public string test;
    }
}
