using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Bilet_uygulama
{
    public partial class Form5 : Form
    {
        SqlConnection baglanti = new SqlConnection(@"Data Source = EREN\SQLEXPRESS;Initial Catalog = Yolcu_bilet; Integrated Security = True");

        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                
                SqlCommand sorgu = new SqlCommand("SELECT * FROM Yolcu_bilgiler WHERE yolcu_pnr = @pnr", baglanti);
                sorgu.Parameters.AddWithValue("@pnr", txtPNR.Text);

                SqlDataReader reader = sorgu.ExecuteReader();

                
                if (reader.Read())
                {
                    
                    string ad = reader["yolcu_ad"].ToString();
                    string soyad = reader["yolcu_soyad"].ToString();
                    string tc = reader["yolcu_tc"].ToString();
                    string cins = reader["yolcu_cins"].ToString();
                    string mail = reader["yolcu_mail"].ToString();

                    reader.Close();

                   
                    SqlCommand koltukSorgu = new SqlCommand("SELECT koltuk FROM sefer_detay WHERE yolcu_tc = @tc", baglanti);
                    koltukSorgu.Parameters.AddWithValue("@tc", tc);

                    int koltukNo = Convert.ToInt32(koltukSorgu.ExecuteScalar());
                    



                    
                    SqlCommand seferSorgu = new SqlCommand("SELECT sefer_bilgileri.sefer_kalkis,sefer_bilgileri.sefer_saat, sefer_bilgileri.sefer_varis, sefer_bilgileri.sefer_tarih FROM sefer_bilgileri INNER JOIN sefer_detay ON sefer_bilgileri.sefer_no = sefer_detay.sefer_no WHERE sefer_detay.yolcu_tc = @tc", baglanti);
                    seferSorgu.Parameters.AddWithValue("@tc", tc);

                    SqlDataReader seferReader = seferSorgu.ExecuteReader();


                    
                    
                    if (seferReader.Read())
                    {
                        string kalkis = seferReader["sefer_kalkis"].ToString();
                        string varis = seferReader["sefer_varis"].ToString();
                        string saat = seferReader["sefer_saat"].ToString();
                        string tarih = seferReader["sefer_tarih"].ToString();

                        
                        MessageBox.Show($"Ad: {ad}\nSoyad: {soyad}\nTC: {tc}\nCinsiyet: {cins}\nE-posta: {mail}\nKalkış: {kalkis}\nVarış: {varis}\nKoltuk No: {koltukNo}\nSaat: {saat}\nTarih: {tarih}", "Yolcu Bilgileri", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    seferReader.Close();
                }
                else
                {
                    MessageBox.Show("Belirtilen PNR koduna sahip bir yolcu bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata Oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                baglanti.Close();
            }
        }

    }
}
