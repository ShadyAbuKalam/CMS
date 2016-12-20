using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS
{
    interface Istudent
    {
        string fullName { get; set; }
        string email { get; set; }
        string depName { get; set; }
        string phone { get; set; }
        int studentId { get; set; }
        int GPA { get; set; }
        string dateOfBirth { get; set; }

        List<Student> get_student();
        void add_student(string fullName, int studentId, string depName, string phone, string email, string dateOfBirth ,int GPA);
        void update_student(int studentId, string newFullName, int newstudentId, string newDepName, string newPhone, string newEmail,string newDateOfBirth , int newGPA);
        void delete_student(int studentId);
    }
}
