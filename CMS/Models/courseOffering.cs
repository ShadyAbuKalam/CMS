namespace CMS.Models
{
    public class CourseOffering 
    {
        
        public string Semster { get; set; }
        public string CourseId { get; set; }


        public override bool Equals(object obj)
        {

            var offering = obj as CourseOffering;

            if (offering == null)
            {
                // If it is null then it is not equal to this instance.
                return false;
            }

            // Instances are considered equal if the ReferenceId matches.
            return offering.Semster.Equals(Semster) && offering.CourseId.Equals(CourseId);
        }
    }
}
