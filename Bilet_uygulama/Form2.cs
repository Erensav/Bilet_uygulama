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

            
            Timer timer = new Timer();
            timer.Interval = 60000; 
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            
            seferlistesi();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            
            DateTime currentDateTime = DateTime.Now;

            
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();

            
            SqlDataAdapter seferAdapter = new SqlDataAdapter("SELECT sefer_no, sefer_kalkis, sefer_varis, sefer_tarih, sefer_saat, sefer_fiyat FROM sefer_bilgileri", baglanti);
            DataTable seferTable = new DataTable();
            seferAdapter.Fill(seferTable);

            
            foreach (DataRow row in seferTable.Rows)
            {
                DateTime seferDateTime = Convert.ToDateTime(row["sefer_tarih"].ToString() + " " + row["sefer_saat"].ToString());

                
                if (seferDateTime <= currentDateTime)
                {
                    int seferNo = Convert.ToInt32(row["sefer_no"].ToString());

                    
                    SqlCommand deleteCommand = new SqlCommand("DELETE FROM sefer_bilgileri WHERE sefer_no = @seferNo", baglanti);
                    deleteCommand.Parameters.AddWithValue("@seferNo", seferNo);
                    deleteCommand.ExecuteNonQuery();
                }
            }

            
            baglanti.Close();

            
            seferlistesi();
            koltukRenklendir();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source = EREN\SQLEXPRESS;Initial Catalog = Yolcu_bilet; Integrated Security = True");

        void seferlistesi()
        {
            SqlDataAdapter sefergoster = new SqlDataAdapter("SELECT sefer_no, sefer_kalkis, sefer_varis, sefer_tarih, sefer_saat, sefer_fiyat FROM sefer_bilgileri", baglanti);
            DataTable dt = new DataTable();
            sefergoster.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
            cmbkydlcins.Items.Clear();
            cmbkydlcins.Items.Add("Kadın");
            cmbkydlcins.Items.Add("Erkek");

            seferlistesi(); 
        }

        private void cmbkydlcins_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            txtrezkoltuk.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
        }







        private void btnkaydol_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (string.IsNullOrEmpty(txtkydlad.Text) || string.IsNullOrEmpty(txtkydlsyad.Text) ||
                    string.IsNullOrEmpty(mskkydltel.Text) || string.IsNullOrEmpty(mskkydltc.Text) ||
                    string.IsNullOrEmpty(cmbkydlcins.Text) || string.IsNullOrEmpty(txtkydlmail.Text))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurunuz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; 
                }

                baglanti.Open();

                
                SqlCommand kontrolSorgu = new SqlCommand("SELECT COUNT(*) FROM Yolcu_bilgiler WHERE yolcu_tc = @tc", baglanti);
                kontrolSorgu.Parameters.AddWithValue("@tc", mskkydltc.Text);

                int kayitSayisi = (int)kontrolSorgu.ExecuteScalar();

                if (kayitSayisi > 0)
                {
                    
                    MessageBox.Show("Bu TC Kimlik Numarasına sahip bir kullanıcı zaten kayıtlı!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    
                    SqlCommand bilgiyolcu = new SqlCommand("INSERT INTO Yolcu_bilgiler (yolcu_ad, yolcu_soyad, yolcu_tel, yolcu_tc, yolcu_cins, yolcu_mail) VALUES (@p1, @p2, @p3, @p4, @p5, @p6)", baglanti);
                    bilgiyolcu.Parameters.AddWithValue("@p1", txtkydlad.Text);
                    bilgiyolcu.Parameters.AddWithValue("@p2", txtkydlsyad.Text);
                    bilgiyolcu.Parameters.AddWithValue("@p3", mskkydltel.Text);
                    bilgiyolcu.Parameters.AddWithValue("@p4", mskkydltc.Text);
                    bilgiyolcu.Parameters.AddWithValue("@p5", cmbkydlcins.Text);
                    bilgiyolcu.Parameters.AddWithValue("@p6", txtkydlmail.Text);

                    bilgiyolcu.ExecuteNonQuery();

                    MessageBox.Show("Yolcu Bilgisi Sisteme Kaydedildi", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata Oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                baglanti.Close();
                
                txtkydlad.Text = "";
                txtkydlsyad.Text = "";
                mskkydltel.Text = "";
                mskkydltc.Text = "";
                cmbkydlcins.Text = "";
                txtkydlmail.Text = "";
            }
        }

        private string GeneratePNRCode()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
        }


        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                
                if (!IsValidTc(mskreztc.Text))
                {
                    MessageBox.Show("Geçerli bir TC Kimlik Numarası giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                
                SqlCommand kontrolSorgu = new SqlCommand("SELECT COUNT(*) FROM Yolcu_bilgiler WHERE yolcu_tc = @tc", baglanti);
                kontrolSorgu.Parameters.AddWithValue("@tc", mskreztc.Text);

                int kayitSayisi = (int)kontrolSorgu.ExecuteScalar();

                if (kayitSayisi > 0)
                {
                    

                    
                    string pnrKodu = GeneratePNRCode();

                    // İlgili sefer ve koltuk için rezervasyon yapıldığını kontrol et
                    SqlCommand koltukKontrolSorgu = new SqlCommand("SELECT COUNT(*) FROM koltuk_durumu WHERE sefer_no = @seferNo AND koltuk_no = @koltukNo AND durum = 1", baglanti);
                    koltukKontrolSorgu.Parameters.AddWithValue("@seferNo", txtrezno.Text);
                    koltukKontrolSorgu.Parameters.AddWithValue("@koltukNo", txtrezkoltuk.Text);

                    int rezervasyonSayisi = (int)koltukKontrolSorgu.ExecuteScalar();

                    if (rezervasyonSayisi > 0)
                    {
                        MessageBox.Show($"Seçtiğiniz koltuk zaten dolu. Lütfen başka bir koltuk seçiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        // Koltuk dolu değilse rezervasyonu yap

                        SqlCommand rezerve = new SqlCommand("INSERT INTO sefer_detay (sefer_no, yolcu_tc, koltuk) VALUES (@1, @2, @3)", baglanti);
                        rezerve.Parameters.AddWithValue("@1", txtrezno.Text);
                        rezerve.Parameters.AddWithValue("@2", mskreztc.Text);
                        rezerve.Parameters.AddWithValue("@3", txtrezkoltuk.Text);
                        rezerve.ExecuteNonQuery();

                        SqlCommand koltuk_durum = new SqlCommand("INSERT INTO koltuk_durumu (sefer_no, koltuk_no, durum) VALUES (@1, @2, @3)", baglanti);
                        koltuk_durum.Parameters.AddWithValue("@1", txtrezno.Text);
                        koltuk_durum.Parameters.AddWithValue("@2", txtrezkoltuk.Text);
                        koltuk_durum.Parameters.AddWithValue("@3", 1);
                        koltuk_durum.ExecuteNonQuery();

                        // PNR kodunu yolcu bilgiler tablosuna kaydet
                        SqlCommand pnrKaydet = new SqlCommand("UPDATE Yolcu_bilgiler SET yolcu_pnr = @pnr WHERE yolcu_tc = @tc", baglanti);
                        pnrKaydet.Parameters.AddWithValue("@pnr", pnrKodu);
                        pnrKaydet.Parameters.AddWithValue("@tc", mskreztc.Text);
                        pnrKaydet.ExecuteNonQuery();

                        MessageBox.Show($"Rezervasyonunuz Başarı ile Sisteme Kaydedildi.\n PNR Kodu: {pnrKodu}", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Bu TC Kimlik Numarasına sahip bir yolcu kayıtlı değil. Lütfen kayıt yapınız.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata Oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                baglanti.Close();
                txtrezno.Text = "";
                mskreztc.Text = "";
                txtrezkoltuk.Text = "";
            }
        }



        
        private bool IsValidTc(string tc)
        {
            
            return tc.Length == 11;
        }




        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            txtrezno.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
        }
        private void koltukRenklendir()
        {
            
            if (baglanti.State == ConnectionState.Closed)
                baglanti.Open();

            
            Button[] koltukButtons = { button1, button2, button3, button4, button5, button6, button7, button8, button9, button10, button11, button12, button13, button14, button15 };

            foreach (Button button in koltukButtons)
            {
                
                string koltukNo = button.Text;

                
                SqlCommand durumSorgu = new SqlCommand("SELECT durum FROM koltuk_durumu WHERE sefer_no = @seferNo AND koltuk_no = @koltukNo", baglanti);
                durumSorgu.Parameters.AddWithValue("@seferNo", txtrezno.Text);
                durumSorgu.Parameters.AddWithValue("@koltukNo", koltukNo);

                int durum = Convert.ToInt32(durumSorgu.ExecuteScalar());

                
                if (durum == 1)
                {
                    
                    button.BackColor = Color.Blue;
                }
                else
                {
                    
                    button.BackColor = Color.Silver;
                }
            }

            
            baglanti.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "1";

            
            if (button1.BackColor == Color.Red)
            {
                button1.BackColor = Color.Silver;
            }
            else
            {
                button1.BackColor = Color.Red;
            }
            koltukRenklendir();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "2";

            
            if (button2.BackColor == Color.Red)
            {
                button2.BackColor = Color.Silver;
            }
            else
            {
                button2.BackColor = Color.Red;
            }
            koltukRenklendir();



        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "3";

            
            if (button3.BackColor == Color.Red)
            {
                button3.BackColor = Color.Silver;
            }
            else
            {
                button3.BackColor = Color.Red;
            }
            koltukRenklendir();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "4";

            
            if (button4.BackColor == Color.Red)
            {
                button4.BackColor = Color.Silver;
            }
            else
            {
                button4.BackColor = Color.Red;
            }
            koltukRenklendir();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "5";

            
            if (button5.BackColor == Color.Red)
            {
                button5.BackColor = Color.Silver;
            }
            else
            {
                button5.BackColor = Color.Red;
            }
            koltukRenklendir();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "6";

            
            if (button6.BackColor == Color.Red)
            {
                button6.BackColor = Color.Silver;
            }
            else
            {
                button6.BackColor = Color.Red;
            }
            koltukRenklendir();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "7";

            
            if (button7.BackColor == Color.Red)
            {
                button7.BackColor = Color.Silver;
            }
            else
            {
                button7.BackColor = Color.Red;
            }
            koltukRenklendir();
        }



        private void button8_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "8";

            
            if (button8.BackColor == Color.Red)
            {
                button8.BackColor = Color.Silver;
            }
            else
            {
                button8.BackColor = Color.Red;
            }
            koltukRenklendir();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "9";

            
            if (button9.BackColor == Color.Red)
            {
                button9.BackColor = Color.Silver;
            }
            else
            {
                button9.BackColor = Color.Red;
            }
            koltukRenklendir();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "10";

            
            if (button10.BackColor == Color.Red)
            {
                button10.BackColor = Color.Silver;
            }
            else
            {
                button10.BackColor = Color.Red;
            }
            koltukRenklendir();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "11";

            
            if (button11.BackColor == Color.Red)
            {
                button11.BackColor = Color.Silver;
            }
            else
            {
                button11.BackColor = Color.Red;
            }
            koltukRenklendir();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "12";

            
            if (button12.BackColor == Color.Red)
            {
                button12.BackColor = Color.Silver;
            }
            else
            {
                button12.BackColor = Color.Red;
            }
            koltukRenklendir();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "13";

            
            if (button13.BackColor == Color.Red)
            {
                button13.BackColor = Color.Silver;
            }
            else
            {
                button13.BackColor = Color.Red;
            }
            koltukRenklendir();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "14";

            if (button14.BackColor == Color.Red)
            {
                button14.BackColor = Color.Silver;
            }
            else
            {
                button14.BackColor = Color.Red;
            }
            koltukRenklendir();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            txtrezkoltuk.Text = "15";

            
            if (button15.BackColor == Color.Red)
            {
                button15.BackColor = Color.Silver;
            }
            else
            {
                button15.BackColor = Color.Red;
            }
            koltukRenklendir();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {

        }
    }
}