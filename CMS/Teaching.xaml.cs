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
            var availableCourseOfferings = DepartmentCourseOffering.Where(
                offering => !TaughtCourseOfferingsView.ToList().ConvertAll(view => view.Offering).Contains(offering));

            foreach (var courseOffering in availableCourseOfferings)
            {
                var cv = new CourseOfferingCompositeView();
                cv.Offering = courseOffering;
                cv.Course = courses.Find(course => course.Id.Equals(courseOffering.CourseId));
                AvailableCourseOfferingsView.Add(cv);
            }
        }


        #endregion

        #region Adding/Removing Offerings

        public List<CourseOfferingCompositeView> ToBeAddedCourseOfferingViews { get; set; } = new List<CourseOfferingCompositeView>();
        public List<CourseOfferingCompositeView> ToBeRemovedCourseOfferingViews { get; set; } = new List<CourseOfferingCompositeView>();

        private void RemoveSelectedOfferingFromInstructor(object sender, RoutedEventArgs e)
        {
            if (ToBeRemovedCourseOfferingViews.Count == 0)
            {
                MessageBox.Show("You must select at least one course offering to be removed", "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;

            }
            foreach (var courseOfferingCompositeView in ToBeRemovedCourseOfferingViews)
            {
                db.RemoveCourseOfferingFromInstructor(Instructor, courseOfferingCompositeView.Offering);
            }
            LoadCourseOfferings();
        }

        private void AddSelectedOfferingToInstructor(object sender, RoutedEventArgs e)
        {

            if(ToBeAddedCourseOfferingViews.Count == 0)
            {
                MessageBox.Show("You must select at least one course offering to be added","Error",MessageBoxButton.OK,MessageBoxImage.Stop);
                return;
                
            }
            foreach (var courseOfferingCompositeView in ToBeAddedCourseOfferingViews)
            {
                db.AddCourseOfferingToInstructor(Instructor, courseOfferingCompositeView.Offering);
            }
            LoadCourseOfferings();
            

        }

        private void SetToBeRemovedOfferings(object sender, SelectionChangedEventArgs e)
        {
            foreach (var eAddedItem in e.AddedItems)
            {
                ToBeRemovedCourseOfferingViews.Add((CourseOfferingCompositeView) eAddedItem);

            }
            foreach (var eRemovedItem in e.RemovedItems)
            {
                ToBeRemovedCourseOfferingViews.Remove((CourseOfferingCompositeView)eRemovedItem);

            }
    
        }

        private void SetToBeAddedOfferings(object sender, SelectionChangedEventArgs e)
        {
            foreach (var eAddedItem in e.AddedItems)
            {
                ToBeAddedCourseOfferingViews.Add((CourseOfferingCompositeView)eAddedItem);

            }
            foreach (var eRemovedItem in e.RemovedItems)
            {
                ToBeAddedCourseOfferingViews.Remove((CourseOfferingCompositeView)eRemovedItem);

            }
        }
        #endregion


    }
}
