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
    public partial class AdminLogIn : Form
    {
        public AdminLogIn()
        {
            InitializeComponent();
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (username == "admin" && password == "1234")
            {
                MessageBox.Show("Log in successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                
                ReservationOverview reservationForm = new ReservationOverview();
                reservationForm.Show();

                
                this.Close();
            }
            else
            {
                MessageBox.Show("Error, try again", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
