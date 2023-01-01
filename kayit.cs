using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Ardunio
{
    public partial class kayit : Form
    {
        OleDbConnection  bag = new OleDbConnection("Provider=Microsoft.Jet.Oledb.4.0; Data Source=vtb1.mdb");
        OleDbCommand kmt = new OleDbCommand();

        public kayit()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string sonuc = serialPort1.ReadExisting();

            if (sonuc != "")
            {
                label6.Text = sonuc;

            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
         
        }

        private void kayit_Load(object sender, EventArgs e)
        {
            serialPort1.PortName = Form1.portismi;
            serialPort1.BaudRate = Convert.ToInt16(Form1.banthizi);
            if (serialPort1.IsOpen == false)
            {
                try
                {
                    serialPort1.Open();
                    label7.Text = "Bağlantı Sağlandı";
                    label7.ForeColor = Color.Green;
                }
                catch (Exception)
                {
                    label7.Text = "Bağlantı Sağlanamadı";


                   
                }
              
            }
            else
            {
                label7.Text = "Bağlantı Sağlanamadı";
                label7.ForeColor = Color.Red;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Start();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            label6.Text = "___________";
            comboBox1.Text = "Seçiniz";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog resim=new OpenFileDialog();
            resim.Filter = "Resim Dosyaları (jpg) |*.jpg|Tüm Dosyalar|*.*";
            openFileDialog1.InitialDirectory = Application.StartupPath + "\\Images";
            resim.RestoreDirectory = true;

            if (resim.ShowDialog()==DialogResult.OK)
            {
                string di = resim.SafeFileName;
                textBox3.Text = di;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (label6.Text== "___________"|| textBox1.Text==""||textBox2.Text==""||textBox3.Text==""||comboBox1.Text== "Seçiniz")
            {
                MessageBox.Show("Bilgileri Eksiz Doldurunuz","Uyarı!",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    bag.Open();
                    kmt.Connection = bag;
                    kmt.CommandText = "Insert into personel (kid,isim,soyisim,bolum,resim)VALUES ('" + label6.Text + "','" + textBox1.Text + "','" + textBox2.Text + "','" + comboBox1.Text + "','" + textBox3.Text + "')";
                    kmt.ExecuteNonQuery();
                    label8.Text = "Kayıt Başarılı";
                    label8.ForeColor = Color.Green;
                    bag.Close();
                }
                catch (Exception)
                {
                     MessageBox.Show("Bu ID VeriTabanında zaten mevcut","HATA!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }


            }
           
     
        }

        private void kayit_FormClosed(object sender, FormClosedEventArgs e)
        {
          timer1.Stop();
            serialPort1.Close();
        }
    }
}
