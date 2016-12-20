using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Models;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace CMS.Database
{
    class DBHandler : IStudent
    {
        private MySqlConnection _connection;

        private MySqlConnection Connection
        {
            get
            {
                string connStr = "server=localhost;user=root;database=cms;port=3306;";
                if (_connection == null)
                {
                    _connection = new MySqlConnection(connStr);
                    _connection.Open();
                }
                return
                    _connection;
            }
        }


        public List<Student> GetStudents()
        {
            List<Student> listStudent = new List<Student>();

            try
            {
                // Perform database operations
                String DBreply = "SELECT * FROM students";
                MySqlCommand cmd = new MySqlCommand(DBreply, Connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Student student = new Student();
                    student.StudentId = Convert.ToInt32(rdr["s_id"]);
                    student.FullName = rdr["full_name"].ToString();
                    student.Email = rdr["email"].ToString();
                    student.Phone = rdr["phone"].ToString();
                    student.GPA = Convert.ToInt32(rdr["gpa"]);
                    student.DepName = rdr["dep_name"].ToString();
                    student.DateOfBirth = ConvertDateFromDatabae( rdr["date_of_birth"]);
                    listStudent.Add(student);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return listStudent;
        }

        public bool AddStudent(Student student)
        {
           

                string sql =
                    "INSERT INTO students (full_name, email , phone , gpa, dep_name, date_of_birth) VALUES (@full_name,@email,@phone,@gpa,@dep_name, @date_of_birth);";
                MySqlCommand cmd = new MySqlCommand(sql, Connection);
                cmd.Parameters.AddWithValue("@full_name", student.FullName);
                cmd.Parameters.AddWithValue("@email", student.Email);
                cmd.Parameters.AddWithValue("@phone", student.Phone);
                cmd.Parameters.AddWithValue("@gpa", student.GPA);
                cmd.Parameters.AddWithValue("@dep_name", student.DepName);
                cmd.Parameters.AddWithValue("@date_of_birth", student.DateOfBirth);
                return cmd.ExecuteNonQuery() == 1;
          
        }



        public bool UpdateStudent(Student student)
        {


                string sql =
                    "UPDATE students SET full_name = @full_name, email = @email, phone = @phone,gpa = @gpa,dep_name = @dep_name,date_of_birth = @date_of_birth WHERE s_id = @id ;";

                MySqlCommand cmd = new MySqlCommand(sql, Connection);
                cmd.Parameters.AddWithValue("@id", student.StudentId);
                cmd.Parameters.AddWithValue("@full_name", student.FullName);
                cmd.Parameters.AddWithValue("@email", student.Email);
                cmd.Parameters.AddWithValue("@phone", student.Phone);
                cmd.Parameters.AddWithValue("@gpa", student.GPA);
                cmd.Parameters.AddWithValue("@dep_name", student.DepName);
                cmd.Parameters.AddWithValue("@date_of_birth", student.DateOfBirth);
                return cmd.ExecuteNonQuery() == 1;
            
           
        }

        public bool DeleteStudent(Student student)
        {

          

                string sql = "DELETE FROM students WHERE s_id = @id ;";
                MySqlCommand cmd = new MySqlCommand(sql, Connection);
                cmd.Parameters.AddWithValue("@id", student.StudentId);
                return cmd.ExecuteNonQuery() == 1;
           
        }

        private DateTime? ConvertDateFromDatabae(object obj)
        {
            try
            {
                string date = obj.ToString();
                return DateTime.Parse(date);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
