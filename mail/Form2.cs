using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Utils;

namespace mail
{
    public partial class Form2 : Form
    {
        int sayac = 30;
        bool cıkıs;
        Form3 sayfa3 = new Form3();
        DataAccess kp2 = new DataAccess();

        int rndm_tutan_deger;
        public int random_fonksiyon()
        {
            Random random = new Random();
            int rand_sayı = random.Next(10000, 99999); //doğrulama codu (5 haneli)
            return rand_sayı;
        }
        public void random_sayi_yolla(int random_sayı)
        {
            var message = new MimeMessage();
            var builder = new BodyBuilder();
            var image = builder.LinkedResources.Add(@"image.jpg");
            image.ContentId = MimeUtils.GenerateMessageId();
            builder.HtmlBody = string.Format
                (
                    @"<b>XyzMail Doğrulama Kodunuz: </b> " + random_sayı +
                    @"<p>Bizi Tercih Ettiğiniz İçin teşekkür ederiz. <br/></p>

                    <img src=""cid:{0}"" width=500 height=200 > ", image.ContentId
                );

            message.From.Add(MailboxAddress.Parse("bugraverify@gmail.com"));
            message.To.Add(MailboxAddress.Parse(login_user.Instance.Eposta));
            message.Subject = "XyzMail - Güvenlik Doğrulama Kodu.";
            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                client.Authenticate("bugraverify@gmail.com", "31082000B");

                client.Send(message);
                client.Disconnect(true);
            }
        }


        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            rndm_tutan_deger = random_fonksiyon();
            random_sayi_yolla(rndm_tutan_deger);
            timer1.Interval = 1000;
            timer1.Enabled = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (sayac > 0)
            {
                sayac--;
                button2.Text = sayac.ToString();
            }
            else if (sayac == 0)
            {
                button2.Text = "Yeni Kod";
                timer1.Enabled = false;
                sayac = 31;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (sayac == 31)
            {
                rndm_tutan_deger = random_fonksiyon();
                random_sayi_yolla(rndm_tutan_deger);
                MessageBox.Show("Güvenlik Kodu Gönderildi!", "Güvenlik");
                timer1.Enabled = true;
            }
            else
                MessageBox.Show("Biraz Sakinleşmelisin! (30 sec)", "Güvenlik");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "" || int.Parse(textBox1.Text) < 10000)
                {
                    MessageBox.Show("Doğrulama Kodu Hatalı!", "Güvenlik");
                }
                else
                {
                    if (int.Parse(textBox1.Text) == rndm_tutan_deger)
                    {
                        //doğrulama geçerse db işlemleri başlicak
                        if (kp2.login_user_db_islemler(true) == true)   //db işlemleri
                        {
                            MessageBox.Show("Güvenlik Doğrulandı!", "Güvenlik");
                            this.Close();
                            sayfa3.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Doğrulama Kodu Hatalı!", "Güvenlik");
                    }
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Doğrulama Kodu Hatalı!" , "Güvenlik");
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
        int mouse_x,mouse_y;

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

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
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
