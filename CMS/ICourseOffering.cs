using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CMS
{
    interface ICourseOffering
    {
        string semster { get; set; }
        int course_id { get; set; }

        List<courseOffering> get_offerings();
        void add_offerings(string sem, int id);

        void update_offering(string sem, string osem, int oid, int id);

        void delete_offering(string sem);


    }
}
