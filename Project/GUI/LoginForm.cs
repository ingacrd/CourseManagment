using Project.DAC;
using Project.GUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            
            if(txtUserID.Text.Length > 0 && txtPassword.Text.Length>0)
            {
                bool validUserID = validateUserID(txtUserID.Text);

                if(validUserID)
                {
                    try
                    {
                        using (var context = new TeacherCourseDBEntities())
                        {
                            var userToLogin = from user
                                              in context.Users
                                              where user.UserID.ToString() == txtUserID.Text
                                              select user;
                            
                            foreach (var user in userToLogin)
                            {
                                
                                if(user.Password == txtPassword.Text)
                                {
                                    if(user.JobTitle == "Program Coordinator")
                                    {
                                        CourseAssignmentForm obj = new CourseAssignmentForm();
                                        obj.Show();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Only the Coordinator can Assign Courses");
                                    }
                                    
                                }else
                                {
                                    MessageBox.Show("User or Password incorrect");
                                }
                            }
                          

                           
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
               

            }
            else
            {
                MessageBox.Show("User Name or Password can't be empty");
            }
        }

        private bool validateUserID(string userID)
        {
            int id = 0;
            try
            {
                id = Int32.Parse(userID);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Input a valid ID" + ex.Message);
                return false;
            }
        }

        private void btnEye_Click(object sender, EventArgs e)
        {
            if (txtPassword.UseSystemPasswordChar == true)
                txtPassword.UseSystemPasswordChar = false;
            else
                txtPassword.UseSystemPasswordChar = true;
        }

        private void btnExitLogin_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
