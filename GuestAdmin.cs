using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HR5
{
    public partial class GuestAdmin : Form
    {
        public GuestAdmin()
        {
            InitializeComponent();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            
            AdminLogIn adminLogInForm = new AdminLogIn();
            adminLogInForm.Show();

           
            this.Hide();
        }

        private void btnGuest_Click(object sender, EventArgs e)
        {
            
            UserLogIn userLogInForm = new UserLogIn();
            userLogInForm.Show();

          
            this.Hide();
        }
    }
}
