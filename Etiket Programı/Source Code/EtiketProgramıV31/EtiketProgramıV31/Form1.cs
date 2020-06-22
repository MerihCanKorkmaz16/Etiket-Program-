using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Threading;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.IO;
using Word = Microsoft.Office.Interop.Word;

namespace EtiketProgramıV31
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static int Grupid;
        private static byte etiketSayisi= 0;
        private static byte ilkEtiketSay = 0;
        static string ExeDosyaYolu = Application.StartupPath.ToString();
        static string path = ExeDosyaYolu + "\\Dosya\\Etiket.docx";
        static string path2 = ExeDosyaYolu + "\\Dosya\\Etiket2.docx";
        void DökümanHazırla()
        {
            progressBar1.Value = 20;
            if (!File.Exists(path))
            {
                MessageBox.Show("Dosya Yok");
            }
            else
            {
                var word = new Word.Application();
                var document = word.Documents.Add(path);
                progressBar1.Value = 50;
                foreach (RichTextBox item in tableLayoutPanel3.Controls)
                {
                    String [] richSayi;
                    richSayi = item.Name.Split('_');
                    
                    
                    if (Convert.ToByte(richSayi[1]) >= ilkEtiketSay )
                    {
                        item.Text = item.Text.Replace("\n", "-");

                        document.Variables["etiket"+ richSayi[1]].Value = item.Text;
                    }else
                        document.Variables["etiket" + richSayi[1]].Value = " ";

                }

                document.Fields.Update();
                FileInfo fi = new FileInfo(path2);
                bool kilitliMi = DosyaKilitliMi(fi);
                if (kilitliMi == true)
                {
                    MessageBox.Show("Word Dosyası Zaten açık kapatmayı unutmayınız");
                }
                else
                {
                    document.SaveAs2(path2);
                    progressBar1.Value = 100;
                    MessageBox.Show("Belgeniz Başarıyla Hazırlandı.", "Bilgilendirme Penceresi");
                    word.Visible = true;
                }
               
            }

        }
        public static bool DosyaKilitliMi(FileInfo dosya)
        {
            FileStream stream = null;
            try
            {
                stream = dosya.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                // Dosya kilitli.
                return true;
            }
            finally
            {
                // Eğer açılabildiyse stream nesnesini serbest bırak.
                if (stream != null)
                    stream.Close();
            }
            // Dosya kilitli değil.
            return false;
        }

        //////////////////// Veritabanı İşlemleri //////////////////////////////////////////////
        async void VeritabanıVeriCekmeİslemleri()
        {
            await Task.Run(() =>
            {
                MySqlConnection conn = BaglantiKatmanı.baglan();
                MySqlCommand komut = new MySqlCommand();
                komut.CommandText = "SELECT * FROM kurumlar ORDER BY KurumAdi";
                komut.Connection = conn;
                komut.CommandType = CommandType.Text;


                MySqlDataReader dr;
                dr = komut.ExecuteReader();

                while (dr.Read())
                {
                    KurumAdınaGöreAraComboBax.Items.Add(dr["KurumAdi"]);
                    comboBox4.Items.Add(dr["KurumAdi"]);
                    Invoke((Action)(() =>
                    {
                        KurumAdınaGöreAraComboBax.AutoCompleteSource = AutoCompleteSource.ListItems;
                        KurumAdınaGöreAraComboBax.AutoCompleteMode = AutoCompleteMode.Suggest;
                        comboBox4.AutoCompleteSource = AutoCompleteSource.ListItems;
                        comboBox4.AutoCompleteMode = AutoCompleteMode.Suggest;


                    }));

                }
                conn.Close();

                conn.Open();

                komut.CommandText = "SELECT * FROM gruplar ORDER BY GrupAdi";
                komut.Connection = conn;
                komut.CommandType = CommandType.Text;
                dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    comboBox1.Items.Add(dr["GrupAdi"]);
                    GrupAdınaGöreAraComboBax.Items.Add(dr["GrupAdi"]);
                    Invoke((Action)(() =>
                    {
                        GrupAdınaGöreAraComboBax.AutoCompleteSource = AutoCompleteSource.ListItems;
                        GrupAdınaGöreAraComboBax.AutoCompleteMode = AutoCompleteMode.Suggest;
                        comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
                        comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
                    }));

                }
                conn.Close();


            });

        }
        void VeritabanıGrupİdÇekme()
        {

            MySqlConnection conn = BaglantiKatmanı.baglan();
            MySqlCommand komut = new MySqlCommand("Select GrupID from gruplar where GrupAdi = '" + comboBox1.SelectedItem.ToString() + "'", conn);
            MySqlDataReader dr = komut.ExecuteReader();
            if (dr.Read())
            {
                Grupid = Convert.ToInt32(dr[0]);
                conn.Close();
                dr.Close();
                conn.Dispose();
            }

        }
        void VeritabanıKurumEkleme()
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Kurum Adı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("Adres Kısmı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else if (textBox3.Text == "")
            {
                MessageBox.Show("İl Kısmı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else if (textBox4.Text == "")
            {
                MessageBox.Show("İlçe Kısmı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else
            {

                MySqlConnection conn = BaglantiKatmanı.baglan();
                MySqlCommand komut = new MySqlCommand("Select KurumAdi from kurumlar where KurumAdi = '" + textBox1.Text + "'", conn);
                MySqlDataReader dr = komut.ExecuteReader();

                if (dr.Read())
                {

                    MessageBox.Show("Kurum Adı sistemde  mevcut kontrol ediniz.", "UYARI! ");
                    conn.Close();
                    dr.Close();
                    conn.Dispose();
                }
                else
                {
                    MySqlConnection conn2 = BaglantiKatmanı.baglan();
                    MySqlCommand komut2 = new MySqlCommand("insert into kurumlar (KurumAdi,KurumAdres,Kurum_il,Kurum_ilce) values (@KurumAdi,@KurumAdres,@Kurum_il,@Kurum_ilce)", conn2);
                    komut2.Parameters.Clear();
                    komut2.Parameters.AddWithValue("@KurumAdi", textBox1.Text);
                    komut2.Parameters.AddWithValue("@KurumAdres", textBox2.Text);
                    komut2.Parameters.AddWithValue("@Kurum_il", textBox3.Text);
                    komut2.Parameters.AddWithValue("@Kurum_ilce", textBox4.Text);

                    komut2.ExecuteNonQuery();
                    conn2.Close();
                    if (comboBox4.Items.IndexOf(textBox1.Text) != -1) MessageBox.Show("Eklemek İstediğiniz Kurum Adı Zaten Mevcut");
                    else if (KurumAdınaGöreAraComboBax.Items.IndexOf(textBox1.Text) != -1) MessageBox.Show("Eklemek İstediğiniz Kurum Adı Zaten Mevcut");
                    else
                    {
                        comboBox4.Items.Add(textBox1.Text);
                        KurumAdınaGöreAraComboBax.Items.Add(textBox1.Text);
                        MessageBox.Show("Kayıt İşlemi Tamamlandı!", "Bilgilendirme Penceresi");

                    }
                    foreach (Control item in YeniAdresGroupBox.Controls)
                    {
                        if (item is TextBox)
                        {
                            item.Text = String.Empty;

                        }
                    }

                }

            }

        }
        void VeritabanıGrupEkleme()
        {
            if (textBox5.Text == "")
            {
                MessageBox.Show("Grup Adı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }

            else
            {
                MySqlConnection conn = BaglantiKatmanı.baglan();
                MySqlCommand komut = new MySqlCommand("Select GrupAdi from gruplar where GrupAdi = '" + textBox5.Text + "'");
                komut.Connection = conn;
                MySqlDataReader dr = komut.ExecuteReader();
                if (dr.Read())
                {

                    MessageBox.Show("Grup Adı sistemde  mevcut kontrol ediniz.", "UYARI! ");
                    conn.Close();
                    dr.Close();
                    conn.Dispose();
                }
                else
                {

                    MySqlConnection conn2 = BaglantiKatmanı.baglan();

                    MySqlCommand komut2 = new MySqlCommand("insert into gruplar (GrupAdi) values (@GrupAdi)", conn2);
                    komut2.Parameters.Clear();
                    komut2.Parameters.AddWithValue("@GrupAdi", textBox5.Text);

                    komut2.ExecuteNonQuery();
                    conn2.Close();
                    if (GrupAdınaGöreAraComboBax.Items.IndexOf(textBox5.Text) != -1) MessageBox.Show("Eklemek İstediğiniz Grup Adı Zaten Mevcut");
                    else if (comboBox1.Items.IndexOf(textBox5.Text) != -1) MessageBox.Show("Eklemek İstediğiniz Grup Adı Zaten Mevcut");
                    else
                    {
                        GrupAdınaGöreAraComboBax.Items.Add(textBox5.Text);
                        comboBox1.Items.Add(textBox5.Text);
                        MessageBox.Show("Kayıt İşlemi Tamamlandı!", "Bilgilendirme Penceresi");

                    }



                }




            }
        }
        void KurumAdlarınıGruplaraEkleme()
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Grup Adı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else if (comboBox4.Text == "")
            {
                MessageBox.Show("Kurum Adı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else
            {
                using (MySqlConnection connn = BaglantiKatmanı.baglan())
                {

                    MySqlCommand komut = new MySqlCommand("update kurumlar set GrupID=@GrupID where KurumAdi= '" + comboBox4.Text + "'");
                    komut.Parameters.AddWithValue("@GrupID", Grupid);
                    komut.Connection = connn;
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Kayıt İşlemi Başarılı!", "Başarılı ! ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    connn.Close();

                }

            }
        }
        void GrupAdınaGöreAramaYapma()
        {
            GrupAdınaGöreAraComboBax2.Items.Clear();
            GrupAdınaGöreAraComboBax2.Text = string.Empty;
            using (MySqlConnection baglanti = BaglantiKatmanı.baglan())
            {
                MySqlCommand komut = new MySqlCommand();
                komut.CommandText = "SELECT * FROM kurumlar where GrupID = (select GrupID from gruplar where GrupAdi = '" + GrupAdınaGöreAraComboBax.Text + "') ";
                komut.Connection = baglanti;
                komut.CommandType = CommandType.Text;

                MySqlDataReader dr;
                dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    GrupAdınaGöreAraComboBax2.Items.Add(dr["KurumAdi"]);
                    GrupAdınaGöreAraComboBax2.Visible = true;
                }
                baglanti.Close();
            }
        }
        void KurumBilgileriÇekme()
        {
            using (MySqlConnection baglanti = BaglantiKatmanı.baglan())
            {
                MySqlCommand komut = new MySqlCommand();
                komut.CommandText = "SELECT * FROM kurumlar where KurumAdi = '" + GrupAdınaGöreAraComboBax2.Text + "' or KurumAdi = '" + KurumAdınaGöreAraComboBax.Text + "' ";
                komut.Connection = baglanti;
                komut.CommandType = CommandType.Text;

                MySqlDataReader dr;
                dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    KurumAdıtext.Text = dr["KurumAdi"].ToString();
                    AdresText.Text = dr["KurumAdres"].ToString();
                    İlText.Text = dr["Kurum_il"].ToString();
                    İlceText.Text = dr["Kurum_ilce"].ToString();
                    
                }
                baglanti.Close();
            }
        }
        void KurumBilgileriGüncelle()
        {
            if (KurumAdıtext.Text == "")
            {
                MessageBox.Show("Kurum Adı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else if (AdresText.Text == "")
            {
                MessageBox.Show("Adres Alanı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else if (İlceText.Text == "")
            {
                MessageBox.Show("İlçe Alanı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else if (İlText.Text == "")
            {
                MessageBox.Show("İl Alanı Boş Bırakılamaz", "Bilgilendirme Penceresi");
                return;
            }
            else
            {
                using (MySqlConnection connn = BaglantiKatmanı.baglan())
                {

                    MySqlCommand komut = new MySqlCommand("update kurumlar set KurumAdres = @KurumAdres , KurumAdi = @KurumAdi , Kurum_il = @Kurum_il, Kurum_ilce = @Kurum_ilce where KurumAdi= '" + KurumAdıtext.Text + "'");
                    komut.Parameters.AddWithValue("@KurumAdi", KurumAdıtext.Text);
                    komut.Parameters.AddWithValue("@KurumAdres", AdresText.Text);
                    komut.Parameters.AddWithValue("@Kurum_il", İlText.Text);
                    komut.Parameters.AddWithValue("@Kurum_ilce", İlceText.Text);
                    komut.Connection = connn;
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Bilgiler Başarıyla Güncellendi!", "Başarılı ! ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    connn.Close();
                    checkBox5.Enabled = true;
                    checkBox5.Checked = false;
                }

            }
        }
        //////////////////// Veritabanı İşlemleri //////////////////////////////////////////////

        void richTextBilgiEkleme(){

            string richTxt = "etiketRichText_" + etiketSayisi.ToString();
            foreach (RichTextBox item in tableLayoutPanel3.Controls)
            {
                if (item.Name == richTxt)
                {
                    
                    item.Text = KurumAdıtext.Text +Environment.NewLine+ AdresText.Text + Environment.NewLine + İlText.Text +"/"+ İlceText.Text;
                }
                item.ReadOnly = true;
            }
            etiketSayisi += 1;

        }

        /////// CheckBox Kullanıcı işlem yaptırma kod alanı ////////////////////
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox2.Visible = true;
                checkBox3.Visible = true;
                YeniAdresEklemeGroupBox.Visible = true;
            }
            else
            {
                checkBox2.Visible = false;
                checkBox3.Visible = false;
                YeniAdresEklemeGroupBox.Visible = false;

            }
        }
        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                YeniAdresGroupBox.Visible = true;
            }
            else
            {
                YeniAdresGroupBox.Visible = false ;

            }
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                GrupİsimGroupBox.Visible = true;
            }
            else
            {
                GrupİsimGroupBox.Visible = false;

            }
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                FirmaGrupEkleme.Visible = true;
            }
            else
            {
                FirmaGrupEkleme.Visible = false;

            }
        }
        private void GrupAdınaGöreAraComboBax_SelectedIndexChanged(object sender, EventArgs e)
        {
            GrupAdınaGöreAramaYapma();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            VeritabanıGrupİdÇekme();
        }
        private void KurumAdınaGöreAraComboBax_SelectedIndexChanged(object sender, EventArgs e)
        {
            KurumBilgileriÇekme();
        }
        private void GrupAdınaGöreAraComboBax2_SelectedIndexChanged(object sender, EventArgs e)
        {
            KurumBilgileriÇekme();
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                KurumAdıtext.Enabled = true;
                AdresText.Enabled = true;
                İlceText.Enabled = true;
                İlText.Enabled = true;
                button4.Visible = true;
                button5.Enabled = false;
                checkBox5.Enabled = false;

            }
            else
            {
                KurumAdıtext.Enabled = false;
                AdresText.Enabled = false;
                İlceText.Enabled = false;
                İlText.Enabled = false;
                button4.Visible = false;
                button5.Enabled = true;
            }
        }

        /////// CheckBox Kullanıcı işlem yaptırma kod alanı ////////////////////
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            VeritabanıVeriCekmeİslemleri();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            VeritabanıKurumEkleme();
            

        }
        private void button2_Click(object sender, EventArgs e)
        {
            VeritabanıGrupEkleme();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            KurumAdlarınıGruplaraEkleme();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            KurumBilgileriGüncelle();
            
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if ((GrupAdınaGöreAraComboBax.Text != "" && GrupAdınaGöreAraComboBax2.Text == "" ) || GrupAdınaGöreAraComboBax.Text == "" && GrupAdınaGöreAraComboBax2.Text == "" && KurumAdınaGöreAraComboBax.Text == "")
            {
                MessageBox.Show("Lütfen bir kurum veya grup adı seçiniz...");
            }
            else
            {
                if (tableLayoutPanel2.Enabled == true)
                {

                    String[] strlist;
                    foreach (RadioButton item in tableLayoutPanel2.Controls)
                    {
                        if (item.Checked)
                        {
                            strlist = item.Name.Split('_');
                            etiketSayisi = Convert.ToByte(strlist[1]);
                            ilkEtiketSay = etiketSayisi;
                            tableLayoutPanel2.Enabled = false;
                            richTextBilgiEkleme();
                            button6.Visible = true;
                        }

                    }
                    if (tableLayoutPanel2.Enabled)
                        MessageBox.Show("Lütfen bir etiket seçiniz...");
                }
                else if (etiketSayisi < 17)
                {
                    richTextBilgiEkleme();
                    button6.Visible = true;

                }
                if (etiketSayisi >= 17)
                {
                    button5.Enabled = false;
                    belgeHazirla.Enabled = true;
                }
            }
            
            
        }
        private void clearButton_Click(object sender, EventArgs e)
        {
            foreach (RadioButton item in tableLayoutPanel2.Controls)
            {
                if (item.Checked)
                { 
                    item.Checked = false;
                    break;
                }

            }
            foreach (RichTextBox item in tableLayoutPanel3.Controls)
                    item.Text = "";

            
            tableLayoutPanel2.Enabled = true;
            etiketSayisi = 0;
            belgeHazirla.Enabled = false;
            button5.Enabled = true;
            progressBar1.Value = 0;
        }
        private void belgeHazirla_Click(object sender, EventArgs e)
        {
            DökümanHazırla();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (etiketSayisi >= 1)
            {
                etiketSayisi--;
                string richTextAd = "etiketRichText_" + etiketSayisi.ToString();
                foreach (RichTextBox item in tableLayoutPanel3.Controls)
                    if (item.Name == richTextAd)
                    {
                        item.Text = string.Empty;
                        break;
                    }
                belgeHazirla.Enabled = false;
                button5.Enabled = true;
                
            }

            if ((etiketSayisi - ilkEtiketSay) == 0)
            {
                button6.Visible = false;
                foreach (RadioButton item in tableLayoutPanel2.Controls)
                {
                    if(item.Checked)
                    {
                        item.Checked = false;
                        break;
                    }
                }
                tableLayoutPanel2.Enabled = true;
                etiketSayisi = 0;
                ilkEtiketSay = 0;
                button5.Enabled = true;
            }


        }
    }
}
