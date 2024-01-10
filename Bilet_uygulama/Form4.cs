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

        private bool KaptanNoVarMi(string kaptanNo)
        {
            using (SqlConnection baglanti = new SqlConnection(@"Data Source = EREN\SQLEXPRESS; Initial Catalog = Yolcu_bilet; Integrated Security = True"))
            {
                baglanti.Open();

                SqlCommand kontrolKomutu = new SqlCommand("SELECT COUNT(*) FROM kaptan_bilgi WHERE kaptan_no = @kaptanNo", baglanti);
                kontrolKomutu.Parameters.AddWithValue("@kaptanNo", kaptanNo);

                int kaptanSayisi = (int)kontrolKomutu.ExecuteScalar();

                return kaptanSayisi > 0;
            }
        }

        private bool KaptanAdVarMi(string kaptanAd)
        {
            using (SqlConnection baglanti = new SqlConnection(@"Data Source = EREN\SQLEXPRESS; Initial Catalog = Yolcu_bilet; Integrated Security = True"))
            {
                baglanti.Open();

                SqlCommand kontrolKomutu = new SqlCommand("SELECT COUNT(*) FROM kaptan_bilgi WHERE kaptanad_soyad = @kaptanAd", baglanti);
                kontrolKomutu.Parameters.AddWithValue("@kaptanAd", kaptanAd);

                int kaptanSayisi = (int)kontrolKomutu.ExecuteScalar();

                return kaptanSayisi > 0;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // Girişlerin boş olup olmadığını kontrol et
            if (string.IsNullOrWhiteSpace(txtkptnad.Text) || string.IsNullOrWhiteSpace(mskkptntel.Text) || string.IsNullOrWhiteSpace(txtkptnno.Text))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Boşsa işlemi sonlandır
            }

            // Kaptan numarasının 4 haneli olup olmadığını kontrol et
            if (txtkptnno.Text.Length != 4)
            {
                MessageBox.Show("Kaptan numarası 4 haneli olmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kaptan numarasının daha önce kullanılıp kullanılmadığını kontrol et
            if (KaptanNoVarMi(txtkptnno.Text))
            {
                MessageBox.Show("Bu kaptan numarası zaten kullanılmış.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Kaptan numarası varsa işlemi sonlandır
            }

            // Kaptan adının daha önce kullanılıp kullanılmadığını kontrol et
            if (KaptanAdVarMi(txtkptnad.Text))
            {
                MessageBox.Show("Bu kaptan adı zaten kullanılmış.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Kaptan adı varsa işlemi sonlandır
            }

            using (SqlConnection baglanti = new SqlConnection(@"Data Source = EREN\SQLEXPRESS; Initial Catalog = Yolcu_bilet; Integrated Security = True"))
            {
                baglanti.Open();

                SqlCommand kaptanbilgi = new SqlCommand("insert into kaptan_bilgi (kaptanad_soyad, kaptan_tel, kaptan_no) values (@1, @2, @3)", baglanti);
                kaptanbilgi.Parameters.AddWithValue("@1", txtkptnad.Text);
                kaptanbilgi.Parameters.AddWithValue("@2", mskkptntel.Text);
                kaptanbilgi.Parameters.AddWithValue("@3", txtkptnno.Text);

                kaptanbilgi.ExecuteNonQuery();

                MessageBox.Show("Kaptan Bilgisi Sisteme Kaydedildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Alanları temizle
                txtkptnad.Text = "";
                mskkptntel.Text = "";
                txtkptnno.Text = "";
            }
        }








        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            txtsfrno.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
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

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];

                if (selectedRow.Index >= 0 && selectedRow.Index < dataGridView2.Rows.Count)
                {
                    string secilenKaptanNo = selectedRow.Cells["kaptan_no"].Value.ToString();

                    DialogResult result = MessageBox.Show("Seçili kaptanı silmek istediğinize emin misiniz?", "Kaptan Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Seçilen kaptanı veritabanından sil
                        using (SqlConnection baglanti = new SqlConnection(@"Data Source=EREN\SQLEXPRESS;Initial Catalog=Yolcu_bilet;Integrated Security=True"))
                        {
                            baglanti.Open();

                            SqlCommand kaptanSilKomutu = new SqlCommand("DELETE FROM kaptan_bilgi WHERE kaptan_no = @kaptanNo", baglanti);
                            kaptanSilKomutu.Parameters.AddWithValue("@kaptanNo", secilenKaptanNo);

                            int affectedRows = kaptanSilKomutu.ExecuteNonQuery();

                            if (affectedRows > 0)
                            {
                                MessageBox.Show("Kaptan başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // DataGridView'i güncelle
                                kaptanlistesi();

                                txtkptnad.Text = "";
                                mskkptntel.Text = "";
                                txtkptnno.Text = "";
                            }
                            else
                            {
                                MessageBox.Show("Kaptan silinirken bir hata oluştu. Silinen satır sayısı: 0", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Seçilen satır DataGridView2'ye ait değil.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir satır seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
