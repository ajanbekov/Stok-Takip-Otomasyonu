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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Stok_Takip_Otomasyonu
{
    public partial class frmÜrünEkle : Form
    {
        public frmÜrünEkle()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection
            ("Data Source=LAPTOP-9LLNTJQM\\SQLEXPRESS;Initial Catalog=Stok_Takip;Integrated Security=True");
        bool durum; // durum diye bir değişken tanımlandı
        private void barkodkontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from urun", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read()) // kayıtlar okunduğu sürece
            {
                if (txtBarkodNo.Text == read["barkodno"].ToString() || txtBarkodNo.Text=="")
                // aradığımız kayıt veritabanında varsa durumu false yap, o kategoride girdigimiz marka
                // varsa engeller
                {
                    durum = false; // bunu engelle 
                }
            }
            baglanti.Close();
        }
        private void kategorigetir()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from kategoribilgileri", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboKategori.Items.Add(read["kategori"].ToString()); // kayıtlar buraya gelecek
            }
            baglanti.Close();
        }

        private void frmÜrünEkle_Load(object sender, EventArgs e)
        {
            kategorigetir();
        }

        private void comboKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboMarka.Items.Clear(); // bu işlem yapılırken daha önceki kayıtlar silinsin
            comboMarka.Text = "";
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select *from markabilgileri where kategori=" +
                "'"+comboKategori.SelectedItem+"'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                comboMarka.Items.Add(read["marka"].ToString()); // kayıtlar buraya gelecek
            }
            baglanti.Close();
        }

        private void btnYeniEkle_Click(object sender, EventArgs e)
        {
            barkodkontrol();
            if (durum==true)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into urun(barkodno,kategori,marka,urunadi,miktari,alisfiyati," +
                    "satisfiyati,tarih) values(@barkodno,@kategori,@marka,@urunadi,@miktari,@alisfiyati," +
                    "@satisfiyati,@tarih)", baglanti);
                komut.Parameters.AddWithValue("@barkodno", txtBarkodNo.Text);
                komut.Parameters.AddWithValue("@kategori", comboKategori.Text);
                komut.Parameters.AddWithValue("@marka", comboMarka.Text);
                komut.Parameters.AddWithValue("@urunadi", txtÜrünAdı.Text);
                komut.Parameters.AddWithValue("@miktari", int.Parse(txtMiktarı.Text));
                komut.Parameters.AddWithValue("@alisfiyati", double.Parse(txtAlışFiyatı.Text));
                komut.Parameters.AddWithValue("@satisfiyati", double.Parse(txtSatışFiyatı.Text));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery(); // işlemi onaylıyoruz 
                baglanti.Close();
                MessageBox.Show("Ürün Eklendi ^_+");
            }
            else
            {
                MessageBox.Show("Böyle Bir Barkod No Var -_-","Uyarı !!!");
            }
            comboMarka.Items.Clear();   
            foreach (Control item in groupBox1.Controls) // groupBox1'deki kontrolleri dolaş
            {
                if (item is System.Windows.Forms.TextBox) // TextBox içindekileri temizler 
                {
                    item.Text = "";
                } 
                if (item is System.Windows.Forms.ComboBox) // ComboBox içindekileri temizler
                {
                    item.Text = "";
                }   
            }
        }

        private void BarkodNotxt_TextChanged(object sender, EventArgs e)
        {
            if (BarkodNotxt.Text == "")
            {
                lblMiktari.Text = "";
                foreach (Control item in groupBox2.Controls) // groupbox2'deki kontrolleri dolaş
                {
                    if(item is System.Windows.Forms.TextBox)
                    {
                        item.Text = "";
                    }
                }
            }
            baglanti.Open() ;
            SqlCommand komut = new SqlCommand("select *from urun where barkodno like '"+BarkodNotxt.Text+"'",
                baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                Kategoritxt.Text = read["kategori"].ToString();
                Markatxt.Text = read["marka"].ToString();
                ÜrünAdıtxt.Text = read["urunadi"].ToString();
                lblMiktari.Text = read["miktari"].ToString();
                AlışFiyatıtxt.Text = read["alisfiyati"].ToString();
                SatışFiyatıtxt.Text = read["satisfiyati"].ToString(); 
            }
            baglanti.Close();         }

        private void btnVarOlanaEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update urun set miktari=miktari+'"+int.Parse(Miktarıtxt.Text)
                +"' where barkodno='"+BarkodNotxt.Text+"'",baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close() ;
            foreach (Control item in groupBox2.Controls) // groupbox2'deki kontrolleri dolaş
            {
                if (item is System.Windows.Forms.TextBox)
                {
                    item.Text = "";
                }
            }
            MessageBox.Show("Var Olan Ürüne Eklendi ^_-");
        }
    }
}
