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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CustomizeControls();
            CenterControls();
        }

        private void CustomizeControls()
        {
            
            lblWelcome.Font = new Font("Arial", 24, FontStyle.Bold);

            
            btnContinue.Font = new Font("Arial", 14, FontStyle.Regular);
        }

        private void CenterControls()
        {
          
            lblWelcome.Left = (this.ClientSize.Width - lblWelcome.Width) / 2;
            lblWelcome.Top = (this.ClientSize.Height - lblWelcome.Height) / 3;

            
            btnContinue.Left = (this.ClientSize.Width - btnContinue.Width) / 2;
            btnContinue.Top = lblWelcome.Bottom + 20; 
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
          
            GuestAdmin guestAdminForm = new GuestAdmin();
            guestAdminForm.Show();

            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

       
    }
}
