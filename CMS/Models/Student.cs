using System;

namespace CMS.Models
{
    public class Student
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string DepName { get; set; }
        public string Phone { get; set; }
        public int StudentId { get; set; }
        public int GPA { get; set; }
        public DateTime? DateOfBirth { get; set; }


     
    }
}
