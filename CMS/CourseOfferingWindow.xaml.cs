using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using MySql.Data.MySqlClient;

namespace CMS
{
    /// <summary>
    /// Interaction logic for CourseOfferingWindow.xaml
    /// </summary>
    public partial class CourseOfferingWindow : Window,INotifyPropertyChanged
    {
        private Course _course;
        private DBHandler db;
        private CourseOffering _formOffering = new CourseOffering();

        public Course Course
        {
            get { return _course; }
            set { _course = value; OnPropertyChanged(); }
        }

        public ObservableCollection<CourseOffering> CourseOfferings { get; set; } = new ObservableCollection<CourseOffering>();

        public CourseOffering FormOffering
        {
            get { return _formOffering; }
            set { _formOffering = value; OnPropertyChanged();}
        }

        #region Constructors & Initializaiton code

       
        public CourseOfferingWindow(Course c) 
        {
            db = new DBHandler();
            InitializeComponent();
            DataContext = this;

            Course = c;
            LoadOfferings();

        }
        private void LoadOfferings()
        {
            CourseOfferings.Clear();
            foreach (var courseOffering in db.GetOfferings().FindAll(offering => offering.CourseId.Equals(Course.Id)))
                CourseOfferings.Add(courseOffering);
        }

        #endregion

        #region CourseOffering manipulation

        private void ShowCourseOfferingHours(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    try
                    {
                        LoadHours(row.Item as CourseOffering);
                        

                    }
                    catch (MySqlException)
                    {
                        MessageBox.Show("Can't delete a course offering that has registered students", "", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                    break;
                }
        }
        private void DeleteCourseOffering(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    try
                    {
                        db.DeleteCourseOffering(row.Item as CourseOffering);
                        LoadOfferings();

                    }
                    catch (MySqlException)
                    {
                        MessageBox.Show("Can't delete a course offering that has registered students", "", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                    break;
                }
        }
        private void AddCourseOffering(object sender, RoutedEventArgs e)
        {
            FormOffering.CourseId = Course.Id;
            db.AddCourseOffering(FormOffering);
            FormOffering = new CourseOffering();
            LoadOfferings();
        }
        #endregion

        #region Hours Section
        public Dictionary<string,string> Days { get; }= new Dictionary<string, string>()
        {
            {"Saturday","saturday"},
            {"Sunday","sunday"},
            {"Monday","monday"},
            {"Tuesday","tuesday"},
            {"Wednesday","wednesday"},
            {"Thursday","thursday"},
            {"Friday","friday"},


        };
        private ClassHour _formClassHour = new ClassHour();

        public ClassHour FormClassHour
        {
            get { return _formClassHour; }
            set { _formClassHour = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ClassHour> Hours { get; set; } = new ObservableCollection<ClassHour>();
        private CourseOffering _currentEditingOffering;

        private void LoadHours(CourseOffering offering)
        {
            _currentEditingOffering = offering;
            Hours.Clear();
            foreach (var classHour in db.GetHours().FindAll(hour => hour.CourseId == offering.CourseId && hour.Semster.Equals(offering.Semster)))
                Hours.Add( classHour);
        }

        private void DeleteHour(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    try
                    {
                        db.DeleteClassHour(row.Item as ClassHour);
                        LoadHours(_currentEditingOffering);

                    }
                    catch (MySqlException exception)
                    {
                        MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                    break;
                }
        }
        private void SubmitClassHourForm(object sender, RoutedEventArgs e)
        {
            if (_currentEditingOffering == null)
            {
                MessageBox.Show("You have to select an offering first.", "Error",
                  MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            FormClassHour.CourseId = _currentEditingOffering.CourseId;
            FormClassHour.Semster = _currentEditingOffering.Semster;
            var chour = db.GetConflictingHour(FormClassHour);
            if (chour == null)
            {
                db.AddClassHour(FormClassHour);
                LoadHours(_currentEditingOffering);
            }
            else
            {
                var ccourse = db.GetCourseById(chour.CourseId);
                MessageBox.Show($"This would conflict with course {ccourse.Id} : {ccourse.Name}", "Confliction",
                    MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            CancelClasHourForm(sender, e);
        }

        private void CancelClasHourForm(object sender, RoutedEventArgs e)
        {
            FormClassHour = new ClassHour();
        }
        #endregion
        #region INotifyProperty Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #endregion

     
    }
}
