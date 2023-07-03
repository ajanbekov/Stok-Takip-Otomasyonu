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

namespace Stok_Takip_Otomasyonu
{
    public partial class FrmMüşteriEkle : Form
    {
        public FrmMüşteriEkle()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-9LLNTJQM\\SQLEXPRESS;Initial Catalog=Stok_Takip;Integrated Security=True");
        private void FrmMüşteriEkle_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand
                ("insert into müşteri(tc,adsoyad,telefon,adres,email) values " +
                "(@tc,@adsoyad,@telefon,@adres,@email)", baglanti);
            komut.Parameters.AddWithValue("@tc", txtTc.Text);
            komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
            komut.Parameters.AddWithValue("@telefon", txtTelefon.Text);
            komut.Parameters.AddWithValue("@adres", txtAdres.Text);
            komut.Parameters.AddWithValue("@email", txtEmail.Text);
            komut.ExecuteNonQuery(); // bu işlemi onaylamak için
            baglanti.Close();
            MessageBox.Show("Müşteri Kaydı Eklendi👍");
            foreach (Control item in this.Controls) // text box'ları silme işlemi
            {
                if(item is TextBox) // eger bu kontroller textbox ise
                {
                    item.Text = ""; // textbox'ları sil
                }
            } 
        }
    }
}
