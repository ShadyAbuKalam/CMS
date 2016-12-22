using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CMS.Annotations;
using CMS.Database;
using CMS.Models;

namespace CMS
{
    /// <summary>
    /// Interaction logic for EnrollementWindow.xaml
    /// </summary>
    public partial class EnrollementWindow : Window,INotifyPropertyChanged
    {
        public Student Student { get; set; }
        private DBHandler db = new DBHandler();
        private int? _formGrade;
        private CourseOffering _formCourseOffer = new CourseOffering();

        public EnrollementWindow(Student student)
        {
            Student = student;
            LoadEnrollements();
            LoadAllAvailableEnrollements();

            DataContext = this;
            InitializeComponent();
        }

        public class EnrollementView
        {
            public Enrollement Enrollement { get; set; }
            public Course Course { get; set; }
        }

        public class CourseOfferingView
        {
            public CourseOffering Offering { get; set; }
            public Course Course { get; set; }
            public override string ToString()
            {
                return $"{Course.Id} : {Course.Name} - {Offering.Semster}";
            }
        }

        public ObservableCollection<EnrollementView> EnrollementViews { get; set; } = new ObservableCollection<EnrollementView>();
        public ObservableCollection<CourseOfferingView> AllOfferingsViews { get; set; } = new ObservableCollection<CourseOfferingView>();

        private void LoadAllAvailableEnrollements()
        {
            AllOfferingsViews.Clear();
            var courses = db.GetCourses();

            foreach (var offering in db.GetOfferingsByDepartmnet(Student.DepName))
            {
                var view = new CourseOfferingView();
                view.Offering = offering;
                view.Course = courses.Find(course => course.Id.Equals(offering.CourseId));
                AllOfferingsViews.Add(view);
            }
        }
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

        public CourseOffering FormCourseOffer
        {
            get { return _formCourseOffer; }
            set { _formCourseOffer = value; OnPropertyChanged(); }
        }

        public int? FormGrade
        {
            get { return _formGrade; }
            set { _formGrade = value; OnPropertyChanged(); }
        }

        private void CancelEnrollementForm(object sender, RoutedEventArgs e)
        {
            FormGrade = null;
            FormCourseOffer = new CourseOffering();
        }

        private void SubmitEnrollementForm(object sender, RoutedEventArgs e)
        {
            var found = EnrollementViews.Count(view => view.Enrollement.CourseId.Equals(FormCourseOffer.CourseId) &&
                                                       view.Enrollement.Semster.Equals(FormCourseOffer.Semster)) == 1;
            var enrollement = new Enrollement()
            {
                CourseId = FormCourseOffer.CourseId,
                Semster = FormCourseOffer.Semster,
                Grade = FormGrade,
                StudentId = Student.StudentId
            };

            if (found)
            {
                //If found, update the grade
                db.SetGrade(enrollement);

            }
            else
            {
                //This is insertion process 
                db.EnrollsInCourseOffering(enrollement);
            }
            LoadEnrollements();
        }

        private void EditEnrollement(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    var EV = (row.Item as EnrollementView);
                    FormCourseOffer =
                        AllOfferingsViews
                            .First(view => Equals(view.Offering, EV.Enrollement as CourseOffering))
                            .Offering;
                    FormGrade = EV.Enrollement.Grade;

                    break;
                }
       
        }
        #region INotifyProperty


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        
    }


}
