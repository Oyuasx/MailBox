using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data; 
using System.Data.SqlClient;

using MailKit.Net.Smtp;
using System.Net;
using MailKit;
using MimeKit;
using MimeKit.Utils;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;

namespace mail
{
    class DataAccess
    {
        public SqlConnection con;
        string durum, convert;
        bool gonder;
        List<mail_get_user> dondur = new List<mail_get_user>();
        List<string> baslik = new List<string>();
        List<string> icerik = new List<string>();
        List<string> yollayan = new List<string>();
        List<DateTimeOffset> tarih = new List<DateTimeOffset>();
        int mail_alan_kisi;

        public DataAccess()
        {
            Connection();
        }
        public void Connection()
        {
            SqlConnection connection = new SqlConnection("Server=.\\SQLEXPRESS;Database=Mail;User Id=sa;Password=123456");
            con = connection;
        }

        public string bilgisayar_deger_kontrol(string IPV4, string pc_user_name)         //şuan ki pc, ipv4 nin değerlerini alır ve bu değerlerle eşleşem mail varmı diye kontrol eder.
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT TOP 1 IPV4,pc_user_name FROM login_user WHERE IPV4=@IPV4 AND pc_user_name=@pc_user_name ORDER BY tarih DESC";
                cmd.Parameters.AddWithValue("@IPV4", IPV4);
                cmd.Parameters.AddWithValue("@pc_user_name", pc_user_name);
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    using (SqlCommand cmd2 = new SqlCommand())
                    {
                        dr.Close();
                        cmd2.Connection = con;
                        cmd2.CommandText = "SELECT TOP 1 Eposta FROM login_user WHERE IPV4=@IPV4 AND pc_user_name=@pc_user_name ORDER BY tarih DESC";
                        cmd2.Parameters.AddWithValue("@IPV4", IPV4);
                        cmd2.Parameters.AddWithValue("@pc_user_name", pc_user_name);
                        cmd2.CommandType = CommandType.Text;
                        durum = (string)cmd2.ExecuteScalar();
                    }
                }
                else
                {
                    dr.Close();
                    durum = "";
                }
                con.Close();
                return durum;
            }
        }

        public void mail_baglanti_kontrol()
        {
            var client = new SmtpClient();

            client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
            client.Authenticate(login_user.Instance.Eposta, login_user.Instance.sifre);
            client.Disconnect(true);
        }
        public void verift_kod_gonder(int random_sayı)
        {
            var message = new MimeMessage();
            var builder = new BodyBuilder();
            var image = builder.LinkedResources.Add(@"image.jpg");
            image.ContentId = MimeUtils.GenerateMessageId();
            builder.HtmlBody = string.Format
                (
                    @"<b>XyzMail Doğrulama Kodunuz: </b> " + random_sayı +
                    @"<p>Bizi Tercih Ettiğiniz İçin teşekkür ederiz. <br/></p>

                    <img src=""cid:{0}"" width=500 height=200 > ", image.ContentId
                );

            message.From.Add(MailboxAddress.Parse("bugraverify@gmail.com"));
            message.To.Add(MailboxAddress.Parse(login_user.Instance.Eposta));
            message.Subject = "XyzMail - Güvenlik Doğrulama Kodu.";
            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                client.Authenticate("bugraverify@gmail.com", "31082000B");

                client.Send(message);
                client.Disconnect(true);
            }

        }

        public bool login_user_db_kontrol_posta() // db eposta kontrolü. (aşağıdaki fonksiyonlar birden fazla kez çağıralacağından bu şekilde yaptım)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT Eposta FROM login_user WHERE Eposta=@Eposta";
                cmd.Parameters.AddWithValue("@Eposta", login_user.Instance.Eposta);
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    using (SqlCommand cmd2 = new SqlCommand())
                    {
                        cmd2.Connection = con;
                        cmd2.CommandText = "SELECT id FROM login_user WHERE Eposta=@Eposta";
                        cmd2.Parameters.AddWithValue("@Eposta", login_user.Instance.Eposta);
                        cmd2.CommandType = CommandType.Text;
                        login_user.Instance.id = Convert.ToInt32(cmd2.ExecuteScalar());          //postanın id sini aldık (bundan sonra fonksiyonlarda aralık oluştururken işimizi kolaylaştıracak)
                        gonder = true;
                    }
                }
                else
                {
                    dr.Close();
                    gonder = false;
                }
                con.Close();
                return gonder;
            }
        }
        public bool login_user_db_kontrol_sifre()       // db sifre kontrolü.
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT sifre FROM login_user WHERE id=@id and sifre=@sifre ";   //giriş için kullandığımız şifre db'de var mı yok mu kontolü
                cmd.Parameters.AddWithValue("@id", login_user.Instance.id);
                cmd.Parameters.AddWithValue("@sifre", login_user.Instance.sifre);
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    gonder = true;
                }
                else
                {
                    dr.Close();
                    gonder = false;
                }
                con.Close();
                return gonder;
            }
        }
        public bool login_user_db_kontrol_ipv4_username() //başta yaptığımız ip-username kontorlünün tam tersi(burada belirli bir hesap için değerler uyuyormu diye kontrol etçez)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT IPV4,pc_user_name FROM login_user WHERE id=@id and IPV4=@IPV4 and pc_user_name=@pc_user_name";
                cmd.Parameters.AddWithValue("@id", login_user.Instance.id);
                cmd.Parameters.AddWithValue("@IPV4", login_user.Instance.IPV4);
                cmd.Parameters.AddWithValue("@pc_user_name", login_user.Instance.pc_user_name);
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    gonder = true;
                }
                else
                {
                    dr.Close();
                    gonder = false;
                }
                con.Close();
                return gonder;
            }
        }

        public void login_user_db_update_ipv4_username()    //ip-username update işlemi
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "update login_user set IPV4=@IPV4, pc_user_name=@pc_user_name where id=@id";
                cmd.Parameters.AddWithValue("@id", login_user.Instance.id);
                cmd.Parameters.AddWithValue("@IPV4", login_user.Instance.IPV4);
                cmd.Parameters.AddWithValue("@pc_user_name", login_user.Instance.pc_user_name);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void login_user_db_tarih()
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "update login_user set tarih=GETDATE() where id=@id";
                cmd.Parameters.AddWithValue("@id", login_user.Instance.id);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public bool login_user_db_islemler(bool internet_varmı)
        {
            if (internet_varmı == true)
            {
                if (login_user_db_kontrol_posta() == true)      //db de eposta varsa yapılacak işlemler
                {
                    if (login_user_db_kontrol_sifre() == false)  //db de şifre uymuyorsa yapılacak işlemler / uyuyorsa işlem yok
                    {
                        //db deki şifre farklı olmasına rağmen aynı eposta ile giriş yapıldıysa(+ eposta giriş kısmını geçerse) şifre değiştirilmiş demektir. update yapacağız
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = con;
                            con.Open();
                            cmd.CommandText = "update login_user set sifre=@sifre where id=@id";
                            cmd.Parameters.AddWithValue("@id", login_user.Instance.id);
                            cmd.Parameters.AddWithValue("@sifre", login_user.Instance.sifre);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                    if (login_user_db_kontrol_ipv4_username() == false)     //db de ip-username "uymuyorsa" yapılacak işlemler / uyuyorsa işlem yok
                        login_user_db_update_ipv4_username(); //ip - username update fonksiyonunu çağırıyoru
                }
                else
                {
                    using (SqlCommand cmd = new SqlCommand())   //db ekleme işlemi 
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.CommandText = "INSERT INTO login_user(Eposta,sifre,IPV4,pc_user_name) VALUES (@Eposta, @sifre, @IPV4, @pc_user_name)";
                        cmd.Parameters.AddWithValue("@Eposta", login_user.Instance.Eposta);
                        cmd.Parameters.AddWithValue("@sifre", login_user.Instance.sifre);
                        cmd.Parameters.AddWithValue("@IPV4", login_user.Instance.IPV4);
                        cmd.Parameters.AddWithValue("@pc_user_name", login_user.Instance.pc_user_name);

                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                login_user_db_tarih();
                return true;
            }
            else
            {
                if (login_user_db_kontrol_posta() == true)
                {
                    if (login_user_db_kontrol_sifre() == true)
                    {
                        if (login_user_db_kontrol_ipv4_username() == false)     //db de ip-username "uymuyorsa" yapılacak işlemler / uyuyorsa işlem yok
                            login_user_db_update_ipv4_username(); //ip - username update fonksiyonunu çağırıyor
                        login_user_db_tarih();
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }

        }



        //form3 kısmı  -hatalı kısımlar var bakçam

        public void form3_db_ekleme_islemler()   //false dönerse db ile maildeki değerler farklı olduğundan mail indirme işlemi olcak
        {
            List<string> baslik = new List<string>();
            List<string> icerik = new List<string>();
            List<string> yollayan = new List<string>();
            List<DateTimeOffset> tarih = new List<DateTimeOffset>();
            using (var client = new ImapClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);

                client.Authenticate(login_user.Instance.Eposta, login_user.Instance.sifre);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);
                int toplam_mesaj = inbox.Count;
                foreach (var summary in inbox.Fetch(0, -1, MessageSummaryItems.Full))
                {
                    baslik.Add(summary.Envelope.Subject);
                    tarih.Add((DateTimeOffset)summary.Envelope.Date);
                    convert = Convert.ToString(summary.Envelope.Sender);
                    yollayan.Add(convert);
                }

                foreach (var summary in inbox.Fetch(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure))
                {
                    if (summary.TextBody != null)
                    {
                        var text = (TextPart)inbox.GetBodyPart(summary.UniqueId, summary.TextBody);
                        icerik.Add(text.Text);
                    }
                    if (summary.HtmlBody != null)
                    {
                        // this will download *just* the text/html part
                        var html = (TextPart)inbox.GetBodyPart(summary.UniqueId, summary.HtmlBody);
                        icerik.Add(html.Text);
                    }
                }

                //db işlemleri

                for (int i = 0; i < toplam_mesaj; i++)
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        con.Open();
                        cmd.CommandText = "SELECT alınan_mail_konu,mail_alma_tarhi from mail_get_user where alınan_mail_konu=@alınan_mail_konu and mail_alma_tarhi=@mail_alma_tarhi and alan_kisi_no=@alan_kisi_no";
                        cmd.Parameters.AddWithValue("@alınan_mail_konu", baslik[i]);
                        cmd.Parameters.AddWithValue("@mail_alma_tarhi", tarih[i]);
                        cmd.Parameters.AddWithValue("@alan_kisi_no", login_user.Instance.id);
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            dr.Close();
                            cmd.Parameters.Clear();
                        }
                        else
                        {
                            cmd.Parameters.Clear();
                            dr.Close();
                            using (SqlCommand cmd2 = new SqlCommand())
                            {
                                cmd2.Connection = con;
                                cmd2.CommandText = "insert into mail_get_user(alan_kisi_no, yollayan_kisi, alınan_mail_konu,mail_alma_tarhi,alınan_mail_icerik) values (@alan_kisi_no, @yollayan_kisi, @alınan_mail_konu,@mail_alma_tarhi,@alınan_mail_icerik)";
                                cmd2.Parameters.AddWithValue("@alan_kisi_no", login_user.Instance.id);
                                cmd2.Parameters.AddWithValue("@yollayan_kisi", yollayan[i]);
                                cmd2.Parameters.AddWithValue("@alınan_mail_konu", baslik[i]);
                                cmd2.Parameters.AddWithValue("@mail_alma_tarhi", tarih[i]);
                                cmd2.Parameters.AddWithValue("@alınan_mail_icerik", icerik[i]);
                                cmd2.ExecuteNonQuery();
                                cmd2.Parameters.Clear();
                            }
                        }
                        con.Close();
                    }
                }
                client.Disconnect(true);
            }

        }




        public List<mail_get_user> form3_db_cekme_islemler_baslik()
        {
            mail_get_user maildb = new mail_get_user();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT alınan_mail_konu,mail_alma_tarhi,yollayan_kisi from mail_get_user where alan_kisi_no=@alan_kisi_no";
                cmd.Parameters.AddWithValue("@alan_kisi_no", login_user.Instance.id); ;
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    maildb.alan_kisi_no = mail_alan_kisi;
                    maildb.alınan_mail_konu = dr["alınan_mail_konu"].ToString();
                    maildb.yollayan_kisi = dr["yollayan_kisi"].ToString();
                    maildb.mail_alma_tarhi = (DateTimeOffset)dr["mail_alma_tarhi"];
                    dondur.Add(maildb);
                }
                dr.Close();
            }
            con.Close();
            return dondur;
        }






        //Form4 kısmı
        public bool form4_mail_gonderme(mail_send_user mail1)
        {
            try
            {
                var message = new MimeMessage();
                var builder = new BodyBuilder();
                builder.HtmlBody = string.Format(mail1.gonderilen_mail_icerik);

                message.From.Add(MailboxAddress.Parse(login_user.Instance.Eposta));
                message.To.Add(MailboxAddress.Parse(mail1.yolladıgımız_kisi));
                message.Subject = mail1.gonderilen_mail_konu;

                builder.Attachments.Add(@"" +mail1.gonderilen_mail_dosyalar);

                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                    client.Authenticate(login_user.Instance.Eposta, login_user.Instance.sifre);

                    client.Send(message);
                    client.Disconnect(true);
                }
                gonder = true;
            }
            catch (Exception)
            {
                gonder = false;
            }
            if (gonder == true)
            {
                DateTimeOffset dateTime = DateTimeOffset.Now;
                byte[] bytes = System.IO.File.ReadAllBytes(mail1.gonderilen_mail_dosyalar);           //yükleidğimiz dosyayı byte'a dönüştüyyor ki db ye kaydedebilelim.
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "insert into mail_send_user(yollayan_kisi_no,yolladıgımız_kisi,mail_yollama_tarhi,gonderilen_mail_konu,gonderilen_mail_icerik,gonderilen_mail_dosyalar) values (@yollayan_kisi_no, @yolladıgımız_kisi,@mail_yollama_tarhi ,@gonderilen_mail_konu ,@gonderilen_mail_icerik, @gonderilen_mail_dosyalar)";
                    cmd.Parameters.AddWithValue("@yollayan_kisi_no", mail1.yollayan_kisi_no);
                    cmd.Parameters.AddWithValue("@yolladıgımız_kisi", mail1.yolladıgımız_kisi);
                    cmd.Parameters.AddWithValue("@mail_yollama_tarhi", dateTime);
                    cmd.Parameters.AddWithValue("@gonderilen_mail_konu", mail1.gonderilen_mail_konu);
                    cmd.Parameters.AddWithValue("@gonderilen_mail_icerik", mail1.gonderilen_mail_icerik);
                    cmd.Parameters.AddWithValue("@gonderilen_mail_dosyalar", bytes);
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return true;
            }
            else
                return false;
        }
    }
}