using System.Collections.Generic;
using CMS.Models;

namespace CMS.Database
{
    public interface IStudent
    {
     

        List<Student> GetStudents();
        bool AddStudent(Student student);
        bool UpdateStudent(Student student);
        bool DeleteStudent(Student student);
    }
}
