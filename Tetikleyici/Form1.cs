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

namespace Tetikleyici
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=MONSTER;Initial Catalog=test;Integrated Security=True");
        void listele()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from Tbl_kitaplar", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            baglanti.Close();
        }
        void sayac()
        {   //sistemdeki kitapların toplamı=sayac
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from Tbl_Sayac", baglanti);
            SqlDataReader dr = komut.ExecuteReader();
            while(dr.Read())
            {
                lbl_Kitap_Adet.Text = dr[0].ToString();
            }
            baglanti.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {   
            listele();
            sayac();
        }

        private void btn_Ekle_Click(object sender, EventArgs e)
        { //Kitap ekleme
            try
            {   if (string.IsNullOrWhiteSpace(txt_Ad.Text) || string.IsNullOrWhiteSpace(txt_Yazar.Text) || string.IsNullOrWhiteSpace(txt_Sayfa.Text) || string.IsNullOrWhiteSpace(txt_Yayınevi.Text) || string.IsNullOrWhiteSpace(txt_Tur.Text))
                    //nul veya boşsa aşadakini yap
                {
                    MessageBox.Show("Lütfen Tüm Alanları doldurun", "uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("INSERT INTO Tbl_kitaplar (AD,YAZAR,SAYFA,YAYINEVİ,TUR) values (@p1,@p2,@p3,@p4,@p5)", baglanti);
                    komut.Parameters.AddWithValue("@p1", txt_Ad.Text);
                    komut.Parameters.AddWithValue("@p2", txt_Yazar.Text);
                    komut.Parameters.AddWithValue("@p3", txt_Sayfa.Text);
                    komut.Parameters.AddWithValue("@p4", txt_Yayınevi.Text);
                    komut.Parameters.AddWithValue("@p5", txt_Tur.Text);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Kitap Sisteme Başarıyla Eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex )
            {
                MessageBox.Show("Kitap Eklenmedi"+ex.Message, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                baglanti.Close();
            }
            listele();
            sayac();
        }

        private void btn_Sil_Click(object sender, EventArgs e)
        {  //Kitap silme
            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("delete from Tbl_kitaplar  where ID=@p1", baglanti);
                komut.Parameters.AddWithValue("@p1", txt_ID.Text);
                komut.ExecuteNonQuery();
                MessageBox.Show("Kitap Silindi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kitap Silm İşlemi Başarısız" + ex.Message, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                baglanti.Close();
            }
            listele();
            sayac();
        }

        private void btn_Güncelle_Click(object sender, EventArgs e)
        {  //Kitap Güncelleme
            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("update Tbl_kitaplar set AD=@p1,YAZAR=@p2,SAYFA=@p3,YAYINEVİ=@p4,TUR=@p5 where ID=@p6", baglanti);
                komut.Parameters.AddWithValue("@p1", txt_Ad.Text);
                komut.Parameters.AddWithValue("@p2", txt_Yazar.Text);
                komut.Parameters.AddWithValue("@p3", txt_Sayfa.Text);
                komut.Parameters.AddWithValue("@p4", txt_Yayınevi.Text);
                komut.Parameters.AddWithValue("@p5", txt_Tur.Text);
                komut.Parameters.AddWithValue("@p6", txt_ID.Text);
                komut.ExecuteNonQuery();
                MessageBox.Show("Kitap Güncellendi" , "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kitap Güncellenemedi" + ex.Message, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                baglanti.Close();
            }
            listele();
            sayac();
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {   //Çift Tıklama Olayı
            txt_ID.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txt_Ad.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txt_Yazar.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txt_Sayfa.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txt_Yayınevi.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txt_Tur.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
        }
    }
}
