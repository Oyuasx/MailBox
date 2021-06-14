using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mail
{
    public class login_user
    {
        public int id { get; set; }
        public string Eposta { get; set; }
        public string sifre { get; set; }
        public string IPV4 { get; set; }
        public string pc_user_name { get; set; }

        public login_user() {}
        public static login_user Instance = new login_user();
    }
}
