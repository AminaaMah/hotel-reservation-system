namespace HR5
{
    partial class Reservation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbChildren = new System.Windows.Forms.ComboBox();
            this.cmbAdults = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdbDeLux = new System.Windows.Forms.RadioButton();
            this.rdbKing = new System.Windows.Forms.RadioButton();
            this.rdbApartment = new System.Windows.Forms.RadioButton();
            this.rdbTriple = new System.Windows.Forms.RadioButton();
            this.rdbDouble = new System.Windows.Forms.RadioButton();
            this.rdbSingle = new System.Windows.Forms.RadioButton();
            this.SeelctPansion = new System.Windows.Forms.GroupBox();
            this.rdbAllInclusive = new System.Windows.Forms.RadioButton();
            this.rdbOnlyBreakfast = new System.Windows.Forms.RadioButton();
            this.rdbHalfPasnion = new System.Windows.Forms.RadioButton();
            this.rdbFullPansion = new System.Windows.Forms.RadioButton();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdbHotel = new System.Windows.Forms.RadioButton();
            this.rdbCard = new System.Windows.Forms.RadioButton();
            this.btnContinue = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SeelctPansion.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.LightBlue;
            this.groupBox1.Controls.Add(this.cmbChildren);
            this.groupBox1.Controls.Add(this.cmbAdults);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(181, 85);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fill in guests";
            // 
            // cmbChildren
            // 
            this.cmbChildren.FormattingEnabled = true;
            this.cmbChildren.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30"});
            this.cmbChildren.Location = new System.Drawing.Point(79, 51);
            this.cmbChildren.Name = "cmbChildren";
            this.cmbChildren.Size = new System.Drawing.Size(81, 21);
            this.cmbChildren.TabIndex = 3;
            // 
            // cmbAdults
            // 
            this.cmbAdults.FormattingEnabled = true;
            this.cmbAdults.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30"});
            this.cmbAdults.Location = new System.Drawing.Point(79, 24);
            this.cmbAdults.Name = "cmbAdults";
            this.cmbAdults.Size = new System.Drawing.Size(81, 21);
            this.cmbAdults.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Children";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Adults(18+)";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.LightBlue;
            this.groupBox2.Controls.Add(this.rdbDeLux);
            this.groupBox2.Controls.Add(this.rdbKing);
            this.groupBox2.Controls.Add(this.rdbApartment);
            this.groupBox2.Controls.Add(this.rdbTriple);
            this.groupBox2.Controls.Add(this.rdbDouble);
            this.groupBox2.Controls.Add(this.rdbSingle);
            this.groupBox2.Location = new System.Drawing.Point(12, 127);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(326, 130);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RoomType:";
            // 
            // rdbDeLux
            // 
            this.rdbDeLux.AutoSize = true;
            this.rdbDeLux.Location = new System.Drawing.Point(149, 28);
            this.rdbDeLux.Name = "rdbDeLux";
            this.rdbDeLux.Size = new System.Drawing.Size(109, 17);
            this.rdbDeLux.TabIndex = 5;
            this.rdbDeLux.TabStop = true;
            this.rdbDeLux.Text = "DeLux room 150€";
            this.rdbDeLux.UseVisualStyleBackColor = true;
            // 
            // rdbKing
            // 
            this.rdbKing.AutoSize = true;
            this.rdbKing.Location = new System.Drawing.Point(149, 93);
            this.rdbKing.Name = "rdbKing";
            this.rdbKing.Size = new System.Drawing.Size(174, 17);
            this.rdbKing.TabIndex = 4;
            this.rdbKing.TabStop = true;
            this.rdbKing.Text = "King Apartment (12 guets) 500€";
            this.rdbKing.UseVisualStyleBackColor = true;
            // 
            // rdbApartment
            // 
            this.rdbApartment.AutoSize = true;
            this.rdbApartment.Location = new System.Drawing.Point(149, 60);
            this.rdbApartment.Name = "rdbApartment";
            this.rdbApartment.Size = new System.Drawing.Size(149, 17);
            this.rdbApartment.TabIndex = 3;
            this.rdbApartment.TabStop = true;
            this.rdbApartment.Text = "Apartment (6 guests) 250€";
            this.rdbApartment.UseVisualStyleBackColor = true;
            // 
            // rdbTriple
            // 
            this.rdbTriple.AutoSize = true;
            this.rdbTriple.Location = new System.Drawing.Point(16, 93);
            this.rdbTriple.Name = "rdbTriple";
            this.rdbTriple.Size = new System.Drawing.Size(104, 17);
            this.rdbTriple.TabIndex = 2;
            this.rdbTriple.TabStop = true;
            this.rdbTriple.Text = "Triple room 110€";
            this.rdbTriple.UseVisualStyleBackColor = true;
            // 
            // rdbDouble
            // 
            this.rdbDouble.AutoSize = true;
            this.rdbDouble.Location = new System.Drawing.Point(16, 60);
            this.rdbDouble.Name = "rdbDouble";
            this.rdbDouble.Size = new System.Drawing.Size(106, 17);
            this.rdbDouble.TabIndex = 1;
            this.rdbDouble.TabStop = true;
            this.rdbDouble.Text = "Double room 80€";
            this.rdbDouble.UseVisualStyleBackColor = true;
            // 
            // rdbSingle
            // 
            this.rdbSingle.AutoSize = true;
            this.rdbSingle.Location = new System.Drawing.Point(16, 28);
            this.rdbSingle.Name = "rdbSingle";
            this.rdbSingle.Size = new System.Drawing.Size(101, 17);
            this.rdbSingle.TabIndex = 0;
            this.rdbSingle.TabStop = true;
            this.rdbSingle.Text = "Single room 50€";
            this.rdbSingle.UseVisualStyleBackColor = true;
            // 
            // SeelctPansion
            // 
            this.SeelctPansion.BackColor = System.Drawing.Color.LightBlue;
            this.SeelctPansion.Controls.Add(this.rdbAllInclusive);
            this.SeelctPansion.Controls.Add(this.rdbOnlyBreakfast);
            this.SeelctPansion.Controls.Add(this.rdbHalfPasnion);
            this.SeelctPansion.Controls.Add(this.rdbFullPansion);
            this.SeelctPansion.Location = new System.Drawing.Point(12, 279);
            this.SeelctPansion.Name = "SeelctPansion";
            this.SeelctPansion.Size = new System.Drawing.Size(326, 108);
            this.SeelctPansion.TabIndex = 3;
            this.SeelctPansion.TabStop = false;
            this.SeelctPansion.Text = "Select Pansion:";
            // 
            // rdbAllInclusive
            // 
            this.rdbAllInclusive.AutoSize = true;
            this.rdbAllInclusive.Location = new System.Drawing.Point(36, 38);
            this.rdbAllInclusive.Name = "rdbAllInclusive";
            this.rdbAllInclusive.Size = new System.Drawing.Size(114, 17);
            this.rdbAllInclusive.TabIndex = 3;
            this.rdbAllInclusive.TabStop = true;
            this.rdbAllInclusive.Text = "All-Inclusive - 120€";
            this.rdbAllInclusive.UseVisualStyleBackColor = true;
            // 
            // rdbOnlyBreakfast
            // 
            this.rdbOnlyBreakfast.AutoSize = true;
            this.rdbOnlyBreakfast.Location = new System.Drawing.Point(174, 68);
            this.rdbOnlyBreakfast.Name = "rdbOnlyBreakfast";
            this.rdbOnlyBreakfast.Size = new System.Drawing.Size(121, 17);
            this.rdbOnlyBreakfast.TabIndex = 2;
            this.rdbOnlyBreakfast.TabStop = true;
            this.rdbOnlyBreakfast.Text = "Only Breakfast - 55€";
            this.rdbOnlyBreakfast.UseVisualStyleBackColor = true;
            // 
            // rdbHalfPasnion
            // 
            this.rdbHalfPasnion.AutoSize = true;
            this.rdbHalfPasnion.Location = new System.Drawing.Point(173, 38);
            this.rdbHalfPasnion.Name = "rdbHalfPasnion";
            this.rdbHalfPasnion.Size = new System.Drawing.Size(112, 17);
            this.rdbHalfPasnion.TabIndex = 1;
            this.rdbHalfPasnion.TabStop = true;
            this.rdbHalfPasnion.Text = "Half Pension - 75€";
            this.rdbHalfPasnion.UseVisualStyleBackColor = true;
            // 
            // rdbFullPansion
            // 
            this.rdbFullPansion.AutoSize = true;
            this.rdbFullPansion.Location = new System.Drawing.Point(36, 68);
            this.rdbFullPansion.Name = "rdbFullPansion";
            this.rdbFullPansion.Size = new System.Drawing.Size(108, 17);
            this.rdbFullPansion.TabIndex = 0;
            this.rdbFullPansion.TabStop = true;
            this.rdbFullPansion.Text = "Full pension - 90€";
            this.rdbFullPansion.UseVisualStyleBackColor = true;
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Location = new System.Drawing.Point(381, 18);
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.LightBlue;
            this.label3.Location = new System.Drawing.Point(290, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Select date:";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.LightBlue;
            this.groupBox3.Controls.Add(this.rdbHotel);
            this.groupBox3.Controls.Add(this.rdbCard);
            this.groupBox3.Location = new System.Drawing.Point(381, 192);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(227, 100);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Payment type:";
            // 
            // rdbHotel
            // 
            this.rdbHotel.AutoSize = true;
            this.rdbHotel.Location = new System.Drawing.Point(25, 61);
            this.rdbHotel.Name = "rdbHotel";
            this.rdbHotel.Size = new System.Drawing.Size(137, 17);
            this.rdbHotel.TabIndex = 1;
            this.rdbHotel.TabStop = true;
            this.rdbHotel.Text = "At the hotel (card/cash)";
            this.rdbHotel.UseVisualStyleBackColor = true;
            // 
            // rdbCard
            // 
            this.rdbCard.AutoSize = true;
            this.rdbCard.Location = new System.Drawing.Point(25, 28);
            this.rdbCard.Name = "rdbCard";
            this.rdbCard.Size = new System.Drawing.Size(77, 17);
            this.rdbCard.TabIndex = 0;
            this.rdbCard.TabStop = true;
            this.rdbCard.Text = "Now (card)";
            this.rdbCard.UseVisualStyleBackColor = true;
            // 
            // btnContinue
            // 
            this.btnContinue.BackColor = System.Drawing.Color.SteelBlue;
            this.btnContinue.Location = new System.Drawing.Point(381, 330);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(86, 34);
            this.btnContinue.TabIndex = 7;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = false;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.SteelBlue;
            this.btnCancel.Location = new System.Drawing.Point(522, 330);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(86, 34);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Reservation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSeaGreen;
            this.ClientSize = new System.Drawing.Size(640, 407);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.monthCalendar1);
            this.Controls.Add(this.SeelctPansion);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Reservation";
            this.Text = "Reservation";
            this.Load += new System.EventHandler(this.Reservation_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.SeelctPansion.ResumeLayout(false);
            this.SeelctPansion.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbChildren;
        private System.Windows.Forms.ComboBox cmbAdults;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdbApartment;
        private System.Windows.Forms.RadioButton rdbTriple;
        private System.Windows.Forms.RadioButton rdbDouble;
        private System.Windows.Forms.RadioButton rdbSingle;
        private System.Windows.Forms.RadioButton rdbDeLux;
        private System.Windows.Forms.RadioButton rdbKing;
        private System.Windows.Forms.GroupBox SeelctPansion;
        private System.Windows.Forms.RadioButton rdbFullPansion;
        private System.Windows.Forms.RadioButton rdbOnlyBreakfast;
        private System.Windows.Forms.RadioButton rdbHalfPasnion;
        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdbHotel;
        private System.Windows.Forms.RadioButton rdbCard;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rdbAllInclusive;
    }
}