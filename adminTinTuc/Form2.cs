using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace adminTinTuc
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void accountManagement_Click(object sender, EventArgs e)
        {
            // Create an instance of Form3
            Form3 form3 = new Form3();

            // Show Form3
            form3.Show();

            // Hide Form2
            this.Hide();

            // Optional: Handle Form3's FormClosed event to show Form2 again if needed
            form3.FormClosed += (s, args) => this.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Subscribe to the FormClosing event
            this.FormClosing += new FormClosingEventHandler(Form2_FormClosing);
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Close the entire application when Form2 is closed
            Application.Exit();
        }

        private void newsManagement_Click(object sender, EventArgs e)
        {

            // Create an instance of Form3
            Form4 form4 = new Form4();

            // Show Form3
            form4.Show();

            // Hide Form2
            this.Hide();

            // Optional: Handle Form3's FormClosed event to show Form2 again if needed
            form4.FormClosed += (s, args) => this.Show();
        }

        private void commentManagement_Click(object sender, EventArgs e)
        {
            // Create an instance of Form3
            Form5 form5 = new Form5();

            // Show Form3
            form5.Show();

            // Hide Form2
            this.Hide();

            // Optional: Handle Form3's FormClosed event to show Form2 again if needed
            form5.FormClosed += (s, args) => this.Show();
        }
    }
}
