using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace EtiketProgramıV31
{
    public partial class KullaniciAyarlari : Form
    {
        public KullaniciAyarlari()
        {
            InitializeComponent();
        }

        private void KullaniciAyarlari_Load(object sender, EventArgs e)
        {
            kullaniciListele();
            dataGridView1.ClearSelection();
        }

        private void kullaniciListele()
        {
            DataTable table = new DataTable();
            MySqlConnection conn = BaglantiKatmanı.baglan();
            MySqlDataAdapter adptr = new MySqlDataAdapter("select * from kullanicilar ", conn);
            adptr.Fill(table);
            conn.Close();
            adptr.Dispose();
            dataGridView1.DataSource = table;
        }
        private void kullaniciGuncelle()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Ad Soyad Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Kullanıcı Adı Kısmı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else if (textBox3.Text == "")
            {
                MessageBox.Show("Şifre Kısmı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else if (comboBox1.Text == "")
            {
                MessageBox.Show("Yetki Kısmı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else
            {
                using (MySqlConnection conn = BaglantiKatmanı.baglan())
                {
                    MySqlCommand komut = new MySqlCommand("update kullanicilar set k_adsoyad=@kullaniciAdSoyad, kullanici_ad=@kullaniciAd, kullanici_sifre=@kullaniciSifre, yetki=@kullaniciYetki where kullaniciID='" + dataGridView1.SelectedRows[0].Cells[0].Value + "'", conn);
                    komut.Parameters.AddWithValue("@kullaniciAdSoyad", textBox1.Text);
                    komut.Parameters.AddWithValue("@kullaniciAd", textBox2.Text);
                    komut.Parameters.AddWithValue("@kullaniciSifre", textBox3.Text);
                    komut.Parameters.AddWithValue("@kullaniciYetki", comboBox1.Text);

                    komut.ExecuteNonQuery();
                    conn.Close();
                    kullaniciListele();
                    MessageBox.Show("Güncelleme işlemi başarılı oldu", "Başarılı!");

                }

            }
        }
        private void kullaniciSil()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Ad Soyad Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Kullanıcı Adı Kısmı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else if (textBox3.Text == "")
            {
                MessageBox.Show("Şifre Kısmı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else if (comboBox1.Text == "")
            {
                MessageBox.Show("Yetki Kısmı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show("Silmek istediğinize emin misiniz ?", "Uyarı!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    using (MySqlConnection conn = BaglantiKatmanı.baglan())
                    {
                        MySqlCommand komut = new MySqlCommand("delete from kullanicilar where kullaniciID='" + dataGridView1.SelectedRows[0].Cells[0].Value + "' ", conn);
                        komut.ExecuteNonQuery();
                        conn.Close();
                        kullaniciListele();
                        MessageBox.Show("Silme işlemi başarılı oldu", "Başarılı!");
                    }
                }
            }
           
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Ad Soyad Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Kullanıcı Adı Kısmı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else if (textBox3.Text == "")
            {
                MessageBox.Show("Şifre Kısmı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else if (comboBox1.Text == "")
            {
                MessageBox.Show("Yetki Kısmı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else
            {
                MySqlConnection conn2 = BaglantiKatmanı.baglan();
                MySqlCommand komut2 = new MySqlCommand("insert into kullanicilar (k_adsoyad,kullanici_ad,kullanici_sifre,yetki) values (@k_adsoyad,@kullanici_ad,@kullanici_sifre,@yetki)", conn2);
                komut2.Parameters.Clear();
                komut2.Parameters.AddWithValue("@k_adsoyad", textBox1.Text);
                komut2.Parameters.AddWithValue("@kullanici_ad", textBox2.Text);
                komut2.Parameters.AddWithValue("@kullanici_sifre", textBox3.Text);
                komut2.Parameters.AddWithValue("@yetki", comboBox1.Text);
                komut2.ExecuteNonQuery();
                conn2.Close();
                MessageBox.Show("Kullanici Başarıyla Kaydedildi","Başarılı");

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            kullaniciGuncelle();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            kullaniciSil();
        }
    }
}
