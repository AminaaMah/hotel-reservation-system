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
    public partial class Registration : Form
    {
        string connectionString = @"DSN=HotelDB;";



        public Registration()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string surname = txtSurname.Text.Trim();
            string password = txtPassword.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string mail = txtMail.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(mail))
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (OdbcConnection conn = new OdbcConnection(connectionString))

            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Korisnici (Ime, Prezime, Lozinka, Telefon, Email) VALUES (?, ?, ?, ?, ?)";
                    OdbcCommand cmd = new OdbcCommand(query, conn);
                    cmd.Parameters.AddWithValue("?", name);
                    cmd.Parameters.AddWithValue("?", surname);
                    cmd.Parameters.AddWithValue("?", password);
                    cmd.Parameters.AddWithValue("?", phone);
                    cmd.Parameters.AddWithValue("?", mail);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Prelaz na login formu
                    UserLogIn userLogInForm = new UserLogIn();
                    userLogInForm.Show();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void Registration_Load(object sender, EventArgs e)
        {

        }


    }
}