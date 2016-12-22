using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Models;

namespace CMS.Database
{
    interface IDepartment
    {

        List<Department> GetDepartments();
        bool AddDepartment(Department department);
        bool UpdateDepartment(Department department,string oldDepartmentName);
        bool DeleteDepartment(Department department);
    }
}
