using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CMS.Database;
using CMS.Models;

namespace CMS
{
    /// <summary>
    /// Interaction logic for Teaching.xaml
    /// </summary>
    public partial class Teaching : Window
    {
        public Instructor Instructor { get; set; }
        private DBHandler db = new DBHandler();
        public Teaching(Instructor inst)
        {
            DataContext = this;
            Instructor = inst;
            LoadCourseOfferings();
            InitializeComponent();
        }

        #region Members

        public class CourseOfferingCompositeView
        {
            public Course Course { get; set; }
            public CourseOffering Offering { get; set; }
        }

        public ObservableCollection<CourseOfferingCompositeView> AvailableCourseOfferingsView { get; } = new ObservableCollection<CourseOfferingCompositeView>();
        public ObservableCollection<CourseOfferingCompositeView> TaughtCourseOfferingsView { get; } = new ObservableCollection<CourseOfferingCompositeView>();


        #endregion
        #region Intialization 

        private void LoadCourseOfferings()
        {
            AvailableCourseOfferingsView.Clear();
            TaughtCourseOfferingsView.Clear();
            var courses = db.GetCourses();
            foreach (var courseOffering in db.GetCourseOfferingsByInstructor(Instructor))
            {
                var cv = new CourseOfferingCompositeView();
                cv.Offering = courseOffering;
                cv.Course = courses.Find(course => course.Id.Equals(courseOffering.CourseId));
                TaughtCourseOfferingsView.Add(cv);
            }

            var DepartmentCourseOffering = db.GetOfferingsByDepartmnet(Instructor.DepName);
            var availableCourseOfferings = DepartmentCourseOffering.SkipWhile(
                offering => TaughtCourseOfferingsView.ToList().ConvertAll(view => view.Offering).Contains(offering));

            foreach (var courseOffering in availableCourseOfferings)
            {
                var cv = new CourseOfferingCompositeView();
                cv.Offering = courseOffering;
                cv.Course = courses.Find(course => course.Id.Equals(courseOffering.CourseId));
                AvailableCourseOfferingsView.Add(cv);
            }
        }


        #endregion
      
    }
}
