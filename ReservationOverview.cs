using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HR5
{
    public partial class ReservationOverview : Form
    {

        public ReservationOverview()
        {
            InitializeComponent();
           
        }

        private void ReservationOverview_Load(object sender, EventArgs e)
        {
            string FilePath = @"C:\Users\Amina\Desktop\HR5\Dates.txt";
            // Učitavanje podataka iz fajla u listBox1ReservedDates
            if (File.Exists(FilePath))
            {
                string[] lines = File.ReadAllLines(FilePath);
                foreach (string line in lines)
                {
                    listBox1ReservedDates.Items.Add(line);
                }
            }
        }

        public void AddReservation(string reservation)
        {
            string FilePath = @"C:\Users\Amina\Desktop\HR5\Dates.txt";
            listBox1ReservedDates.Items.Add(reservation);

            // Upisivanje nove rezervacije u fajl
            using (StreamWriter writer = new StreamWriter(FilePath, true))
            {
                writer.WriteLine(reservation);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBox1ReservedDates.SelectedItem != null)
            {
                string selectedItem = listBox1ReservedDates.SelectedItem.ToString();

                // Uklanjanje iz listBox1ReservedDates
                listBox1ReservedDates.Items.Remove(selectedItem);

                // Dodavanje u listBoxDelete
                listBoxDelete.Items.Add(selectedItem);

                // Ažuriranje fajla nakon brisanja
                UpdateFile();
            }
            else
            {
                MessageBox.Show("Please select a reservation to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateFile()
        {
            string FilePath = @"C:\Users\Amina\Desktop\HR5\Dates.txt";
            // Upisivanje trenutnih podataka iz listBox1ReservedDates u fajl
            using (StreamWriter writer = new StreamWriter(FilePath, false))
            {
                foreach (var item in listBox1ReservedDates.Items)
                {
                    writer.WriteLine(item.ToString());
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        
    }
}
