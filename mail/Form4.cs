using MarkupConverter;
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
        List<string> dosya_dondur = new List<string>();

        private void Form4_Load(object sender, EventArgs e)
        {
        }
        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Multiselect=true;
            if (file.ShowDialog() == DialogResult.OK)
            {
                foreach(string dosya_Ad in file.FileNames)
                {
                    dosya_dondur.Add(dosya_Ad);
                    listBox1.Items.Add(dosya_Ad);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dosya_dondur.Clear();
            listBox1.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Boş değer girilemez!!! -Hata");
            }
            else
            {
                mail1.yollayan_kisi_no = login_user.Instance.id;
                mail1.yolladıgımız_kisi = textBox1.Text;
                mail1.gonderilen_mail_konu = textBox2.Text;
                string text;
                try
                {
                    IMarkupConverter markupConverter = new MarkupConverter.MarkupConverter();
                    text = markupConverter.ConvertRtfToHtml(richTextBox1.Rtf);
                }
                catch (Exception)
                {
                    text = richTextBox1.Text;
                }
                mail1.gonderilen_mail_icerik = text;
                try
                {
                    if (kp4.form4_mail_gonderme(mail1, dosya_dondur) == true)
                        MessageBox.Show("Mesaj Gönderildi!");
                    else
                        MessageBox.Show("Mesaj Gönderilemedi! -Hata");
                }
                catch(Exception ht)
                {
                    MessageBox.Show(""+ht);
                }
            }
        }

        /*
         //richtext box içine resim ekleme işlemleri
        private void button6_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.png; *.bmp)|*.jpg; *.jpeg; *.gif; *.png; *.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Image image = Image.FromFile(ofd.FileName);
                    MemoryStream stream = new MemoryStream();
                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                    richTextBox1.InsertImage(image);
                }
            }
        }
        */
    }
}
