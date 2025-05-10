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
    public partial class YourReservation : Form
    {
        private decimal totalPrice;
        private DateTime startDate;
        private DateTime endDate;
        private bool isCardPayment;

        public YourReservation(DateTime startDate, DateTime endDate, decimal totalPrice)
        {
            InitializeComponent();
            this.totalPrice = totalPrice;
            this.startDate = startDate;
            this.endDate = endDate;
            this.isCardPayment = isCardPayment;
        }

       
        private void YourReservation_Load(object sender, EventArgs e)
        {
            
            listBox1.Items.Add("Start Date: " + startDate.ToShortDateString());
            listBox1.Items.Add("End Date: " + endDate.ToShortDateString());
            listBox1.Items.Add("Total Price: " + totalPrice + " €");

            
            string roomType = this.Tag as string;

            if (roomType != null)
            {
                switch (roomType)
                {
                    case "Single":
                        pictureBox1.Image = Image.FromFile(@"C:\Users\Amina\Desktop\HR5\Images\singleRoom.jpg");
                        break;
                    case "Double":
                        pictureBox1.Image = Image.FromFile(@"C:\Users\Amina\Desktop\HR5\Images\doubleRoom.jpg");
                        break;
                    case "Triple":
                        pictureBox1.Image = Image.FromFile(@"C:\Users\Amina\Desktop\HR5\Images\tripleRoom.jpg");
                        break;
                    case "DeLux":
                        pictureBox1.Image = Image.FromFile(@"C:\Users\Amina\Desktop\HR5\Images\DeLuxRoom.jpg");
                        break;
                    case "Apartment":
                        pictureBox1.Image = Image.FromFile(@"C:\Users\Amina\Desktop\HR5\Images\Apartment.jpg");
                        break;
                    case "King":
                        pictureBox1.Image = Image.FromFile(@"C:\Users\Amina\Desktop\HR5\Images\kingApartment.jpg");
                        break;
                    default:
                        MessageBox.Show("No image available for the selected room type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }

                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; 
            }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            if (isCardPayment)
            {
                
                CardInfo cardInfoForm = new CardInfo();
                cardInfoForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Your reservation has been confirmed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.Close();

        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            // Odbijanje rezervacije
            MessageBox.Show("Your reservation has been canceled.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close(); // Zatvaranje trenutne forme
        }
    }
}
