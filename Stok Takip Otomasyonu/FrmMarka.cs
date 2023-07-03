using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stok_Takip_Otomasyonu
{
    public partial class frmMarka : Form
    {
        public frmMarka()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection // veritabanı ile bağlantı sağlar
            ("Data Source=LAPTOP-9LLNTJQM\\SQLEXPRESS;Initial Catalog=Stok_Takip;Integrated Security=True");
        bool durum; // durum diye bir değişken tanımlandı
        private void markakontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from markabilgileri", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read()) // kayıtlar okunduğu sürece
            {
                if (comboBox1.Text == read["kategori"].ToString() && textBox1.Text == read["marka"].ToString()
                    || comboBox1.Text == "" || textBox1.Text == "")
                // aradığımız kayıt veritabanında varsa durumu false yap, o kategoride girdigimiz marka
                // varsa engeller
                {
                    durum = false; // bunu engelle
                }
            }
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            markakontrol();
            if (durum == true)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into markabilgileri(kategori,marka) " +
                    "values('" + comboBox1.Text + "','" + textBox1.Text + "')", baglanti);
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Marka Eklendi O_O");
            }
            else
            {
                MessageBox.Show("Böyle Kategori Ve Marka Var !!!", "Uyarı !!!");
            }
            textBox1.Text = ""; // textbox'ı temizledik
            comboBox1.Text = ""; // comboBox'ı temizledik
        }

        private void frmMarka_Load(object sender, EventArgs e)
        {
            kategorigetir();
        }

        private void kategorigetir()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from kategoribilgileri", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboBox1.Items.Add(read["kategori"].ToString()); // kayıtlar buraya gelecek
            }
            baglanti.Close(); 
        }
    }
}
