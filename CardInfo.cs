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
    public partial class CardInfo : Form
    {
        string connectionString = @"DSN=HotelDB;";



        public CardInfo()
        {
            InitializeComponent();

        }

        private void btnOK_Click(object sender, EventArgs e)
        {


            if (txtCardNumber.Text == null || txtCardNumber.Text.Trim() == "")
            {
                MessageBox.Show("Please enter your card number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int controlNumber;
            if (txtControlNumber.Text.Length != 3 || !int.TryParse(txtControlNumber.Text, out controlNumber))
            {
                MessageBox.Show("Control number must be exactly 3 digits.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtPassword.Text == null || txtPassword.Text.Trim() == "")
            {
                MessageBox.Show("Please enter your card password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (OdbcConnection conn = new OdbcConnection(connectionString))

            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Kartice (BrojKartice, KontrolniBroj, Sifra) VALUES (?, ?, ?)";
                    OdbcCommand cmd = new OdbcCommand(query, conn);
                    cmd.Parameters.AddWithValue("?", txtCardNumber.Text.Trim());
                    cmd.Parameters.AddWithValue("?", txtControlNumber.Text.Trim());
                    cmd.Parameters.AddWithValue("?", txtPassword.Text.Trim());

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Payment successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void CardInfo_Load(object sender, EventArgs e)
        {

        }
    }
}