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
            MessageBox.Show("" + login_user.Instance.id);
            dgr_al = kp3.form3_db_cekme_islemler_baslik();
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
