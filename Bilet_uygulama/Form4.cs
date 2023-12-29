using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bilet_uygulama
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            InitializeTimer();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source = EREN\SQLEXPRESS; Initial Catalog = Yolcu_bilet; Integrated Security = True");

        private void Form4_Load(object sender, EventArgs e)
        {
            seferlistesi();
            kaptanlistesi();
        }
        void seferlistesi()
        {
            SqlDataAdapter sefergoster = new SqlDataAdapter("Select * From sefer_bilgileri", baglanti);
            DataTable dt = new DataTable();
            sefergoster.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        void kaptanlistesi()
        {
            SqlDataAdapter kaptangoster = new SqlDataAdapter("Select * From kaptan_bilgi", baglanti);
            DataTable dt = new DataTable();
            kaptangoster.Fill(dt);
            dataGridView2.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand seferbilgi = new SqlCommand("insert into sefer_bilgileri (sefer_kalkis,sefer_varis,sefer_tarih,sefer_saat,sefer_kaptan,sefer_fiyat) values (@1,@2,@3,@4,@5,@6)", baglanti);
            seferbilgi.Parameters.AddWithValue("@1", txtkls.Text);
            seferbilgi.Parameters.AddWithValue("@2", txtvrs.Text);
            seferbilgi.Parameters.AddWithValue("@3", msktarih.Text);
            seferbilgi.Parameters.AddWithValue("@4", msksaat.Text);
            seferbilgi.Parameters.AddWithValue("@5", mskkaptan.Text);
            seferbilgi.Parameters.AddWithValue("@6", txtfyt.Text);
            seferbilgi.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Sefer Bilgisi Sisteme Kaydedildi", "Sefer Kaydi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtkls.Text = "";
            txtvrs.Text = "";
            msktarih.Text = "";
            msksaat.Text = "";
            mskkaptan.Text = "";
            txtfyt.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand kaptanbilgi = new SqlCommand("insert into kaptan_bilgi (kaptanad_soyad,kaptan_tel,kaptan_no) values (@1,@2,@3)", baglanti);
            kaptanbilgi.Parameters.AddWithValue("@1", txtkptnad.Text);
            kaptanbilgi.Parameters.AddWithValue("@2", mskkptntel.Text);
            kaptanbilgi.Parameters.AddWithValue("@3", txtkptnno.Text);
            kaptanbilgi.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kaptan Bilgisi Sisteme Kaydedildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtkptnad.Text = "";
            mskkptntel.Text = "";
            txtkptnno.Text = "";

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            txtsfrno.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilenktpn = dataGridView1.SelectedCells[0].RowIndex;
            txtkptnno.Text = dataGridView1.Rows[secilenktpn].Cells[0].Value.ToString();
        }
        private void InitializeTimer()
        {
            // Timer'ı başlat
            timer1 = new Timer();
            timer1.Interval = 1000; // Her bir saniyede bir (1000 milisaniye)
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            seferlistesi();
            kaptanlistesi();
        }
    }
}
