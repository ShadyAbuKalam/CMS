using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Models;

namespace CMS.Database
{
    interface ICourse
    {
        List<Course> GetCourses();
        bool AddCourse(Course course);
        bool UpdateCourse(Course course,string selector);
        bool DeleteCourse(Course course);
    }
}
