using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bilet_uygulama
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "admin" && textBox2.Text == "12" )
            {
                Form4 yeni = new Form4();
                yeni.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Gecersiz Kullanici Girisi", "Hatali Giris", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
            }
        }
    }
}
