using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models
{
    public class Enrollement : CourseOffering
    {
        public int StudentId { get; set; }
        public int? Grade{ get; set; }

        public override bool Equals(object obj)
        {
            var enrollement = obj as Enrollement;

            if (enrollement == null)
            {
                // If it is null then it is not equal to this instance.
                return false;
            }

            return base.Equals(obj) && StudentId.Equals(enrollement.StudentId) && Nullable.Equals(Grade,enrollement.Grade);
        }
    }
}
