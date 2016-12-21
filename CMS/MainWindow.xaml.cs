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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CMS.Database;
using CMS.Models;
using MySql.Data.MySqlClient;

namespace CMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        private DBHandler db = new DBHandler();

        public MainWindow()
        {
            InitializeStudentTab();
            InitializeInstructorTab();
            InitializeDepartmentTab();
            InitializeComponent();
            DataContext = this;

        }


        #region Students Tab
        private Student _formStudent = new Student();

        public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();

        public Student FormStudent
        {
            get { return _formStudent; }
            set
            {
                _formStudent = value;
                OnPropertyChanged();
            }
        }

        private void InitializeStudentTab()
        {
            LoadStudents();
        }

        private void LoadStudents()
        {
            Students.Clear();
            foreach (var student in db.GetStudents())
                Students.Add(student);
        }
        private void EditStudent(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                  
                        FormStudent = row.Item as Student;
                    
                    
                   
                    break;
                }
        }
        private void DeleteStudent(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    try
                    {
                        db.DeleteStudent(row.Item as Student);
                        LoadStudents();

                    }
                    catch (MySqlException )
                    {
                        MessageBox.Show("Can't delete a student who has classes","",MessageBoxButton.OK,MessageBoxImage.Stop);
                    }
                    break;
                }
        }

        private void SubmitStudentForm(object sender, RoutedEventArgs e)
        {
            if (FormStudent.StudentId == 0)
            {
                db.AddStudent(FormStudent);
                LoadStudents();
                FormStudent = new Student();

            }
            else
            {
                db.UpdateStudent(FormStudent);
                LoadStudents();
                FormStudent = new Student();
            }
        }


        private void CancelStudentForm(object sender, RoutedEventArgs e)
        {
FormStudent = new Student();
        }
        #endregion

        #region Instructors Tab
        private Instructor _formInstructor = new Instructor();


        public Instructor FormInstructor
        {
            get { return _formInstructor; }
            set
            {
                _formInstructor = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Instructor> Instructors { get; set; } = new ObservableCollection<Instructor>();

        private void InitializeInstructorTab()
        {
            LoadInstructors();
        }

        private void LoadInstructors()
        {
            Instructors.Clear();
            foreach (var instructor in db.GetInstructors())
                Instructors.Add(instructor);
        }

        private void DeleteInstructor(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    try
                    {
                        db.DeleteInstructor(row.Item as Instructor);
                        LoadInstructors();
                        
                    }
                    catch (MySqlException)
                    {
                        MessageBox.Show("Can't delete an instructor who teaches classes", "", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                    break;
                }
        }
        private void EditInstructor(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;

                    FormInstructor = row.Item as Instructor;



                    break;
                }
        }

        private void SubmitInstructorForm(object sender, RoutedEventArgs e)
        {
            if (FormInstructor.Id == 0)
            {
                db.AddInstructor(FormInstructor);
                LoadInstructors();
                FormInstructor = new Instructor();

            }
            else
            {
                db.UpdateInstructor(FormInstructor);
                LoadInstructors();
                FormInstructor = new Instructor();
            }
        }

        private void CancelInstructorForm(object sender, RoutedEventArgs e)
        {
            FormInstructor = new Instructor();
            
        }

        #endregion

        #region Departmetns Tab

        public class DepartmentView
        {
            public Department Department { get; set; }
            public Instructor Head { get; set; }

        }
        public ObservableCollection<DepartmentView> Departments { get; set; } = new ObservableCollection<DepartmentView>();

        //Note : must intialize Instructors before it.
        private void InitializeDepartmentTab()
        {
            
            LoadDepartments();
        }

        private void LoadDepartments()
        {
            Departments.Clear();
            foreach (var department in db.GetDepartments())
            {
                var departmentView = new DepartmentView()
                {
                    Department = department,
                    Head = Instructors.FirstOrDefault(instructor => instructor.Id == department.HeadInstructorId)
                };
                Departments.Add(departmentView);
        }
        }
        private void DeleteDepartment(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    try
                    {
                        db.DeleteDepartment((row.Item as DepartmentView)?.Department);
                        LoadDepartments();

                    }
                    catch (MySqlException)
                    {
                        MessageBox.Show("Can't delete an department if it has head or courses", "", MessageBoxButton.OK, MessageBoxImage.Stop);
                    }
                    break;
                }
        }

        #endregion

        #region  INotify Implemntation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

  
    }
}
