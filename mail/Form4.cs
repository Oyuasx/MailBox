using MailKit.Net.Smtp;
using MailKit.Security;
using MarkupConverter;
using MimeKit;
using MimeKit.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mail
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
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

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            richTextBox1.SelectionFont = fontDialog1.Font;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            richTextBox1.SelectionColor = colorDialog1.Color;
        }

        DataAccess kp4 = new DataAccess();
        mail_send_user mail1 = new mail_send_user();
        List<string> attachment_dondur = new List<string>();
        private void Form4_Load(object sender, EventArgs e)
        {
            listBox1.HorizontalScrollbar = true;
        }
        private void button5_Click(object sender, EventArgs e)  //attachment ekleme
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Multiselect=true;
            if (file.ShowDialog() == DialogResult.OK)
            {
                foreach(string dosya_Ad in file.FileNames)
                {
                    attachment_dondur.Add(dosya_Ad);
                    listBox1.Items.Add(dosya_Ad);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)  // attachment temizleme
        {
            attachment_dondur.Clear();
            listBox1.Items.Clear();
        }


        string tut, rtf;
        int metinbaslangicIndex = 0;
        private void button2_Click(object sender, EventArgs evnt) //gönderme işlemi
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                metinbaslangicIndex = 0;
                SmtpClient client = new SmtpClient();
                MimeMessage message = new MimeMessage();
                BodyBuilder builder = new BodyBuilder();
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                client.Authenticate(login_user.Instance.Eposta, login_user.Instance.sifre);
                bool hata_varmı = false;
                rtf = richTextBox1.Rtf;
                while (true)
                {
                    try
                    {
                        tut = ExtractImgRtf(rtf);
                    }
                    catch (Exception)
                    {
                        hata_varmı = true;
                    }
                    try
                    {
                        bool duplicate_engelleme = false;
                        IMarkupConverter markupConverter = new MarkupConverter.MarkupConverter();
                        if (hata_varmı == false)
                        {
                            for (int i = 0; i < bfile.Count; i++)
                            {
                                if (bfile[i].bodyfile_string == tut && duplicate_engelleme == false)
                                {
                                    duplicate_engelleme = true;
                                    string body = rtf.Substring(metinbaslangicIndex, startIndex - metinbaslangicIndex);
                                    metinbaslangicIndex = (endIndex + 12);
                                    var bodyfile = builder.LinkedResources.Add(bfile[i].file_name, bfile[i].bodyfile_byte);
                                    bodyfile.ContentId = MimeUtils.GenerateMessageId();
                                    //try catch gelcek ve text html yerine textx döncek if error
                                    string html_body = markupConverter.ConvertRtfToHtml(body);
                                    builder.HtmlBody += html_body;
                                    builder.HtmlBody += string.Format(@"<img src=""cid:{0}"" width={1} height={2} > ", bodyfile.ContentId, 600, 800);
                                }
                            }
                            duplicate_engelleme = false;
                        }
                        else
                        {
                            string body = rtf.Substring(metinbaslangicIndex, richTextBox1.Rtf.Length - metinbaslangicIndex);
                            string html_body = markupConverter.ConvertRtfToHtml(body);
                            builder.HtmlBody += html_body;
                            break;
                        }
                    }
                    catch (Exception ht)
                    {
                        MessageBox.Show("Hata: " + ht);
                        string body = richTextBox1.Text;
                        builder.TextBody = body;
                        break;
                    }
                }
                bool empty = !attachment_dondur.Any();
                if (empty != true)
                {
                    foreach (string yol in attachment_dondur)
                    {
                        builder.Attachments.Add(@"" + yol);
                    }
                }
                bool mesaj_gittimi = true;
                try
                {
                    message.From.Add(MailboxAddress.Parse(login_user.Instance.Eposta));
                    message.To.Add(MailboxAddress.Parse(textBox1.Text));
                    message.Subject = textBox2.Text;
                    message.Body = builder.ToMessageBody();
                    client.Send(message);
                }
                catch (Exception ht)
                {
                    MessageBox.Show("Mail yollanırken hata ile karşılaşıldı: " + ht);
                    mesaj_gittimi = false;
                }
                finally
                {
                    startIndex = 0;
                    endIndex = 0;
                    metinbaslangicIndex = 0;
                }
                if (mesaj_gittimi == true)
                    MessageBox.Show("Mesaj Gönderildi.");
            }
            else
            {
                if (textBox1.Text == "")
                    textBox1.Text = "Gönderilecek kişi ksmı boş geçilemez.";
                if (textBox2.Text == "")
                    textBox2.Text = "Konu ksmı boş geçilemez.";
                MessageBox.Show("Lütfen Her bir bölümü doldurduğunuza emin olunuz...");
            }

        }


        int startIndex;
        int endIndex;
        List<mail_send_user_bodyfile> bfile = new List<mail_send_user_bodyfile>();
        private void button6_Click(object sender, EventArgs e)  //richtext box içine resim ekleme işlemleri
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Multiselect = false;
            file.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.png; *.bmp)|*.jpg; *.jpeg; *.gif; *.png; *.bmp";
            if (file.ShowDialog() == DialogResult.OK)
            {
                MemoryStream stream = new MemoryStream();
                Image image = Image.FromFile(file.FileName);
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                byte[] bytes = stream.ToArray();
                string imagetortf = BitConverter.ToString(bytes, 0).Replace("-", string.Empty);
                StringBuilder ab = new StringBuilder();

                Graphics _graphics = richTextBox1.CreateGraphics();
                var xDpi = _graphics.DpiX;
                var yDpi = _graphics.DpiY;
                int picw = (int)Math.Round((image.Width / xDpi) * 2540);
                int pich = (int)Math.Round((image.Height / yDpi) * 2540);
                int picwgoal = (int)Math.Round((image.Width / xDpi) * 1440);
                int pichgoal = (int)Math.Round((image.Height / yDpi) * 1440);

                int rictextwgoal = (int)Math.Round((richTextBox1.Width / xDpi) * 1440);
                if (picwgoal >= rictextwgoal)
                {
                    picwgoal = rictextwgoal - 700;
                }

                ab.Append(@"{\rtf1{\pict\pngblip");
                ab.Append(@"\picw" + picw);
                ab.Append(@"\pich" + pich);
                ab.Append(@"\picwgoal" + picwgoal);
                ab.Append(@"\pichgoal" + pichgoal);
                ab.Append(@"\hex ");
                ab.Append(imagetortf + @"}\v image");
                richTextBox1.SelectedRtf = ab.ToString();

                string rtf_sadelesmis_byte = ExtractImgRtf(richTextBox1.Rtf);  //rtf içinden byte çekince ilk değerinden farklı değer çıkıyor...
                metinbaslangicIndex = (endIndex + 12);

                bfile.Add(new mail_send_user_bodyfile
                {
                    file_name = file.FileName,
                    bodyfile_string = rtf_sadelesmis_byte,
                    bodyfile_byte = bytes
                });
                ab.Clear();
            }
        }


        string ExtractImgRtf(string s)
        {
            startIndex = s.IndexOf("{\\pict", metinbaslangicIndex);
            int nextIndex1 = s.IndexOf("\\pichgoal", startIndex);
            int nextIndex2 = s.IndexOf(" ", nextIndex1);
            endIndex = s.IndexOf(@"}\v image\v0", nextIndex2);
            return s.Substring(nextIndex2, endIndex - nextIndex2);
        }
    }
}
