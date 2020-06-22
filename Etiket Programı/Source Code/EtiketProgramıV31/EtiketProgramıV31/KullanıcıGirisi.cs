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
    public partial class KullanıcıGirisi : Form
    {
        public KullanıcıGirisi()
        {
            InitializeComponent();
        }
        public static string k_adisoyadi;
        public static string yetki;

        public static bool WebRequestTest()
        {
            string url = "http://www.google.com";
            try
            {
                System.Net.WebRequest myRequest = System.Net.WebRequest.Create(url);
                System.Net.WebResponse myResponse = myRequest.GetResponse();
            }
            catch (System.Net.WebException)
            {
                return false;
            }
            return true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                label5.Visible = true;
                label5.Text = "Kullanıcı Adı kısmını boş bırakmayınız";
                label5.ForeColor = Color.Red;
            }
            else if (textBox2.Text == "")
            {
                label5.Visible = true;
                label5.Text = "Şifre kısmını boş bırakmayınız";
                label5.ForeColor = Color.Red;

            }
            else
            {
                MySqlConnection conn = BaglantiKatmanı.baglan();
                MySqlCommand komut = new MySqlCommand("Select * from kullanicilar where kullanici_ad = '" + textBox1.Text + "' AND kullanici_sifre = '" + textBox2.Text + "' ", conn);
                MySqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {
                    k_adisoyadi = dr[1].ToString();
                    yetki = dr[4].ToString();
                    Ana_Ekran frm = new Ana_Ekran();
                    frm.Show();
                    this.Hide();
                    conn.Close();
                    dr.Close();
                    conn.Dispose();
                }
                else
                {
                    label5.Visible = true;
                    label5.Text = "Kullanıcı Adı veya Şifre Yanlış!!";
                    label5.ForeColor = Color.Red;
                }



            }
        }

        private void KullanıcıGirisi_Load(object sender, EventArgs e)
        {
            if (WebRequestTest())
            {
                // Bağlantı varsa hiçbir işlem yapmıyorum .
                BaglantiKatmanı.baglan();
            }
            else
            {
                label5.Text = "Internet Bağlantınızda Sorun Oluştu" + Environment.NewLine + "Lütfen Tekrar Deneyiniz";
                label5.Visible = true;
                label5.ForeColor = Color.Red;
                button3.Enabled = false;
            }
        }
    }
}
