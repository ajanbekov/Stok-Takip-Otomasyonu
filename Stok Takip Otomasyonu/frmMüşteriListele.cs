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
    public partial class frmMüşteriListele : Form
    {
        public frmMüşteriListele()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-9LLNTJQM\\SQLEXPRESS;Initial Catalog=Stok_Takip;Integrated Security=True");
        DataSet daset = new DataSet(); // kayıtları geçici tutmak için

        private void frmMüşteriListele_Load(object sender, EventArgs e)
        {
            Kayıt_Göster();
        }

        private void Kayıt_Göster()
        {
            baglanti.Open(); // aşağıdakiler listeleme komutları
            SqlDataAdapter adtr = new SqlDataAdapter("select *from müşteri", baglanti);
            adtr.Fill(daset, "müşteri"); // geçici tablo ve veritabanındaki tabloyu yazıyoruz
            dataGridView1.DataSource = daset.Tables["müşteri"];
            baglanti.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTc.Text = dataGridView1.CurrentRow.Cells["tc"].Value.ToString();
            txtAdSoyad.Text = dataGridView1.CurrentRow.Cells["adsoyad"].Value.ToString();
            txtTelefon.Text = dataGridView1.CurrentRow.Cells["telefon"].Value.ToString();
            txtAdres.Text = dataGridView1.CurrentRow.Cells["adres"].Value.ToString();
            txtEmail.Text = dataGridView1.CurrentRow.Cells["Email"].Value.ToString();
        }

        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update müşteri set adsoyad=@adsoyad," +
                "telefon=@telefon,adres=@adres,email=@email where tc=@tc", 
                baglanti); // tc hariç diğerleri güncelleniyor
            komut.Parameters.AddWithValue("@tc", txtTc.Text);
            komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
            komut.Parameters.AddWithValue("@telefon", txtTelefon.Text);
            komut.Parameters.AddWithValue("@adres", txtAdres.Text);
            komut.Parameters.AddWithValue("@email", txtEmail.Text);
            komut.ExecuteNonQuery(); // bu işlemi onaylamak için
            baglanti.Close();
            daset.Tables["müşteri"].Clear(); // kayıtları önce temizleyip sonra yeni kaydı getirir
            Kayıt_Göster(); // Kayıt_Göster metodunu çağırıyoruz 
            MessageBox.Show("Müşteri Kaydı Güncellendi💾");
            foreach (Control item in this.Controls) // text box'ları silme işlemi
            {
                if (item is TextBox) // eger bu kontroller textbox ise
                {
                    item.Text = ""; // textbox'ları sil
                }
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            // müşteri silme komutları
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from müşteri where tc='" + 
                dataGridView1.CurrentRow.Cells["tc"].Value.ToString()+"'",baglanti);
            komut.ExecuteNonQuery(); // onaylama komutu
            baglanti.Close() ;
            daset.Tables["müşteri"].Clear(); 
            Kayıt_Göster();
            MessageBox.Show("Kayıt Silindi❌");
        }

        private void txtTcAra_TextChanged(object sender, EventArgs e)
        {
            DataTable tablo = new DataTable(); // tanımlama işlemi yapıldı
            baglanti.Open() ;
            SqlDataAdapter adtr = new SqlDataAdapter("select *from müşteri where tc like '%" 
                +txtTcAra.Text + "%'", baglanti);
            adtr.Fill(tablo); // kayıtları tabloya aktarıcaz 
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }
    }
}
