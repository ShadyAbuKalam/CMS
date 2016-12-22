using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Models;

namespace CMS.Database
{
    interface ITeaches
    {
        List<CourseOffering> GetCourseOfferingsByInstructor(Instructor instructor);
        List<CourseOffering> AddCourseOfferingToInstructor(Instructor instructor,CourseOffering offering);
    }
}
