using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Models;

namespace CMS.Database
{
    interface IEnrolls
    {
        
            List<Enrollement> GetEnrollementsByStudent(Student student);
            bool EnrollsInCourseOffering(Enrollement enrollement);
            bool LeavesCourseOffering(Enrollement enrollement);
            bool SetGrade(Enrollement enrollement);
        
    }
}
