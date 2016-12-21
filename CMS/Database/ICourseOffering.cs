using System.Collections.Generic;
using CMS.Models;

namespace CMS.Database
{
    interface ICourseOffering
    {


        List<CourseOffering> GetOfferings();
        bool AddCourseOffering(CourseOffering offering);


        bool DeleteCourseOffering(CourseOffering offering);

     

    }
}
