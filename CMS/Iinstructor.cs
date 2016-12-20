using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS
{
    interface Iinstructor
    {
         string fullName { get; set; }
         string email { get; set; }
         string depName { get; set; }
         string phone { get; set; }
         int instructorId { get; set; }

        List<Instructor> get_instructor();
        void add_instructor(string fullName, int instructorId, string depName, string phone, string email);
        void update_instructor(int instructorId, string newFullName, int newInstructorId, string newDepName, string newPhone, string newEmail);
        void delete_instructor(int instructorId);
    }
}
