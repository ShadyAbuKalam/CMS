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
    class Instructor: Iinstructor
    {
        public string fullName { get; set; }
        public string email { get; set; }
        public string depName { get; set; }
        public string phone { get; set; }
        public int  instructorId { get; set; }


            public List<Instructor> get_instructor()
            {
                List<Instructor> listInstructor = new List<Instructor>();
                string connStr = " ";
                MySqlConnection conn = new MySqlConnection(connStr);
                try
                {
                    conn.Open();
                    // Perform database operations
                    String DBreply = "SELECT * FROM Instructor";
                    MySqlCommand cmd = new MySqlCommand(DBreply, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                    Instructor instructor = new Instructor();
                    instructor.instructorId = Convert.ToInt32(rdr["Instructor ID"]);
                    instructor.fullName = rdr["full name"].ToString();
                    instructor.email = rdr["Email"].ToString();
                    instructor.phone = rdr["Phone"].ToString();
                    instructor.depName = rdr["dep Name"].ToString();
                    listInstructor.Add(instructor);
                    }
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                conn.Close();
                return listInstructor;
            }

            public void add_instructor(string fullName, int instructorId, string depName, string phone, string email)
        { 
                string connStr = " ";
                MySqlConnection conn = new MySqlConnection(connStr);
                try
                {
                    conn.Open();

                    string sql = "INSERT INTO Instructor (FullName, ID , depName , Phone, Email ) VALUES ('" + fullName + "','" + instructorId + "','" + depName + "','" + phone + "','" + email + "');";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                conn.Close();

            }
            public void update_instructor(int instructorId, string newFullName, int newInstructorId, string newDepName, string newPhone, string newEmail)
            {
                string connStr = " ";
                MySqlConnection conn = new MySqlConnection(connStr);
                try
                {
                    conn.Open();

                    string sql = "UPDATE Instructor SET InstructorId ='" + instructorId + "', depName ='" + newDepName + "', Phone ='" + newPhone + "', Email ='" + newEmail + "', fullName ='" + newFullName + "' WHERE InstructorId='" + instructorId + "' ;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                conn.Close();
            }

            public void delete_instructor(int instructorId)
            {
                string connStr = " ";
                MySqlConnection conn = new MySqlConnection(connStr);
                try
                {
                    conn.Open();

                    string sql = "DELETE FROM Instructor WHERE InstructorId = '" + instructorId + "';";
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

