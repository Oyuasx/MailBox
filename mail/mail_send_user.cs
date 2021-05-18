using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mail
{
    public class mail_send_user
    {
        public int yollayan_kisi_no { get; set;}
        public string yolladıgımız_kisi { get; set; }
        public string gonderilen_mail_konu { get; set; }
        public string gonderilen_mail_icerik { get; set; }
        public string gonderilen_mail_dosyalar { get; set; }        //hatalı olabilir
    }
}
