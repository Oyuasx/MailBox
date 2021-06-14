using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mail
{
    public class mail_send_user
    {
        public int id { get; set; }
        public int kisi_no { get; set;}
        public DateTimeOffset mail_yollama_tarhi { get; set; }
        public string gonderilen_mail_konu { get; set; }
        public string gonderilen_mail_icerik { get; set; }


        public string tam_deger
        {
            get
            {
                return $"{gonderilen_mail_konu}     -/-     {login_user.Instance.Eposta}     -/-     {mail_yollama_tarhi}";
            }
        }
    }
    public class mail_send_user_dosyalar
    {
        public int id { get; set; }
        public int kisi_no { get; set; }
        public int gonderilen_mail_no { get; set; }
        public byte[] gonderilen_mail_dosyalar { get; set; }
        public string attachment_name { get; set; }
    }

    public class mail_send_user_bodyfile
    {
        public int id { get; set; }
        public int kisi_no { get; set; }
        public int gonderilen_mail_no { get; set; }
        public Byte[] gonderilen_mail_bodyfile { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

}
