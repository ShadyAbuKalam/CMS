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
    class courseOffering : ICourseOffering
    {
        
        public string semster { get; set; }
        public int course_id { get; set; }

       
        public List<courseOffering> get_offerings()
        {
            List<courseOffering> listco = new List<courseOffering>();
            string connStr = "Server=localhost; Database=CMS; Uid=root; Pwd='' ";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                // Perform database operations
                String DBreply = "SELECT * FROM courseofferings";
                MySqlCommand cmd = new MySqlCommand(DBreply, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    courseOffering co = new courseOffering();
                    co.course_id = Convert.ToInt32(rdr["courseId"]);
                    co.semster = rdr["semster"].ToString();
                    listco.Add(co);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            return listco;
        }

        public void add_offerings(string sem, int id)
        {

            string connStr = "Server=localhost; Database=CMS; Uid=root; Pwd='' ";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "INSERT INTO courseofferings (semster, courseId) VALUES ( @sem, @id)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
       
        }
        public void update_offering(string sem, string osem, int oid, int id)
        {
            string connStr = "Server=localhost; Database=CMS; Uid=root; Pwd='' ";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "UPDATE courseofferings SET semster =@sem, courseId =@id WHERE semster= @osem AND courseId = @oid ";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
        }

        public void delete_offering(string sem)
        {
            string connStr = "Server=localhost; Database=CMS; Uid=root; Pwd='' ";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "DELETE FROMcCourseofferings WHERE semester = @sem";
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
