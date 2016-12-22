using System.Collections.Generic;
using CMS.Models;

namespace CMS.Database
{
    interface IClassHour
    {

        List<ClassHour> GetHours();
        bool AddClassHour(ClassHour hour);
        bool UpdateClassHour(ClassHour hour,ClassHour selector);
        bool DeleteClassHour(ClassHour hour);
        ClassHour GetConflictingHour(ClassHour hour);
    }
}
