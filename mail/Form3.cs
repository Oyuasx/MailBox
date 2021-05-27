using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net.NetworkInformation;
using System.Web;
using MarkupConverter;

namespace mail
{
    public partial class Form3 : Form
    {
        DataAccess kp3 = new DataAccess();
        Form4 sayfa4 = new Form4();
        bool cıkıs;
        List<mail_get_user> dgr_al = new List<mail_get_user>();

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            try
            {
                listBox2.Items.Add("Mailler Yükleniyor...");
                kp3.form3_db_ekleme_islemler();
                listBox2.Items.Remove(1);
                dgr_al = kp3.form3_db_cekme_islemler();
            }
            catch (Exception hy)
            {
                MessageBox.Show("" + hy);
            }
            //dgr_al.Reverse();
            listBox2.DataSource = dgr_al;
            listBox2.DisplayMember = "tam_deger";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(NetworkInterface.GetIsNetworkAvailable()==true)
                sayfa4.Show();
            else
                MessageBox.Show("İnternet Bağlantısı olmadan Mail gönderemezsiniz!!!", "Hata");
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




        public static string ConvertHtmlToText(string html)
        {

            string tut;
            tut = html.Replace("\r", " ");
            tut = tut.Replace("\n", " ");
            tut = tut.Replace("\t", string.Empty);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                                                                  @"( )+", " ");

            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"<( )*head([^>])*>", "<head>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"(<( )*(/)( )*head( )*>)", "</head>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     "(<head>).*(</head>)", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"<( )*script([^>])*>", "<script>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"(<( )*(/)( )*script( )*>)", "</script>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);



            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"(<script>).*(</script>)", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);



            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"<( )*style([^>])*>", "<style>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"(<( )*(/)( )*style( )*>)", "</style>",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     "(<style>).*(</style>)", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);


            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"<( )*td([^>])*>", "\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);


            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"<( )*br( )*>", "\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"<( )*li( )*>", "\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);


            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"<( )*div([^>])*>", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"<( )*tr([^>])*>", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"<( )*p([^>])*>", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);



            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"<[^>]*>", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);


            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"&nbsp;", " ",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"&bull;", " * ",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"&lsaquo;", "<",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"&rsaquo;", ">",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"&trade;", "(tm)",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"&frasl;", "/",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"<", "<",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @">", ">",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"&copy;", "(c)",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"&reg;", "(r)",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
  

            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     @"&(.{2,6});", string.Empty,
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);


            tut = tut.Replace("\n", "\r");

            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     "(\r)( )+(\r)", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     "(\t)( )+(\t)", "\t\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     "(\t)( )+(\r)", "\t\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     "(\r)( )+(\t)", "\r\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     "(\r)(\t)+(\r)", "\r\r",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            tut = System.Text.RegularExpressions.Regex.Replace(tut,
                     "(\r)(\t)+", "\r\t",
                     System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            string breaks = "\r\r\r";

            string tabs = "\t\t\t\t\t";
            for (int i = 0; i < tut.Length; i++)
            {
                tut = tut.Replace(breaks, "\r\r");
                tut = tut.Replace(tabs, "\t\t\t\t");
                breaks = breaks + "\r";
                tabs = tabs + "\t";
            }
            return tut;
        }




        int dizi;
        bool htmlkontrol;
        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            dizi = listBox2.SelectedIndex;
            //html - text kontrol ediyoruz
            try
            {
                htmlkontrol = (dgr_al[dizi].alınan_mail_icerik != HttpUtility.HtmlEncode(dgr_al[dizi].alınan_mail_icerik));
            }
            catch(Exception hata)
            {
                MessageBox.Show("" + hata);
            }
            if (checkBox1.Checked != true)
            {
                if (htmlkontrol == true)
                {
                    try
                    {
                        IMarkupConverter markupConverter = new MarkupConverter.MarkupConverter();
                        string text = markupConverter.ConvertHtmlToRtf(dgr_al[dizi].alınan_mail_icerik);
                        richTextBox1.Rtf = text;
                    }
                    catch (Exception)
                    {
                        richTextBox1.Text = $"{ConvertHtmlToText(dgr_al[dizi].alınan_mail_icerik)}";
                    }
                }
                else
                {
                    richTextBox1.Text = $"{dgr_al[dizi].alınan_mail_icerik}";
                }
            }
            else
            {
                richTextBox1.Text = $"{ConvertHtmlToText(dgr_al[dizi].alınan_mail_icerik)}";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox2.Items.Count >= 1)
            {
                if (listBox2.SelectedValue != null)
                {
                    var items = (List<mail_get_user>)listBox2.DataSource;
                    var item = (mail_get_user)listBox2.SelectedValue;
                    listBox2.DataSource = null;
                    listBox2.Items.Clear();
                    items.Remove(item);
                    listBox2.DataSource = items;
                    listBox2.DisplayMember = "tam_deger";
                    try
                    {
                        kp3.form3_secilen_degeri_sil(item.id,item.alınan_mail_konu,item.mail_alma_tarhi);
                    }
                    catch(Exception hata)
                    {
                        MessageBox.Show("" + hata);
                    }
                }
            }
            else
            {
                MessageBox.Show("Hata - Silinecek Mail Yok!!!");
            }
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }





        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            List<mail_get_user> results = dgr_al.FindAll(x => x.tam_deger.Contains(textBox1.Text));

            if(textBox1.Text == "")
            {
                listBox2.DataSource = dgr_al;
                listBox2.DisplayMember = "tam_deger";
            }
            else
            {
                listBox2.DataSource = results;
                listBox2.DisplayMember = "tam_deger";
            }
        }




        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Çıkış Yapmak İstediğinize Emin misiniz?", "Çıkış", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                cıkıs = true;   //diablog içinde çıkış yapınca hata veriyor(sanırsam dialogu kapatmaya çalışıyor.)
            }
            if (cıkıs == true)
                Environment.Exit(1);
        }
    }
}
