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
    class Hours : IHours
    {
        public string semster { get; set; }
        public int course_id { get; set; }
        public string room { get; set; }
        public string Day { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }

        public List<Hours> get_hours()
        {
            List<Hours> listhour = new List<Hours>();
            string connStr = " ";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                // Perform database operations
                String DBreply = "SELECT * FROM hours";
                MySqlCommand cmd = new MySqlCommand(DBreply, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Hours hour = new Hours();
                    hour.course_id = Convert.ToInt32(rdr["courseId"]);
                    hour.semster = rdr["semster"].ToString();
                    hour.room = rdr["room"].ToString();
                    hour.Day = rdr["day"].ToString();
                    hour.start_time=Convert.ToDateTime(rdr["startTime"]);
                    hour.end_time = Convert.ToDateTime(rdr["endTime"]);
                    listhour.Add(hour);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            return listhour;
        }

        public void update_offering(int id, string day, string oday, string room, DateTime str, DateTime ostr, DateTime end)
        {
            string connStr = " ";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "UPDATE hours SET day ='" + day + "', room ='" + room + "', startTime ='" + str + "', endTime ='" + end + "' WHERE courseId = '" + id + "'AND day = '"+oday+"'AND startTime='"+ostr+"';";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
        }

        public void add_hours(string sem, int id, string room, string day, DateTime start, DateTime end)
        {

            string connStr = " ";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "INSERT INTO courseofferings (semster, courseId, room, day, startTime, endTime) VALUES ('" + sem + "','" + id + "','" + room + "','" + day + "','" + start + "','" + end + "');";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

        }
        public void delete_Hour(string day, string room, int id)
        {
            string connStr = " ";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "DELETE FROM hours WHERE room = '" + room +"', courseId='"+id+"',day = '"+day+ "';";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
        }


    }
}
