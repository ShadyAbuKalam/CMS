using System.Collections.Generic;
using CMS.Models;

namespace CMS.Database
{
    interface IInstructor
    {
    

        List<Instructor> GetInstructors();
        bool AddInstructor(Instructor instructor);
        bool UpdateInstructor(Instructor instructor);
        bool DeleteInstructor(Instructor instructor);
   
    }
}
