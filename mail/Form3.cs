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
using MailKit.Net.Imap;
using MailKit.Security;
using MailKit;
using MimeKit;
using System.IO;
using MailKit.Search;
using System.Threading;

namespace mail
{
    public partial class Form3 : Form
    {
        DataAccess kp3 = new DataAccess();
        Form4 sayfa4 = new Form4();
        bool cıkıs;
        int button_no = 1;
        List<mail_get_user> gelen_kutusu = new List<mail_get_user>();
        List<mail_get_user_dosyalar> gelen_kutusu_dosya = new List<mail_get_user_dosyalar>();
        List<mail_get_user_bodyfile> gelen_kutusu_bodyfile = new List<mail_get_user_bodyfile>();

        List<mail_send_user> giden_kutusu = new List<mail_send_user>();
        List<mail_send_user_dosyalar> giden_kutusu_dosya = new List<mail_send_user_dosyalar>();
        List<mail_send_user_bodyfile> giden_kutusu_bodyfile = new List<mail_send_user_bodyfile>();

        List<trash_get_user> trash_kutusu = new List<trash_get_user>();
        List<trash_get_user_dosyalar> trash_kutusu_dosya = new List<trash_get_user_dosyalar>();
        List<trash_get_user_bodyfile> trash_kutusu_bodyfile = new List<trash_get_user_bodyfile>();

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            listBox2.HorizontalScrollbar = true;
            gelen_kutusu = kp3.form3_db_cekme_islemler_gelen_mail();
            gelen_kutusu_bodyfile = kp3.form3_db_cekme_islemler_gelen_mail_bodyfile();
            gelen_kutusu_dosya = kp3.form3_db_cekme_islemler_gelen_mail_attachment();
            listBox2.DataSource = gelen_kutusu;
            listBox2.DisplayMember = "tam_deger";
            label2.Text = $"Total: {gelen_kutusu.Count}";
            giden_kutusu = kp3.form3_db_cekme_islemler_giden_mail();
            giden_kutusu_bodyfile = kp3.form3_db_cekme_islemler_giden_mail_bodyfile();
            giden_kutusu_dosya = kp3.form3_db_cekme_islemler_giden_mail_attachment();
            trash_kutusu = kp3.form3_db_cekme_islemler_trash_mail();
            trash_kutusu_bodyfile = kp3.form3_db_cekme_islemler_trash_mail_bodyfile();
            trash_kutusu_dosya = kp3.form3_db_cekme_islemler_trash_mail_attachment();


            //trash işlemelri bakılacak


            if (NetworkInterface.GetIsNetworkAvailable() == true)
            {
                backgroundWorker1.WorkerReportsProgress = true;
                backgroundWorker1.WorkerSupportsCancellation = true;
                label1.Text = "Mailler güncelleniyor...";
                backgroundWorker1.RunWorkerAsync();
            }
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



        int metinbaslangicIndex = 0;
        int dizi; 
        bool hataa=false;
        Graphics _graphics;
        private void button2_Click(object sender, EventArgs e)
        {
            dizi = listBox2.SelectedIndex;
            string tut;
            if(listBox2.SelectedItems.Count > 0)
            {
                if(pictureBox3.BackColor == Color.Green)
                    pictureBox3.BackColor = Color.Red;
                okundumu = true;
                richTextBox1.Clear();
                metinbaslangicIndex = 0;
                hataa = false;
                IMarkupConverter markupConverter = new MarkupConverter.MarkupConverter();
                if (button_no == 1)
                {
                    if (checkBox1.Checked != true)
                    {
                        bool htmlkontrol = (gelen_kutusu[dizi].alınan_mail_icerik != HttpUtility.HtmlEncode(gelen_kutusu[dizi].alınan_mail_icerik));
                        tut = gelen_kutusu[dizi].alınan_mail_icerik;
                        if (htmlkontrol == true)
                        {
                            string text;
                            string convert;
                            foreach (var x in gelen_kutusu_bodyfile)
                            {
                                try
                                {
                                    int kontrol = tut.IndexOf("< img src", metinbaslangicIndex);
                                    if (kontrol == -1)
                                        kontrol = tut.IndexOf("<img src", metinbaslangicIndex);
                                    if (kontrol == -1)
                                        hataa = true;
                                    if (hataa == false)
                                    {
                                        if (x.alınan_mail_no == gelen_kutusu[dizi].id)
                                        {
                                            int deger1 = tut.IndexOf(">", kontrol) + 1;
                                            text = tut.Substring(metinbaslangicIndex, kontrol - metinbaslangicIndex);
                                            convert = markupConverter.ConvertHtmlToRtf(text);
                                            richTextBox1.SelectedRtf = convert;
                                            metinbaslangicIndex = deger1;

                                            StringBuilder ab = new StringBuilder();

                                            _graphics = richTextBox1.CreateGraphics();
                                            int picw = (int)Math.Round((x.width / _graphics.DpiX) * 2540);
                                            int pich = (int)Math.Round((x.height / _graphics.DpiY) * 2540);
                                            int picwgoal = (int)Math.Round((x.width / _graphics.DpiX) * 1440);
                                            int pichgoal = (int)Math.Round((x.height / _graphics.DpiY) * 1440);

                                            int rictextwgoal = (int)Math.Round((richTextBox1.Width / _graphics.DpiX) * 1440);
                                            if (picwgoal >= rictextwgoal)
                                            {
                                                picwgoal = rictextwgoal - 700;
                                            }
                                            string imagetortf = BitConverter.ToString(x.alınan_mail_bodyfile, 0).Replace("-", string.Empty);

                                            ab.Append(@"{\rtf1{\pict\pngblip");
                                            ab.Append(@"\picw" + picw);
                                            ab.Append(@"\pich" + pich);
                                            ab.Append(@"\picwgoal" + picwgoal);
                                            ab.Append(@"\pichgoal" + pichgoal);
                                            ab.Append(@"\hex ");
                                            ab.Append(imagetortf + @"}\v image");
                                            ab.Append(@"}\par}");
                                            richTextBox1.SelectedRtf = ab.ToString();
                                            ab.Clear();
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    hataa = true;
                                }
                            }
                            if (hataa == true)
                            {
                                text = tut.Substring(metinbaslangicIndex, tut.Length - metinbaslangicIndex);

                                convert = markupConverter.ConvertHtmlToRtf(text);
                                richTextBox1.SelectedRtf = convert;
                            }
                        }
                        else
                        {
                            richTextBox1.Text = ConvertHtmlToText(tut); ;
                        }
                    }
                    else
                        richTextBox1.Text = ConvertHtmlToText(gelen_kutusu[dizi].alınan_mail_icerik);
                    //buttonla okuduğumuz mailde attachment varmı, varsa picturebox rengi değişçek
                    if(gelen_kutusu_dosya.Count > 0)
                    {
                        foreach (var y in gelen_kutusu_dosya)
                        {
                            if (y.alınan_mail_no == gelen_kutusu[dizi].id)
                            {
                                pictureBox3.BackColor = Color.Green;
                                break;
                            }
                        }
                    }
                }
                else if(button_no == 2){
                    if (checkBox1.Checked != true)
                    {
                        bool htmlkontrol = (giden_kutusu[dizi].gonderilen_mail_icerik != HttpUtility.HtmlEncode(giden_kutusu[dizi].gonderilen_mail_icerik));
                        tut = giden_kutusu[dizi].gonderilen_mail_icerik;
                        if (htmlkontrol == true)
                        {
                            string text;
                            string convert;
                            foreach (var x in giden_kutusu_bodyfile)
                            {
                                try
                                {
                                    int kontrol = tut.IndexOf("< img src", metinbaslangicIndex);
                                    if (kontrol == -1)
                                        kontrol = tut.IndexOf("<img src", metinbaslangicIndex);
                                    if (kontrol == -1)
                                        hataa = true;
                                    if (hataa == false)
                                    {
                                        if (x.gonderilen_mail_no == giden_kutusu[dizi].id)
                                        {
                                            int deger1 = tut.IndexOf(">", kontrol) + 1;
                                            text = tut.Substring(metinbaslangicIndex, kontrol - metinbaslangicIndex);
                                            convert = markupConverter.ConvertHtmlToRtf(text);
                                            richTextBox1.SelectedRtf = convert;
                                            metinbaslangicIndex = deger1;

                                            StringBuilder ab = new StringBuilder();

                                            _graphics = richTextBox1.CreateGraphics();
                                            int picw = (int)Math.Round((x.width / _graphics.DpiX) * 2540);
                                            int pich = (int)Math.Round((x.height / _graphics.DpiY) * 2540);
                                            int picwgoal = (int)Math.Round((x.width / _graphics.DpiX) * 1440);
                                            int pichgoal = (int)Math.Round((x.height / _graphics.DpiY) * 1440);

                                            int rictextwgoal = (int)Math.Round((richTextBox1.Width / _graphics.DpiX) * 1440);
                                            if (picwgoal >= rictextwgoal)
                                            {
                                                picwgoal = rictextwgoal - 700;
                                            }
                                            string imagetortf = BitConverter.ToString(x.gonderilen_mail_bodyfile, 0).Replace("-", string.Empty);
                                            ab.Append(@"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil\fcharset0 Microsoft Sans Serif;}}
\viewkind4\uc1\pard\f0\fs17");
                                            ab.Append(@"{\pict\pngblip");
                                            ab.Append(@"\picw" + picw);
                                            ab.Append(@"\pich" + pich);
                                            ab.Append(@"\picwgoal" + picwgoal);
                                            ab.Append(@"\pichgoal" + pichgoal);
                                            ab.Append(@"\hex ");
                                            ab.Append(imagetortf + @"}\v image");
                                            ab.Append(@"}\par}");
                                            richTextBox1.SelectedRtf = ab.ToString();
                                            ab.Clear();
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    hataa = true;
                                }
                            }
                            if (hataa == true)
                            {
                                text = tut.Substring(metinbaslangicIndex, tut.Length - metinbaslangicIndex);

                                convert = markupConverter.ConvertHtmlToRtf(text);
                                richTextBox1.SelectedRtf = convert;
                            }
                        }
                        else
                        {
                            richTextBox1.Text = ConvertHtmlToText(tut); ;
                        }
                    }
                    else
                        richTextBox1.Text = ConvertHtmlToText(giden_kutusu[dizi].gonderilen_mail_icerik);
                    //buttonla okuduğumuz mailde attachment varmı, varsa picturebox rengi değişçek
                    if (giden_kutusu_dosya.Count > 0)
                    {
                        foreach (var y in giden_kutusu_dosya)
                        {
                            if (y.gonderilen_mail_no == giden_kutusu[dizi].id)
                            {
                                pictureBox3.BackColor = Color.Green;
                                break;
                            }
                        }
                    }
                }
                else if(button_no == 3)
                {
                    if (checkBox1.Checked != true)
                    {
                        bool htmlkontrol = (trash_kutusu[dizi].alınan_mail_icerik != HttpUtility.HtmlEncode(trash_kutusu[dizi].alınan_mail_icerik));
                        tut = trash_kutusu[dizi].alınan_mail_icerik;
                        if (htmlkontrol == true)
                        {
                            string text;
                            string convert;
                            foreach (var x in trash_kutusu_bodyfile)
                            {
                                try
                                {
                                    int kontrol = tut.IndexOf("< img src", metinbaslangicIndex);
                                    if (kontrol == -1)
                                        kontrol = tut.IndexOf("<img src", metinbaslangicIndex);
                                    if (kontrol == -1)
                                        hataa = true;
                                    if (hataa == false)
                                    {
                                        if (x.alınan_mail_no == trash_kutusu[dizi].id)
                                        {
                                            int deger1 = tut.IndexOf(">", kontrol) + 1;
                                            text = tut.Substring(metinbaslangicIndex, kontrol - metinbaslangicIndex);
                                            convert = markupConverter.ConvertHtmlToRtf(text);
                                            richTextBox1.SelectedRtf = convert;
                                            metinbaslangicIndex = deger1;

                                            StringBuilder ab = new StringBuilder();

                                            _graphics = richTextBox1.CreateGraphics();
                                            int picw = (int)Math.Round((x.width / _graphics.DpiX) * 2540);
                                            int pich = (int)Math.Round((x.height / _graphics.DpiY) * 2540);
                                            int picwgoal = (int)Math.Round((x.width / _graphics.DpiX) * 1440);
                                            int pichgoal = (int)Math.Round((x.height / _graphics.DpiY) * 1440);

                                            int rictextwgoal = (int)Math.Round((richTextBox1.Width / _graphics.DpiX) * 1440);
                                            if (picwgoal >= rictextwgoal)
                                            {
                                                picwgoal = rictextwgoal - 700;
                                            }
                                            string imagetortf = BitConverter.ToString(x.alınan_mail_bodyfile, 0).Replace("-", string.Empty);

                                            ab.Append(@"{\rtf1{\pict\pngblip");
                                            ab.Append(@"\picw" + picw);
                                            ab.Append(@"\pich" + pich);
                                            ab.Append(@"\picwgoal" + picwgoal);
                                            ab.Append(@"\pichgoal" + pichgoal);
                                            ab.Append(@"\hex ");
                                            ab.Append(imagetortf + @"}\v image");
                                            ab.Append(@"}\par}");
                                            richTextBox1.SelectedRtf = ab.ToString();
                                            ab.Clear();
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    hataa = true;
                                }
                            }
                            if (hataa == true)
                            {
                                text = tut.Substring(metinbaslangicIndex, tut.Length - metinbaslangicIndex);

                                convert = markupConverter.ConvertHtmlToRtf(text);
                                richTextBox1.SelectedRtf = convert;
                            }
                        }
                        else
                        {
                            richTextBox1.Text = ConvertHtmlToText(tut); ;
                        }
                    }
                    else
                        richTextBox1.Text = ConvertHtmlToText(trash_kutusu[dizi].alınan_mail_icerik);
                    //buttonla okuduğumuz mailde attachment varmı, varsa picturebox rengi değişçek
                    if (trash_kutusu_dosya.Count > 0)
                    {
                        foreach (var y in trash_kutusu_dosya)
                        {
                            if (y.alınan_mail_no == trash_kutusu[dizi].id)
                            {
                                pictureBox3.BackColor = Color.Green;
                                break;
                            }
                        }
                    }
                }
            }   
        }

        int toplam_mesaj_giden;
        int toplam_mesaj_gelen;
        int toplam_mesaj_trash;
        private void button3_Click(object sender, EventArgs evnt)
        {
            if (listBox2.Items.Count > 0)
            {
                if (listBox2.SelectedValue != null)
                {
                    if (NetworkInterface.GetIsNetworkAvailable() == true)
                    {
                        using (var client = new ImapClient())
                        {
                            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                            client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);
                            client.Authenticate(login_user.Instance.Eposta, login_user.Instance.sifre);
                            if (button_no == 1)
                            {
                                var inbox = client.Inbox;
                                inbox.Open(FolderAccess.ReadWrite);
                                var items = (List<mail_get_user>)listBox2.DataSource;
                                var item = (mail_get_user)listBox2.SelectedValue;
                                for (int i = 0; i < toplam_mesaj_gelen; i++)
                                {
                                    if (item.id == gelen_kutusu[i].id)
                                    {
                                        var matchFolder = client.GetFolder(SpecialFolder.Trash);
                                        if (matchFolder != null)
                                            inbox.MoveTo(i, matchFolder);
                                        break;
                                    }
                                }
                                inbox.Expunge();
                                inbox.Close();
                                listBox2.DataSource = null;
                                items.Remove(item);
                                listBox2.DataSource = items;
                                listBox2.DisplayMember = "tam_deger";
                                label2.Text = $"Total: {items.Count}";
                                kp3.form3_secilen_degeri_sil_1(item.id, items, gelen_kutusu_dosya, gelen_kutusu_bodyfile);        //silmek yerine databasede silinenlere ekle dicez
                                client.Disconnect(true);
                            }

                            else if (button_no == 2)
                            {
                                var inbox = client.GetFolder(SpecialFolder.Sent);
                                inbox.Open(FolderAccess.ReadWrite);
                                var items = (List<mail_send_user>)listBox2.DataSource;
                                var item = (mail_send_user)listBox2.SelectedValue;
                                for (int i = 0; i < toplam_mesaj_giden; i++)
                                {
                                    if (item.id == giden_kutusu[i].id)
                                    {
                                        var matchFolder = client.GetFolder(SpecialFolder.Trash);
                                        if (matchFolder != null)
                                            inbox.MoveTo(i, matchFolder);
                                        break;
                                    }
                                }
                                inbox.Expunge();
                                inbox.Close();
                                listBox2.DataSource = null;
                                items.Remove(item);
                                listBox2.DataSource = items;
                                listBox2.DisplayMember = "tam_deger";
                                label2.Text = $"Total: {items.Count}";
                                kp3.form3_secilen_degeri_sil_2(item.id, items, giden_kutusu_dosya, giden_kutusu_bodyfile);        //silmek yerine databasede silinenlere ekle dicez
                                client.Disconnect(true);
                            }

                            else if (button_no == 3)
                            {
                                var inbox = client.GetFolder(SpecialFolder.Trash);
                                inbox.Open(FolderAccess.ReadWrite);
                                var items = (List<trash_get_user>)listBox2.DataSource;
                                var item = (trash_get_user)listBox2.SelectedValue;
                                for (int i = 0; i < toplam_mesaj_trash; i++)
                                {
                                    if (item.id == trash_kutusu[i].id)
                                    {
                                        inbox.AddFlags(i, MessageFlags.Deleted, true);
                                        break;
                                    }
                                }
                                inbox.Expunge();
                                inbox.Close();
                                listBox2.DataSource = null;
                                items.Remove(item);
                                listBox2.DataSource = items;
                                listBox2.DisplayMember = "tam_deger";
                                label2.Text = $"Total: {items.Count}";
                                kp3.form3_secilen_degeri_sil_3(item.id);        //tamemen silme işlemi
                            }
                        }

                    }
                    else
                    {
                        if(button_no == 1)
                        {
                            var items = (List<mail_get_user>)listBox2.DataSource;
                            var item = (mail_get_user)listBox2.SelectedValue;
                            listBox2.DataSource = null;
                            items.Remove(item);
                            listBox2.DataSource = items;
                            listBox2.DisplayMember = "tam_deger";
                            label2.Text = $"Total: {items.Count}";
                            kp3.form3_secilen_degeri_sil_1(item.id, items, gelen_kutusu_dosya, gelen_kutusu_bodyfile);
                        }
                        else if(button_no == 2)
                        {
                            var items = (List<mail_send_user>)listBox2.DataSource;
                            var item = (mail_send_user)listBox2.SelectedValue;
                            listBox2.DataSource = null;
                            items.Remove(item);
                            listBox2.DataSource = items;
                            listBox2.DisplayMember = "tam_deger";
                            label2.Text = $"Total: {items.Count}";
                            kp3.form3_secilen_degeri_sil_2(item.id, items, giden_kutusu_dosya, giden_kutusu_bodyfile);
                        }
                        else if (button_no == 3)
                        {
                            var items = (List<trash_get_user>)listBox2.DataSource;
                            var item = (trash_get_user)listBox2.SelectedValue;
                            listBox2.DataSource = null;
                            items.Remove(item);
                            listBox2.DataSource = items;
                            listBox2.DisplayMember = "tam_deger";
                            label2.Text = $"Total: {items.Count}";
                            kp3.form3_secilen_degeri_sil_3(item.id);

                        }
                    }
                    trash_kutusu = kp3.form3_db_cekme_islemler_trash_mail();
                    trash_kutusu_bodyfile = kp3.form3_db_cekme_islemler_trash_mail_bodyfile();
                    trash_kutusu_dosya = kp3.form3_db_cekme_islemler_trash_mail_attachment();
                    //trash kutusu yenilemek için
                }
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
        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(button_no == 1)
            {
                List<mail_get_user> results = gelen_kutusu.FindAll(x => x.tam_deger.Contains(textBox1.Text));

                if (textBox1.Text == "")
                {
                    listBox2.DataSource = gelen_kutusu;
                    listBox2.DisplayMember = "tam_deger";
                }
                else
                {
                    listBox2.DataSource = results;
                    listBox2.DisplayMember = "tam_deger";
                }
            }
            else if(button_no == 2)
            {
                List<mail_send_user> results = giden_kutusu.FindAll(x => x.tam_deger.Contains(textBox1.Text));

                if (textBox1.Text == "")
                {
                    listBox2.DataSource = giden_kutusu;
                    listBox2.DisplayMember = "tam_deger";
                }
                else
                {
                    listBox2.DataSource = results;
                    listBox2.DisplayMember = "tam_deger";
                }
            }
            else if (button_no == 3)
            {
                List<trash_get_user> results = trash_kutusu.FindAll(x => x.tam_deger.Contains(textBox1.Text));

                if (textBox1.Text == "")
                {
                    listBox2.DataSource = trash_kutusu;
                    listBox2.DisplayMember = "tam_deger";
                }
                else
                {
                    listBox2.DataSource = results;
                    listBox2.DisplayMember = "tam_deger";
                }
            }
        }






        ///////////////////////////////////////////////////////////////////////////////
        List<mail_tut> mail_tut = new List<mail_tut>();
        List<mail_bodyfile_tut> mail_bodyfile_tut = new List<mail_bodyfile_tut>();
        List<mail_attachment_tut> mail_attachment_tut = new List<mail_attachment_tut>();


        int toplam_mesaj;

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs evnt)
        {
            using (var client = new ImapClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);

                client.Authenticate(login_user.Instance.Eposta, login_user.Instance.sifre);
                IMailFolder inbox = null;
                for (int i =1; i<4; i++)
                {
                    //inbox.close() gereksiz - sunucu otomatik olarak eski klasörü kapatıyor. https://stackoverflow.com/questions/29490638/mailkit-imailfolder-close-throws-exception  - jstedfast 12/06/2021 18:15
                    if (i == 1)
                    {
                        inbox = client.Inbox;
                        get_mails(inbox, client);
                        kp3.form3_db_ekleme_islemler_gelen_mesaj(toplam_mesaj, mail_tut, mail_bodyfile_tut, mail_attachment_tut);
                        gelen_kutusu.Clear();
                        gelen_kutusu_bodyfile.Clear();
                        gelen_kutusu_dosya.Clear();
                        gelen_kutusu = kp3.form3_db_cekme_islemler_gelen_mail();
                        gelen_kutusu_bodyfile = kp3.form3_db_cekme_islemler_gelen_mail_bodyfile();
                        gelen_kutusu_dosya = kp3.form3_db_cekme_islemler_gelen_mail_attachment();
                    }
                    if(i==2)
                    {
                        inbox = client.GetFolder(SpecialFolder.Sent);
                        get_mails(inbox, client);
                        kp3.form3_db_ekleme_islemler_gonderilen_mesaj(toplam_mesaj, mail_tut, mail_bodyfile_tut, mail_attachment_tut);
                        giden_kutusu.Clear();
                        giden_kutusu_bodyfile.Clear();
                        giden_kutusu_dosya.Clear();
                        giden_kutusu = kp3.form3_db_cekme_islemler_giden_mail();
                        giden_kutusu_bodyfile = kp3.form3_db_cekme_islemler_giden_mail_bodyfile();
                        giden_kutusu_dosya = kp3.form3_db_cekme_islemler_giden_mail_attachment();
                    }
                    if (i == 3)
                    {
                        inbox = client.GetFolder(SpecialFolder.Trash);
                        get_mails(inbox, client);
                        kp3.form3_db_ekleme_islemler_trash_mesaj(toplam_mesaj, mail_tut, mail_bodyfile_tut, mail_attachment_tut);
                        trash_kutusu.Clear();
                        trash_kutusu_bodyfile.Clear();
                        trash_kutusu_dosya.Clear();
                        trash_kutusu = kp3.form3_db_cekme_islemler_trash_mail();
                        trash_kutusu_bodyfile = kp3.form3_db_cekme_islemler_trash_mail_bodyfile();
                        trash_kutusu_dosya = kp3.form3_db_cekme_islemler_trash_mail_attachment();
                    }
                }
                client.Disconnect(true);
            }
        }
        public void get_mails(IMailFolder folder, ImapClient client)
        {
            mail_tut.Clear();
            mail_bodyfile_tut.Clear();
            mail_attachment_tut.Clear();
            int id = 1;
            folder.Open(FolderAccess.ReadOnly);
            toplam_mesaj = folder.Count;
            if (folder == client.GetFolder(SpecialFolder.Sent))
                toplam_mesaj_giden = toplam_mesaj;
            if (folder == client.Inbox)
                toplam_mesaj_gelen = toplam_mesaj;
            if (folder == client.GetFolder(SpecialFolder.Trash))
                toplam_mesaj_trash = toplam_mesaj;
            string text;
            IList<UniqueId> uids = folder.Search(SearchQuery.All);
            foreach (UniqueId uid in uids)
            {
                MimeMessage message = folder.GetMessage(uid);
                if (message.HtmlBody != null)
                {
                    text = message.HtmlBody;
                    string tut = text.Replace(" ", string.Empty);  //değer alacağımızdan boşlukların bir önemi yok
                    foreach (MimePart att in message.BodyParts)
                    {
                        if (att.ContentId != null && att.Content != null && att.ContentType.MediaType == "image" && (text.IndexOf("cid:" + att.ContentId) > -1))
                        {
                            byte[] b;
                            using (var mem = new MemoryStream())
                            {
                                att.Content.DecodeTo(mem);
                                b = mem.ToArray();
                            }
                            string bodyfile_name=string.Empty;
                            int resim_width=0, resim_height=0;
                            bool hata_tut = false;
                            try 
                            {
                                int deger1 = tut.IndexOf(@"<imgsrc=""cid:") + 13;
                                int deger2 = tut.IndexOf(@"""", deger1);
                                bodyfile_name = tut.Substring(deger1, deger2 - deger1);

                                int deger3 = tut.IndexOf("width=", deger1) + 6;
                                int deger4 = tut.IndexOf("height=", deger3);
                                resim_width = Convert.ToInt32(tut.Substring(deger3, deger4 - deger3));

                                int deger5 = tut.IndexOf(@"height=", deger1) + 7;
                                int deger6 = tut.IndexOf(">", deger5);
                                resim_height = Convert.ToInt32(tut.Substring(deger5, deger6 - deger5));
                            }
                            catch (Exception)
                            {
                                hata_tut = true;
                            }
                            if(hata_tut == false)
                            {
                                mail_bodyfile_tut.Add(new mail_bodyfile_tut
                                {
                                    id = id,
                                    alınan_mail_bodyfile = b,
                                    width = resim_width,
                                    height = resim_height
                                });
                            }
                            tut = tut.Replace(@"cid:", string.Empty);
                        }
                    }
                }

                else if (message.TextBody != null)
                    text = message.TextBody;
                else
                    text = string.Empty;
                mail_tut.Add(new mail_tut
                {
                    id = id,
                    baslik = message.Subject,
                    icerik = text,
                    tarih = (DateTimeOffset)message.Date,
                    yollayan = message.From.ToString(),
                });
                if(message.Attachments != null)
                {
                    foreach(var attachment in message.Attachments) {
                        using (var stream = new MemoryStream())
                        {
                            var part = (MimePart)attachment;

                            part.Content.DecodeTo(stream);

                            byte[] byt = stream.ToArray();
                            mail_attachment_tut.Add(new mail_attachment_tut
                            {
                                alınan_mail_attachment= byt,
                                id = id
                            });
                        }

                    }
                }
                id++;
            }

        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label1.Text = "Mailler Güncellendi...";
            if (button_no == 1)
            {
                label2.Text = $"Total: {toplam_mesaj_gelen}";
                listBox2.DataSource = null;
                listBox2.DataSource = gelen_kutusu;
                listBox2.DisplayMember = "tam_deger";
            }
            else if (button_no == 2)
            {
                label2.Text = $"Total: {toplam_mesaj_giden}";
                listBox2.DataSource = null;
                listBox2.DataSource = giden_kutusu;
                listBox2.DisplayMember = "tam_deger";
            }
            else if (button_no == 3)
            {
                label2.Text = $"Total: {toplam_mesaj_trash}";
                listBox2.DataSource = null;
                listBox2.DataSource = trash_kutusu;
                listBox2.DisplayMember = "tam_deger";
            }
            basıldımı = false;
            button6.BorderColor = Color.Black;
        }




        bool okundumu = false;
        private void button9_Click(object sender, EventArgs e)
        {
            if(okundumu == true)
            {
                if (button_no == 1)
                {
                    if (listBox2.SelectedItems.Count > 0)
                    {
                        foreach (var x in gelen_kutusu_dosya)
                        {
                            if (gelen_kutusu[dizi].id == x.alınan_mail_no)
                            {
                                SaveFileDialog file = new SaveFileDialog();
                                file.ShowDialog();

                                if (file.FileName != "")
                                {
                                    string dosya_byte = BitConverter.ToString(x.alınan_mail_dosyalar, 0).Replace("-", string.Empty);
                                    File.WriteAllText(file.FileName, dosya_byte);
                                }
                            }
                        }
                    }
                }
                else if(button_no == 2)
                {
                    if (listBox2.SelectedItems.Count > 0)
                    {
                        foreach (var x in giden_kutusu_dosya)
                        {
                            if (giden_kutusu[dizi].id == x.gonderilen_mail_no)
                            {
                                SaveFileDialog file = new SaveFileDialog();
                                file.ShowDialog();

                                if (file.FileName != "")
                                {
                                    string dosya_byte = BitConverter.ToString(x.gonderilen_mail_dosyalar, 0).Replace("-", string.Empty);
                                    File.WriteAllText(file.FileName, dosya_byte);
                                }
                            }
                        }
                    }
                }
                else if (button_no == 3)
                {
                    if (listBox2.SelectedItems.Count > 0)
                    {
                        foreach (var x in trash_kutusu_dosya)
                        {
                            if (trash_kutusu[dizi].id == x.alınan_mail_no)
                            {
                                SaveFileDialog file = new SaveFileDialog();
                                file.ShowDialog();

                                if (file.FileName != "")
                                {
                                    string dosya_byte = BitConverter.ToString(x.alınan_mail_dosyalar, 0).Replace("-", string.Empty);
                                    File.WriteAllText(file.FileName, dosya_byte);
                                }
                            }
                        }
                    }
                }
            }
        }
        bool basıldımı = false;

        private void button6_Click(object sender, EventArgs e)
        {
            if(basıldımı == false)
            {
                if (NetworkInterface.GetIsNetworkAvailable() == true)
                {
                    label1.Text = "Mailler güncelleniyor...";
                    backgroundWorker1.RunWorkerAsync();
                    basıldımı = true;
                    button6.BorderColor = Color.Red;
                }
                else
                {
                    gelen_kutusu = kp3.form3_db_cekme_islemler_gelen_mail();
                    gelen_kutusu_bodyfile = kp3.form3_db_cekme_islemler_gelen_mail_bodyfile();
                    gelen_kutusu_dosya = kp3.form3_db_cekme_islemler_gelen_mail_attachment();
                    giden_kutusu = kp3.form3_db_cekme_islemler_giden_mail();
                    giden_kutusu_bodyfile = kp3.form3_db_cekme_islemler_giden_mail_bodyfile();
                    giden_kutusu_dosya = kp3.form3_db_cekme_islemler_giden_mail_attachment();
                    trash_kutusu = kp3.form3_db_cekme_islemler_trash_mail();
                    trash_kutusu_bodyfile = kp3.form3_db_cekme_islemler_trash_mail_bodyfile();
                    trash_kutusu_dosya = kp3.form3_db_cekme_islemler_trash_mail_attachment();
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            button_no = 3;
            label2.Text = $"Total: {toplam_mesaj_trash}";
            listBox2.DataSource = null;
            listBox2.DataSource = trash_kutusu;
            listBox2.DisplayMember = "tam_deger";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button_no = 2;
            label2.Text = $"Total: {toplam_mesaj_giden}";
            listBox2.DataSource = null;
            listBox2.DataSource = giden_kutusu;
            listBox2.DisplayMember = "tam_deger";
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            button_no = 1;
            label2.Text = $"Total: {toplam_mesaj_gelen}";
            listBox2.DataSource = null;
            listBox2.DataSource = gelen_kutusu;
            listBox2.DisplayMember = "tam_deger";
        }

    }
}
