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
    class DBHandler : IStudent,IInstructor, IDepartment
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

        #region  IStudent Implemntation

        

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
        #endregion

        #region IInstructor Implementation


        public List<Instructor> GetInstructors()
        {
            List<Instructor> listInstructor = new List<Instructor>();
         
          
                // Perform database operations
                String sql = "SELECT * FROM instructors";
                MySqlCommand cmd = new MySqlCommand(sql,Connection);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Instructor instructor = new Instructor();
                    instructor.Id = Convert.ToInt32(rdr["ins_id"]);
                    instructor.DepName = rdr["dep_name"].ToString();
                    instructor.Email = rdr["email"].ToString();
                    instructor.Phone = rdr["phone"].ToString();
                    instructor.FullName = rdr["full_name"].ToString();
                    listInstructor.Add(instructor);
                }
                rdr.Close();
            
         
            return listInstructor;
        }

        public bool AddInstructor(Instructor instructor)
        {



            string sql =
                                "INSERT INTO instructors (full_name, dep_name , phone , email) VALUES (@full_name,@dep_name,@phone,@email);";
            MySqlCommand cmd = new MySqlCommand(sql, Connection);
            cmd.Parameters.AddWithValue("@full_name", instructor.FullName);
            cmd.Parameters.AddWithValue("@email", instructor.Email);
            cmd.Parameters.AddWithValue("@phone", instructor.Phone);
            cmd.Parameters.AddWithValue("@dep_name", instructor.DepName);

               return cmd.ExecuteNonQuery() ==1;
            
     

        }
        public bool UpdateInstructor(Instructor instructor)
        {
           

                string sql = "UPDATE instructors set full_name = @full_name, phone = @phone,email = @email,dep_name = @dep_name where ins_id = @id";
                MySqlCommand cmd = new MySqlCommand(sql, Connection);
            cmd.Parameters.AddWithValue("@id", instructor.Id);
            cmd.Parameters.AddWithValue("@full_name", instructor.FullName);
            cmd.Parameters.AddWithValue("@email", instructor.Email);
            cmd.Parameters.AddWithValue("@phone", instructor.Phone);
            cmd.Parameters.AddWithValue("@dep_name", instructor.DepName);
           return cmd.ExecuteNonQuery() ==1;
            
       
        }

        public bool DeleteInstructor(Instructor instructor)
        {


            string sql = "DELETE FROM instructors WHERE ins_id = @id";

                MySqlCommand cmd = new MySqlCommand(sql, Connection);
            cmd.Parameters.AddWithValue("@id", instructor.Id);
            return  cmd.ExecuteNonQuery() ==1;
           
        }

        #endregion
        #region IDepartment Implemntation

        public List<Department> GetDepartments()
        {
            List<Department> departments = new List<Department>();


            String sql = "SELECT * FROM departments";
            MySqlCommand cmd = new MySqlCommand(sql, Connection);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Department department = new Department();
                department.HeadInstructorId = Convert.ToInt32(rdr["headed_by"]);
                department.Name = rdr["dep_name"].ToString();
               
                departments.Add(department);
            }
            rdr.Close();


            return departments;
        }

        public bool AddDepartment(Department department)
        {

            string sql =
                                "INSERT INTO departments (dep_name, headed_by) VALUES (@dep_name,@headed_by);";
            MySqlCommand cmd = new MySqlCommand(sql, Connection);
            cmd.Parameters.AddWithValue("@dep_name", department.Name);
            cmd.Parameters.AddWithValue("@headed_by", department.HeadInstructorId);


            return cmd.ExecuteNonQuery() == 1;
        }

        //Selector is used to allow changing the department name, it must be the old dep name 
        public bool UpdateDepartment(Department department,string selector)
        {
            string sql = "UPDATE departments set dep_name = @dep_name, headed_by= @headed_by  where dep_name = @olddep_name";
            MySqlCommand cmd = new MySqlCommand(sql, Connection);
            cmd.Parameters.AddWithValue("@dep_name", department.Name);
            cmd.Parameters.AddWithValue("@headed_by", department.HeadInstructorId);
            cmd.Parameters.AddWithValue("@olddep_name", selector);
    
            return cmd.ExecuteNonQuery() == 1;
        }

        public bool DeleteDepartment(Department department)
        {

            string sql = "DELETE FROM departments WHERE dep_name = @name";

            MySqlCommand cmd = new MySqlCommand(sql, Connection);
            cmd.Parameters.AddWithValue("@name", department.Name);
            return cmd.ExecuteNonQuery() == 1;
        }

        #endregion

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
