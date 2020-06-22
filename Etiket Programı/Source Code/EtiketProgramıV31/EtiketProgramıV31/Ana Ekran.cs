using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EtiketProgramıV31
{
    public partial class Ana_Ekran : Form
    {
        public Ana_Ekran()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Ana_Ekran_Load(object sender, EventArgs e)
        {
            label1.Text = "Hoşgeldiniz : " + " " + KullanıcıGirisi.k_adisoyadi;
            if (KullanıcıGirisi.yetki == "Kullanici")
            {
                button4.Visible = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            KullaniciAyarlari ka = new KullaniciAyarlari();
            ka.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }
    }
}
