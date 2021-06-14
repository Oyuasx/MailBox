using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mail
{
    public class trash_get_user
    {
        public int id { get; set; }
        public int kisi_no { get; set; }
        public string yollayan_kisi { get; set; }
        public DateTimeOffset mail_alma_tarhi { get; set; }
        public string alınan_mail_konu { get; set; }
        public string alınan_mail_icerik { get; set; }

        public string tam_deger
        {
            get
            {
                return $"{alınan_mail_konu}     -/-     {yollayan_kisi}     -/-     {mail_alma_tarhi}";
            }
        }
    }
    public class trash_get_user_dosyalar
    {
        public int id { get; set; }
        public int kisi_no { get; set; }
        public int alınan_mail_no { get; set; }
        public byte[] alınan_mail_dosyalar { get; set; }
        public string attachment_name { get; set; }
    }

    public class trash_get_user_bodyfile
    {
        public int id { get; set; }
        public int kisi_no { get; set; }
        public int alınan_mail_no { get; set; }
        public Byte[] alınan_mail_bodyfile { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
    

}
