using Project.DAC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Project.GUI
{
    public partial class CourseAssignmentForm : Form
    {
        public CourseAssignmentForm()
        {
            InitializeComponent();
        }


        private void btnGetList_Click(object sender, EventArgs e)
        {

            try
            {
                using(var context = new TeacherCourseDBEntities())
                {
                    var teacherToSearch = from course in context.CourseAssignations
                                          where course.TeacherId.ToString() == txtTeacherIDSearch.Text
                                          select course;
                    if (txtTeacherIDSearch.Text == "")
                    {
                        teacherToSearch = from course in context.CourseAssignations
                                          select course;
                    }
                    
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID");
                    dt.Columns.Add("Teacher ID");
                    dt.Columns.Add("Course Number");

                    foreach (CourseAssignation course in teacherToSearch)
                    {
                        dt.Rows.Add(course.id, course.TeacherId, course.CourseNumber);
                    }
                    dataGridView1.DataSource = dt;

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAssign_Click(object sender, EventArgs e)
        {
            int coursesAssigned = TeacherAssignations(txtTeacherIDAssign.Text);
            //MessageBox.Show(coursesAssigned.ToString());
            if (coursesAssigned >= 0 && coursesAssigned < 4) 
            {
                try
                {
                    using (var context = new TeacherCourseDBEntities())
                    {
                        var courseAssig = new CourseAssignation();
                        Random random = new Random();
                        int randomID = random.Next(0, 999999);
                        courseAssig.id = randomID;
                        courseAssig.TeacherId = Int32.Parse(txtTeacherIDAssign.Text);
                        courseAssig.CourseNumber = txtCourseNumberAssign.Text;

                        context.CourseAssignations.Add(courseAssig);
                        context.SaveChanges();

                        MessageBox.Show($"Assignation Added Successfully", "Success",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("A teacher can not have more than 4 courses");
            }
            
        }

        private int TeacherAssignations(string TeacherID)
        {
            int counter = 0;
            try
            {
                using(var context = new TeacherCourseDBEntities())
                {
                    var coursesTeacher = from course in context.CourseAssignations
                                         where course.TeacherId.ToString() == TeacherID
                                         select course;

                    foreach(CourseAssignation course in coursesTeacher)
                    {
                        counter++;
                    }
                    return counter;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
        }
        private bool isCourseNumberExist(string courseNumber)
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {
                    var courseCheck = from course in context.Courses
                                       where course.CourseNumber == courseNumber
                                       select course;

                    foreach (Cours c in courseCheck)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool isTeacherIdExist(int id)
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {
                    var teacherCheck = from teacher in context.Teachers
                                         where teacher.TeacherId == id
                                         select teacher;

                    foreach (Teacher t in teacherCheck)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        private void buttonAddTeacher_Click(object sender, EventArgs e)
        {
            if (!isTeacherIdExist(Int32.Parse(txtTeacherID.Text)))
            {
                try
                {
                    using (var context = new TeacherCourseDBEntities())
                    {
                        var teacherAdd = new Teacher();

                        teacherAdd.TeacherId = Int32.Parse(txtTeacherID.Text);
                        teacherAdd.FirstName = txtTeacherFirstName.Text;
                        teacherAdd.LastName = txtTeacherLastName.Text;
                        teacherAdd.Email = txtTeacherEmail.Text;

                        context.Teachers.Add(teacherAdd);
                        context.SaveChanges();

                        MessageBox.Show($"Teacher Added Successfully", "Success",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("The Teacher ID already exist");
            }
            loadTeachers();


        }

        private void loadTeachers()
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {
                    
                   var teacherLoad = from teacher in context.Teachers
                                 select teacher;
                    

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Teacher ID");
                    dt.Columns.Add("First Name");
                    dt.Columns.Add("Last Name");
                    dt.Columns.Add("Email");


                    foreach (Teacher t in teacherLoad)
                    {
                        dt.Rows.Add(t.TeacherId, t.FirstName, t.LastName, t.Email);
                    }
                    dataGridView2.DataSource = dt;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void loadCourses()
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {

                    var coursesLoad = from course in context.Courses
                                      select course;


                    DataTable dt = new DataTable();
                    dt.Columns.Add("Course Number");
                    dt.Columns.Add("Course Title");
                    dt.Columns.Add("Duration");
                   
                    foreach (Cours c in coursesLoad)
                    {
                        dt.Rows.Add(c.CourseNumber, c.CourseTitle, c.Duration);
                    }
                    dataGridView3.DataSource = dt;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void CourseAssignmentForm_Load(object sender, EventArgs e)
        {
            loadTeachers();
            loadCourses();
        }

        private void buttonEditTeacher_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {
                    var teacherToEdit = from teacher in context.Teachers
                                        where teacher.TeacherId.ToString() == txtTeacherID.Text
                                        select teacher;

                    foreach (var teacher in teacherToEdit)
                    {
                        teacher.FirstName = txtTeacherFirstName.Text;
                        teacher.LastName = txtTeacherLastName.Text;
                        teacher.Email = txtTeacherEmail.Text;
                    }

                    context.SaveChanges();
                    MessageBox.Show($"Teacher Edited Successfully", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    loadTeachers();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonDeleteTeacher_Click(object sender, EventArgs e)
        {

            try
            {
                using (var context = new TeacherCourseDBEntities())
                {
                    var teacherTodelete = from teacher
                                          in context.Teachers
                                          where teacher.TeacherId.ToString() == txtTeacherID.Text
                                          select teacher;

                    foreach (var teacher in teacherTodelete)
                    {
                        context.Teachers.Remove(teacher);
                    }

                    context.SaveChanges();
                    MessageBox.Show($"Teacher Deleted Successfully", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    loadTeachers();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonSearchTeacher_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {

                    var teacherSearch = from teacher in context.Teachers
                                        where teacher.TeacherId.ToString() == txtTeacherID.Text
                                        select teacher;

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Teacher ID");
                    dt.Columns.Add("First Name");
                    dt.Columns.Add("Last Name");
                    dt.Columns.Add("Email");


                    foreach (Teacher t in teacherSearch)
                    {
                        dt.Rows.Add(t.TeacherId, t.FirstName, t.LastName, t.Email);
                    }
                    dataGridView2.DataSource = dt;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void btnAddCourse_Click(object sender, EventArgs e)
        {

            if (!isCourseNumberExist(txtCourseNumber.Text))
            {
                try
                {
                    using (var context = new TeacherCourseDBEntities())
                    {
                        var courseAdd = new Cours();

                        courseAdd.CourseNumber = txtCourseNumber.Text;
                        courseAdd.CourseTitle = txtCourseTitle.Text;
                        courseAdd.Duration = Int32.Parse(txtCourseDuration.Text);

                        context.Courses.Add(courseAdd);
                        context.SaveChanges();

                        MessageBox.Show($"Course Added Successfully", "Success",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("The Course Number already exist");
            }
            loadCourses();
        }

        private void btnEditCourse_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {
                    var courseToEdit = from course in context.Courses
                                        where course.CourseNumber == txtCourseNumber.Text
                                        select course;

                    foreach (var course in courseToEdit)
                    {
                        course.CourseTitle = txtCourseTitle.Text;
                        course.Duration = Int32.Parse(txtCourseDuration.Text);
                        
                    }

                    context.SaveChanges();
                    MessageBox.Show($"Course Edited Successfully", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    loadCourses();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteCourse_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {
                    var courseTodelete = from course
                                          in context.Courses
                                          where course.CourseNumber == txtCourseNumber.Text
                                          select course;

                    foreach (var course in courseTodelete)
                    {
                        context.Courses.Remove(course);
                    }

                    context.SaveChanges();
                    MessageBox.Show($"Course Deleted Successfully", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    loadCourses();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearchCourse_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new TeacherCourseDBEntities())
                {

                    var courseSearch = from course in context.Courses
                                        where course.CourseNumber == txtCourseNumber.Text
                                        select course;

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Course Number");
                    dt.Columns.Add("Course Title");
                    dt.Columns.Add("Course Duration");
                    
                    foreach (Cours c in courseSearch)
                    {
                        dt.Rows.Add(c.CourseNumber, c.CourseTitle, c.Duration);
                    }
                    dataGridView3.DataSource = dt;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
