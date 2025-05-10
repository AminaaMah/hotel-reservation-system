using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.Odbc;


namespace HR5
{
    public partial class UserLogIn : Form
    {
        string connectionString = @"DSN=HotelDB;";



        public UserLogIn()
        {
            InitializeComponent();
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {


            string ime = txtName.Text.Trim();
            string prezime = txtSurname.Text.Trim();
            string lozinka = txtPassword.Text.Trim();

            using (OdbcConnection conn = new OdbcConnection(connectionString))

            {
                conn.Open();
                string query = "SELECT ID FROM Korisnici WHERE Ime = ? AND Prezime = ? AND Lozinka = ?";
                OdbcCommand cmd = new OdbcCommand(query, conn);

                cmd.Parameters.AddWithValue("?", ime);
                cmd.Parameters.AddWithValue("?", prezime);
                cmd.Parameters.AddWithValue("?", lozinka);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    int korisnikID = Convert.ToInt32(result);

                    MessageBox.Show("Log in successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Proslijedi ID korisnika u formu za rezervaciju
                    Reservation reservationForm = new Reservation(korisnikID);
                    reservationForm.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Login failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }

        private void lblAccount_Click(object sender, EventArgs e)
        {

            Registration registrationForm = new Registration();
            registrationForm.Show();


            this.Close();
        }

        private void UserLogIn_Load(object sender, EventArgs e)
        {

        }

    }
}