using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                string DosyaYolu = file.FileName;
                label3.Text = file.SafeFileName;
                mail1.gonderilen_mail_dosyalar = DosyaYolu;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mail1.yollayan_kisi_no = login_user.Instance.id;
            mail1.yolladıgımız_kisi = textBox1.Text;
            mail1.gonderilen_mail_konu = textBox2.Text;
            mail1.gonderilen_mail_icerik = richTextBox1.Text;

            if (kp4.form4_mail_gonderme(mail1) == true)
                MessageBox.Show("Mesaj Gönderildi!");
            else
                MessageBox.Show("Mesaj Gönderilemedi! -Hata");



        }
    }
}
