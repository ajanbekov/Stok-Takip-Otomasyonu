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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Stok_Takip_Otomasyonu
{
    public partial class frmSatışListele : Form
    {
        public frmSatışListele()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection
            ("Data Source=LAPTOP-9LLNTJQM\\SQLEXPRESS;Initial Catalog=Stok_Takip;Integrated Security=True");
        DataSet daset = new DataSet();
        private void satışlistele()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select *from satis", baglanti); // data grid'de
                                                                                      // göstereceğimiz için
                                                                                      // SqlDataAdapter kullandık
            adtr.Fill(daset, "satis");
            dataGridView1.DataSource = daset.Tables["satis"]; // kayıtları data grid'e getiricez
            
            baglanti.Close();
        }
        private void frmSatışListele_Load(object sender, EventArgs e)
        {
            satışlistele();
        }
    }
}
