using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Windows.Forms;

namespace EtiketProgramıV31
{
    public class BaglantiKatmanı
    {
        //public const string conn = "Server = 89.252.187.221:2082; Database=ekinoksv_etiketProgrami;Uid=ekinoksv_etiket;Pwd=muIp..{!@FF{";
        
        public static MySqlConnection baglan() // class içinde baglan adında fonksiyon oluşturduk
        {
            MySqlConnection baglanti = new MySqlConnection("Server =89.252.187.221;Port=3306; Database=ekinoksv_etiketProgrami;Uid=ekinoksv_etiket;Pwd=Gs-@t$XeW8MF;");
            //bağlantı yolunu verdik
            try
            {
                baglanti.Open();//bağlantıyı açtık
                MySqlConnection.ClearPool(baglanti);
                MySqlConnection.ClearAllPools();//bundan önceki bağlantıları temizledik

            }
            catch (MySqlException)
            {
                MessageBox.Show("Yetkiniz yok.Sistem yöneticisi ile görüşün","Bilgilendirme Mesajı");
                Application.Exit();
            }
            return (baglanti);//çağırıldığı yere bağlantıyı yolladık


        }

    }
}
