using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models
{
    public class Department:ICloneable
    {
        public string Name { get; set; }
        public int HeadInstructorId { get; set; }
        public object Clone()
        {
            return (Department) MemberwiseClone();
        }
    }
}
