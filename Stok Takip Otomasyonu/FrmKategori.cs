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
    public partial class frmKategori : Form 
    {
        public frmKategori()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection
            ("Data Source=LAPTOP-9LLNTJQM\\SQLEXPRESS;Initial Catalog=Stok_Takip;Integrated Security=True");
        bool durum; // durum diye bir değişken tanımlandı
        private void kategorikontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from kategoribilgileri", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read()) // kayıtlar okunduğu sürece
            {
                if (textBox1.Text == read["kategori"].ToString() || textBox1.Text=="") 
                    // aradığımız kayıt veritabanında varsa durumu false yap
                {
                    durum = false; // bunu engelle
                } 
            }
            baglanti.Close();
        }
        private void frmKategori_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            kategorikontrol(); // kategoriengelle'yi çağırdık 
            if (durum==true)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into kategoribilgileri(kategori) " +
                    "values('" + textBox1.Text + "')", baglanti);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Kategori Eklendi ^.^");
            }
            else
            {
                MessageBox.Show("Böyle Bir Kategori Var !!!", "Uyarı !!!");
            }
            textBox1.Text = ""; // textbox'ı temizledik
        }
    }
}
