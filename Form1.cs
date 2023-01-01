using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Runtime.CompilerServices;

namespace Ardunio
{
    public partial class Form1 : Form
    {

        //Veritabanı bağlantısı 
        OleDbConnection bag = new OleDbConnection("Provider=Microsoft.Jet.Oledb.4.0; Data Source=vtb1.mdb");
        OleDbCommand kmt = new OleDbCommand();

        //evrensel değişkenler
        public static  string portismi, banthizi;
        string[] ports = SerialPort.GetPortNames();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
                 timer1.Start();
                portismi = comboBox1.Text;
                banthizi = comboBox2.Text;
            try
            {
                serialPort1.PortName = portismi;
                serialPort1.BaudRate = Convert.ToInt16(banthizi);
                serialPort1.Open();
                label1.Text = "Bağlantı Sağlandı";
                label1.ForeColor = Color.Green;
            }
            catch (Exception)
            {


                MessageBox.Show("Zaten Bağlısın", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
             


      
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen==true)
            {
                timer1.Stop();
                serialPort1.Close();
                label1.Text = "Bağlantı Kapatıldı";

                label1.ForeColor = Color.Red;

            }
            else
            {
                MessageBox.Show("Bağlantı Zaten Kapalı","Uyarı",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string sonuc = serialPort1.ReadExisting();

           if (sonuc!="")
            {
                label3.Text = sonuc;
                kart_idlbl.Text=sonuc;
                bag.Open();
                kmt.Connection = bag;
                kmt.CommandText = "SELECT * FROM personel WHERE kid='" + sonuc + "'";

                OleDbDataReader oku=kmt.ExecuteReader();

                if (oku.Read())
                {
                    DateTime bugun = DateTime.Now;
                    pictureBox1.Image = Image.FromFile("Images\\" + oku["resim"].ToString());
                    isimlbl.Text = oku["isim"].ToString();
                    soyisimlbl.Text = oku["soyisim"].ToString();
                    tarihlbl.Text = bugun.ToShortDateString();
                    saatlbl.Text=bugun.ToLongTimeString();
                    bag.Close();
                    bag.Open();
                    kmt.CommandText="Insert into zaman (ID,isim,soyisim,tarih,saat)VALUES ('" +kart_idlbl.Text+"','"+isimlbl.Text+"','"+ soyisimlbl.Text+"','"+tarihlbl.Text+"','"+saatlbl.Text+"')";
                    kmt.ExecuteReader();
                    bag.Close();

                }
                else
                {
                    pictureBox1.Enabled = false;
                    isimlbl.Text = "_____";
                    soyisimlbl.Text = "_____";
                    saatlbl.Text = "_____";
                    tarihlbl.Text = "_____";
                    kart_idlbl.Text = "_____";
                    label3.Text="";

                    MessageBox.Show("Böyle Bir Kullanıcı VeriTabanında Bulunamadı.","HATA!",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }

                bag.Close();
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (banthizi==null| serialPort1==null)
            {
                MessageBox.Show("Bağlantını Kontrol Et", "Uyarı!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                timer1.Stop();
                serialPort1.Close();
                label1.Text = "Bağlantı Kapatıldı";
                label1.ForeColor= Color.Yellow;
                kayit kyt = new kayit();
                kyt.ShowDialog();
            }
          
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
            }
            comboBox2.Items.Add("2400");
            comboBox2.Items.Add("9600");
            comboBox1.SelectedIndex= 0;
            comboBox2.SelectedIndex= 1;

            // comboBox1.Items.Add(port); //Seri portları comBox1' ekleme
        }
    }
}
