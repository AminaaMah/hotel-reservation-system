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
    public partial class Reservation : Form
    {
        private int korisnikID;
        decimal totalPrice;

        string connectionString = @"DSN=HotelDB;";



        public Reservation(int korisnikID)
        {
            InitializeComponent();
            this.korisnikID = korisnikID;
        }

        /*public Reservation()
        {
            InitializeComponent();
        }*/

        private bool isCardPayment = false;

        private void btnContinue_Click(object sender, EventArgs e)
        {
            try
            {
                string tipSobe = "";
                if (rdbSingle.Checked) tipSobe = "Single";
                else if (rdbDouble.Checked) tipSobe = "Double";
                else if (rdbTriple.Checked) tipSobe = "Triple";
                else if (rdbDeLux.Checked) tipSobe = "DeLux";
                else if (rdbApartment.Checked) tipSobe = "Apartment";
                else if (rdbKing.Checked) tipSobe = "King";


                string hrana = "";
                if (rdbAllInclusive.Checked) hrana = "All Inclusive";
                else if (rdbFullPansion.Checked) hrana = "Full Pansion";
                else if (rdbHalfPasnion.Checked) hrana = "Half Pansion";
                else if (rdbOnlyBreakfast.Checked) hrana = "Only Breakfast";

                string nacinPlacanja = rdbCard.Checked ? "Card" : "Cash";

                // Cijena sobe
                decimal cijenaSobe = 0;
                if (rdbSingle.Checked) cijenaSobe = 50;
                else if (rdbDouble.Checked) cijenaSobe = 80;
                else if (rdbTriple.Checked) cijenaSobe = 110;
                else if (rdbDeLux.Checked) cijenaSobe = 150;
                else if (rdbApartment.Checked) cijenaSobe = 250;
                else if (rdbKing.Checked) cijenaSobe = 500;

                // Cijena hrane
                decimal cijenaHrane = 0;
                if (rdbAllInclusive.Checked) cijenaHrane = 120;
                else if (rdbFullPansion.Checked) cijenaHrane = 90;
                else if (rdbHalfPasnion.Checked) cijenaHrane = 75;
                else if (rdbOnlyBreakfast.Checked) cijenaHrane = 55;

                // Ukupna cijena
                totalPrice = cijenaSobe + cijenaHrane;


                using (OdbcConnection conn = new OdbcConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO Rezervacije (KorisnikID, DatumOd, DatumDo, TipSobe, Odrasli, Djeca, Hrana, NacinPlacanja, Cijena) " +
                                   "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";
                    OdbcCommand cmd = new OdbcCommand(query, conn);

                    cmd.Parameters.AddWithValue("?", korisnikID);
                    cmd.Parameters.AddWithValue("?", monthCalendar1.SelectionStart);
                    cmd.Parameters.AddWithValue("?", monthCalendar1.SelectionEnd);
                    cmd.Parameters.AddWithValue("?", tipSobe);
                    cmd.Parameters.AddWithValue("?", int.Parse(cmbAdults.SelectedItem.ToString()));
                    cmd.Parameters.AddWithValue("?", int.Parse(cmbChildren.SelectedItem.ToString()));
                    cmd.Parameters.AddWithValue("?", hrana);
                    cmd.Parameters.AddWithValue("?", nacinPlacanja);
                    cmd.Parameters.AddWithValue("?", totalPrice);

                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show(
    "Total price for your stay is: " + totalPrice + "€\n" +
    "Room type: " + tipSobe + "\n" +
    "Pansion: " + hrana,
    "Reservation Summary",
    MessageBoxButtons.OK,
    MessageBoxIcon.Information
);


                CardInfo cardInfoForm = new CardInfo();
                cardInfoForm.Show();
                this.Hide();

            }




            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            UserLogIn userLogInForm = new UserLogIn();
            userLogInForm.Show();
            this.Close();
        }

        private void Reservation_Load(object sender, EventArgs e)
        {

        }
    }
}