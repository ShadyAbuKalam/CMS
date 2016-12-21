using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CMS.Models
{
    class ClassHour
    {
        public string Semster { get; set; }
        public string CourseId { get; set; }
        public string Room { get; set; }
        public string Day { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }



    }
}
