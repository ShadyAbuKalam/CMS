using System;

namespace CMS.Models
{
    public class Course : ICloneable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int CreditHours { get; set; }
        public string DepartmentName { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
