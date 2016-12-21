using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            DataContext = this;
            InitializeComponent();

            InitializeStudentTab();
            InitializeInstructorTab();
            InitializeDepartmentTab();
            InitializeCourseTab();
            
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
        private Department _formDepartment = new Department();
        private Department _oldFormDepartment = null; // Because we depend on departement name for selection and we allow changing it, we must provide its old values

        public Department FormDepartment
        {
            get { return _formDepartment; }
            set
            {
                _formDepartment = value;
                OnPropertyChanged();
            }
        }
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
        private void EditDepartment(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    FormDepartment = (row.Item as DepartmentView).Department;
                    _oldFormDepartment = (Department) FormDepartment.Clone();
                    break;
                }
        }

        private void SubmitDepartmentForm(object sender, RoutedEventArgs e)
        {
            try
            {

                if (_oldFormDepartment == null)
                {
                    db.AddDepartment(FormDepartment);

                }
                else
                {
                    db.UpdateDepartment(FormDepartment, _oldFormDepartment.Name);
                }
                CancelDepartmentForm(sender, e);
            }
            catch (MySqlException exception)
            {
                if (exception.Number == 1062)
                    MessageBox.Show("Can't have two departments with the same name", "Duplicate Detected",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    MessageBox.Show(exception.Message, "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            LoadDepartments();

        }

        private void CancelDepartmentForm(object sender, RoutedEventArgs e)
        {
            FormDepartment = new Department();
            _oldFormDepartment = null;
        }
        #endregion

        #region Courses Tab

        private Course _formCourse = new Course();
        private Course _oldFormCourse = null; // Because we depend on course idfor selection and we allow changing it, we must provide its old values

        public ObservableCollection<Course> Courses { get; set; } = new ObservableCollection<Course>();

        public Course FormCourse
        {
            get { return _formCourse; }
            set
            {
                _formCourse = value;
                OnPropertyChanged();
            }
        }

        private void InitializeCourseTab()
        {
            LoadCourses();
        }

        private void LoadCourses()
        {
            Courses.Clear();
            foreach (var course in db.GetCourses())
                Courses.Add(course);
        }
        private void EditCourse(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                  
                        FormCourse = row.Item as Course;
                    _oldFormCourse = (Course) FormCourse.Clone();



                    break;
                }
        }
        private void DeleteCourse(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    try
                    {
                        db.DeleteCourse(row.Item as Course);
                        LoadCourses();

                    }
                    catch (MySqlException )
                    {
                        MessageBox.Show("Can't delete a course which was/is taught before/now","",MessageBoxButton.OK,MessageBoxImage.Stop);
                    }
                    break;
                }
        }

        private void SubmitCourseForm(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_oldFormCourse == null)
                {
                    db.AddCourse(FormCourse);

                }
                else
                {
                    db.UpdateCourse(FormCourse, _oldFormCourse.Id);
                }
                CancelCourseForm(sender, e);
            }
            catch (MySqlException exception)
            {//1761 1062
                if (exception.Number == 1761 || exception.Number == 1062)
                {
                    MessageBox.Show("Can't have two courses with the same id.", "Duplicate Detected", MessageBoxButton.OK,MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            LoadCourses();
        }


        private void CancelCourseForm(object sender, RoutedEventArgs e)
        {
            FormCourse = new Course();

            _oldFormCourse = null;
        }

        private void OpenCourseOffering(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    try
                    {
                        var row = (DataGridRow)vis;
                   
                        new CourseOfferingWindow(row.Item as Course).ShowDialog();
                    }
                    catch (MySqlException exception)
                    {
                        if (exception.Number == 1062)
                        {
                            MessageBox.Show("The course can't be taught more than one time a semseter",
                                "Duplicate Detected",MessageBoxButton.OK,MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show(exception.Message,
                               "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
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
