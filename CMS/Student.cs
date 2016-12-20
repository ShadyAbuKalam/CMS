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
    class Student:Istudent
    {
        public string fullName { get; set; }
        public string email { get; set; }
        public string depName { get; set; }
        public string phone { get; set; }
        public int studentId { get; set; }
        public int GPA { get; set; }
        public string dateOfBirth { get; set; }


        public List<Student> get_student()
        {
            List<Student> listStudent = new List<Student>();
            string connStr = " ";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
                // Perform database operations
                String DBreply = "SELECT * FROM Student";
                MySqlCommand cmd = new MySqlCommand(DBreply, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Student student = new Student();
                    student.studentId = Convert.ToInt32(rdr["Student ID"]);
                    student.fullName = rdr["full name"].ToString();
                    student.email = rdr["Email"].ToString();
                    student.phone = rdr["Phone"].ToString();
                    student.GPA = Convert.ToInt32(rdr["GPA"]);
                    student.depName = rdr["dep Name"].ToString();
                    student.dateOfBirth = rdr["Date Of Birth"].ToString();
                    listStudent.Add(student);
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            return listStudent;
        }

        public void add_student(string fullName, int studentId, string depName, string phone, string email, string dateOfBirth,int GPA)
        {
            string connStr = " ";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "INSERT INTO Instructor (FullName, ID , depName , Phone, Email, Date of Birth , GPA ) VALUES ('" + fullName + "','" + studentId + "','" + depName + "','" + phone + "','" + email + "','" + dateOfBirth + "','" + GPA + "');";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();

        }
        public void update_student(int studentId, string newFullName, int newstudentId, string newDepName, string newPhone, string newEmail, string newDateOfBirth, int newGPA)
        {
            string connStr = " ";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "UPDATE Instructor SET StudentId ='" + studentId + "', depName ='" + newDepName + "', Phone ='" + newPhone + "', Email ='" + newEmail + "', fullName ='" + newFullName + "', Date Of Birth ='" + newDateOfBirth + "', GPA ='" + newGPA + "' WHERE InstructorId='" + studentId + "' ;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
        }

        public void delete_student(int studentId)
        {
            string connStr = " ";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = "DELETE FROM Instructor WHERE studentId = '" + studentId + "';";
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
