using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CMS.Database;
using CMS.Models;

namespace CMS
{
    /// <summary>
    /// Interaction logic for EnrollementWindow.xaml
    /// </summary>
    public partial class EnrollementWindow : Window
    {
        public Student Student { get; set; }
        private DBHandler db = new DBHandler();
        public EnrollementWindow(Student student)
        {
            Student = student;
            LoadEnrollements();


            DataContext = this;
            InitializeComponent();
        }

        public class EnrollementView
        {
            public Enrollement Enrollement { get; set; }
            public Course Course { get; set; }
        }

        public ObservableCollection<EnrollementView> EnrollementViews { get; set; } = new ObservableCollection<EnrollementView>();

        private void LoadEnrollements()
        {
            EnrollementViews.Clear();
            var courses = db.GetCourses();
            foreach (var enrollement in db.GetEnrollementsByStudent(Student))
            {
                var view = new EnrollementView();
                view.Enrollement = enrollement;
                view.Course = courses.Find(course => course.Id.Equals(enrollement.CourseId));
                EnrollementViews.Add(view);
            }
        }

        private void RemoveEnrollement(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    var EV = (row.Item as EnrollementView);
                    db.LeavesCourseOffering(EV.Enrollement);


                    break;
                }
            LoadEnrollements();
        }
    }
}
