using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mail
{
    public class mail_get_user
    {
        public int alan_kisi_no { get; set; }
        public string yollayan_kisi { get; set; }
        public DateTimeOffset mail_alma_tarhi { get; set; }
        public string alınan_mail_konu { get; set; }
        public string alınan_mail_icerik { get; set; }
        public Byte[] alınan_mail_dosyalar { get; set; }

        public string tam_deger
        {
            get
            {
                return $"{alınan_mail_konu}-/-{yollayan_kisi}-{mail_alma_tarhi}";
            }
        }

    }
}
