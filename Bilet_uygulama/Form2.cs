using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Bilet_uygulama
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source = EREN\SQLEXPRESS; Initial Catalog = Yolcu_bilet; Integrated Security = True");

        void seferlistesi()
        {
            SqlDataAdapter sefergoster = new SqlDataAdapter("Select * From sefer_bilgileri", baglanti);
            DataTable dt = new DataTable();
            sefergoster.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            txtrezkoltuk.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
        }

        

      

        

        private void btnkaydol_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand bilgiyolcu = new SqlCommand("insert into Yolcu_bilgiler (yolcu_ad,yolcu_soyad,yolcu_tel,yolcu_tc,yolcu_cins,yolcu_mail) values (@p1,@p2,@p3,@p4,@p5,@p6)", baglanti);
            bilgiyolcu.Parameters.AddWithValue("@p1", txtkydlad.Text);
            bilgiyolcu.Parameters.AddWithValue("@p2", txtkydlsyad.Text);
            bilgiyolcu.Parameters.AddWithValue("@p3", mskkydltel.Text);
            bilgiyolcu.Parameters.AddWithValue("@p4", mskkydltc.Text);
            bilgiyolcu.Parameters.AddWithValue("@p5", cmbkydlcins.Text);
            bilgiyolcu.Parameters.AddWithValue("@p6", txtkydlmail.Text);
            bilgiyolcu.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Yolcu Bilgisi Sisteme Kaydedildi", "Basarili", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtkydlad.Text = "";
            txtkydlsyad.Text = "";
            mskkydltel.Text = "";
            mskkydltc.Text = "";
            cmbkydlcins.Text = "";
            txtkydlmail.Text = "";
        }

        private void button17_Click(object sender, EventArgs e)
        {
            baglanti.Open();                      

            SqlCommand rezerve = new SqlCommand("insert into sefer_detay (sefer_no,yolcu_tc,koltuk) values (@1,@2,@3)", baglanti);
            rezerve.Parameters.AddWithValue("@1", txtrezno.Text);
            rezerve.Parameters.AddWithValue("@2", mskreztc.Text);
            rezerve.Parameters.AddWithValue("@3", txtrezkoltuk.Text);
            rezerve.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Rezervasyonunuz Basari ile Sisteme Kaydedildi", "Basarili", MessageBoxButtons.OK, MessageBoxIcon.Information);         
            txtrezno.Text = "";
            mskreztc.Text = "";
            txtrezkoltuk.Text = "";
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            seferlistesi();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            txtrezno.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "1";

            // Butonun rengini kontrol et
            if (button1.BackColor == Color.Red)
            {
                button1.BackColor = Color.Silver;
            }
            else
            {
                button1.BackColor = Color.Red;
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "2";

            // Butonun rengini kontrol et
            if (button2.BackColor == Color.Red)
            {
                button2.BackColor = Color.Silver;
            }
            else
            {
                button2.BackColor = Color.Red;
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "3";

            // Butonun rengini kontrol et
            if (button3.BackColor == Color.Red)
            {
                button3.BackColor = Color.Silver;
            }
            else
            {
                button3.BackColor = Color.Red;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "4";

            // Butonun rengini kontrol et
            if (button4.BackColor == Color.Red)
            {
                button4.BackColor = Color.Silver;
            }
            else
            {
                button4.BackColor = Color.Red;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "5";

            // Butonun rengini kontrol et
            if (button5.BackColor == Color.Red)
            {
                button5.BackColor = Color.Silver;
            }
            else
            {
                button5.BackColor = Color.Red;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "6";

            // Butonun rengini kontrol et
            if (button6.BackColor == Color.Red)
            {
                button6.BackColor = Color.Silver;
            }
            else
            {
                button6.BackColor = Color.Red;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "7";

            // Butonun rengini kontrol et
            if (button7.BackColor == Color.Red)
            {
                button7.BackColor = Color.Silver;
            }
            else
            {
                button7.BackColor = Color.Red;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "8";

            // Butonun rengini kontrol et
            if (button8.BackColor == Color.Red)
            {
                button8.BackColor = Color.Silver;
            }
            else
            {
                button8.BackColor = Color.Red;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "9";

            // Butonun rengini kontrol et
            if (button9.BackColor == Color.Red)
            {
                button9.BackColor = Color.Silver;
            }
            else
            {
                button9.BackColor = Color.Red;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "10";

            // Butonun rengini kontrol et
            if (button10.BackColor == Color.Red)
            {
                button10.BackColor = Color.Silver;
            }
            else
            {
                button10.BackColor = Color.Red;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "11";

            // Butonun rengini kontrol et
            if (button11.BackColor == Color.Red)
            {
                button11.BackColor = Color.Silver;
            }
            else
            {
                button11.BackColor = Color.Red;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "12";

            // Butonun rengini kontrol et
            if (button12.BackColor == Color.Red)
            {
                button12.BackColor = Color.Silver;
            }
            else
            {
                button12.BackColor = Color.Red;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "13";

            // Butonun rengini kontrol et
            if (button13.BackColor == Color.Red)
            {
                button13.BackColor = Color.Silver;
            }
            else
            {
                button13.BackColor = Color.Red;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "14";

            // Butonun rengini kontrol et
            if (button14.BackColor == Color.Red)
            {
                button14.BackColor = Color.Silver;
            }
            else
            {
                button14.BackColor = Color.Red;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "15";

            // Butonun rengini kontrol et
            if (button15.BackColor == Color.Red)
            {
                button15.BackColor = Color.Silver;
            }
            else
            {
                button15.BackColor = Color.Red;
            }
        }
    }
}
