using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace CMS
{
    interface IHours
    {
        string semster { get; set; }
        int course_id { get; set; }
        string room { get; set; }
        string Day { get; set; }
        DateTime start_time { get; set; }
        DateTime end_time { get; set; }
        List<Hours> get_hours();
        void add_hours(string sem, int id, string room, string day, DateTime start, DateTime end);
        void update_offering(int id, string day, string oday, string room, DateTime str, DateTime ostr, DateTime end);
        void delete_Hour(string day, string room, int id);
    }
}
