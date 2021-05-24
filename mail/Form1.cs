using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Net;
using System.Net.NetworkInformation;


namespace mail
{
    public partial class Form1 : Form
    {
        DataAccess kp1 = new DataAccess();
        string pc_name, ip, posta = "", sifre, db_postadeger, tekrar_sifre;
        bool hata_varmı = false, cıkıs;
        Form2 sayfa2 = new Form2();
        Form3 sayfa3 = new Form3();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
            textBox3.UseSystemPasswordChar = true;
            try // database bağlantısı kontrolü
            {
                kp1.Connection();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Database bağlantı hatası! - " + hata);
            }
            pc_name = Environment.MachineName;
            ip = Dns.GetHostAddresses(Environment.MachineName)[1].ToString();
            login_user.Instance.IPV4 = ip;
            login_user.Instance.pc_user_name = pc_name;

            db_postadeger = kp1.bilgisayar_deger_kontrol(ip, pc_name);

            if (db_postadeger == "")
                textBox1.Text = "E posta giriniz...";
            else
            {
                posta = db_postadeger;
                textBox1.Text = posta;       //Oto doldurma kısmı
            }

        }
        public void işlemler()
        {
            posta = textBox1.Text;
            if (posta == "" || posta == "E posta giriniz...") 
            {
                MessageBox.Show("Eposta kısmı boş geçilemez! - Hata!");
            }
            else
            {
                sifre = textBox2.Text;
                tekrar_sifre = textBox3.Text;
                if (sifre == tekrar_sifre && sifre != "")   //şifreler uyuyorsa ve boş değilse data kontrol olcak(int için)
                {                                          //ve int varsa bağlantı kotnrolü olcak
                    login_user.Instance.Eposta = posta;
                    login_user.Instance.sifre = sifre;
                    if (NetworkInterface.GetIsNetworkAvailable() == true)    //internet bağlantısı var ise mail hesabı senin mi diye kontrol
                    {
                        try
                        {
                            kp1.mail_baglanti_kontrol();    //mail üzerinden hesap kontrolü
                        }
                        catch
                        {
                            hata_varmı = true;
                        }
                        if (hata_varmı == true)
                        {
                            MessageBox.Show("Giriş İşlemi Başarısız / Şifre veya Mail Hatalı!");
                        }
                        else //hata vermezse internetli giriş başlicak
                        {
                            sayfa2.Show();
                            this.Hide();
                        }
                    }
                    else
                    {
                        if (kp1.login_user_db_islemler(false) == true)
                        {
                            sayfa3.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Giriş İşlemi Başarısız / Şifre veya Mail Hatalı!");
                        }
                    }
                }
                else
                {
                    if (sifre == "")
                        MessageBox.Show("Şifre boş geçilemez! - Hata!");
                    else
                        MessageBox.Show("Şifre uyuşmazlığı! - Hata!");
                }

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Giriş İşlemi Başlatılıyor...");
            işlemler();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)       //enter'a basıldığında giriş yapar.
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
                textBox3.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
                textBox3.UseSystemPasswordChar = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Çıkış Yapmak İstediğinize Emin misiniz?", "Çıkış", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                cıkıs = true;   //diablog içinde çıkış yapınca hata veriyor(sanırsam dialogu kapatmaya çalışıyor.)
            }
            if (cıkıs == true)
                Environment.Exit(1);
        }
        bool move;
        int mouse_x, mouse_y;
        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouse_x = e.X;
            mouse_y = e.Y;
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouse_x, MousePosition.Y - mouse_y);
            }
        }

    }
}