using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;



namespace mail
{
    class DataAccess
    {
        public SqlConnection con;
        public DataAccess()
        {
            Connection();
        }
        public void Connection()
        {
            SqlConnection connection = new SqlConnection("Server=.\\SQLEXPRESS;Database=Mail;User Id=sa;Password=12345678");
            con = connection;
        }

        public string bilgisayar_deger_kontrol(string IPV4, string pc_user_name)         //şuan ki pc, ipv4 nin değerlerini alır ve bu değerlerle eşleşem mail varmı diye kontrol eder.
        {
            string durum;
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


        bool gonder;
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
                    login_user_db_tarih();
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
                    using (SqlCommand cmd2 = new SqlCommand())
                    {
                        cmd2.Connection = con;
                        con.Open();
                        cmd2.CommandText = "SELECT id FROM login_user WHERE Eposta=@Eposta";
                        cmd2.Parameters.AddWithValue("@Eposta", login_user.Instance.Eposta);
                        cmd2.CommandType = CommandType.Text;
                        login_user.Instance.id = Convert.ToInt32(cmd2.ExecuteScalar());
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



        //Form3 kısmı


        public void form3_db_ekleme_islemler_gelen_mesaj(int toplam_mesaj, List<mail_tut> mail_tut, List<mail_bodyfile_tut> mail_tut_bodyfile, List<mail_attachment_tut> mail_tut_attechment)
        {
            int mail_id;
            foreach (var x in mail_tut) 
            { 
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT kisi_no,yollayan_kisi,mail_alma_tarhi,alınan_mail_konu,alınan_mail_icerik from mail_get_user where kisi_no = @kisi_no and yollayan_kisi = @yollayan_kisi and mail_alma_tarhi = @mail_alma_tarhi and alınan_mail_konu = @alınan_mail_konu and alınan_mail_icerik = @alınan_mail_icerik";
                    cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                    cmd.Parameters.AddWithValue("@yollayan_kisi", x.yollayan);
                    cmd.Parameters.AddWithValue("@mail_alma_tarhi", x.tarih);
                    cmd.Parameters.AddWithValue("@alınan_mail_konu", x.baslik);
                    cmd.Parameters.AddWithValue("@alınan_mail_icerik", x.icerik);
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        dr.Close();
                    }
                    else
                    {
                        dr.Close();
                        using (SqlCommand cmd2 = new SqlCommand())
                        {
                            cmd2.Connection = con;
                            cmd2.CommandText = "insert into mail_get_user( kisi_no, yollayan_kisi, mail_alma_tarhi, alınan_mail_konu, alınan_mail_icerik) values (@kisi_no, @yollayan_kisi, @mail_alma_tarhi, @alınan_mail_konu, @alınan_mail_icerik)";
                            cmd2.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                            cmd2.Parameters.AddWithValue("@yollayan_kisi", x.yollayan);
                            cmd2.Parameters.AddWithValue("@mail_alma_tarhi", x.tarih);
                            cmd2.Parameters.AddWithValue("@alınan_mail_konu", x.baslik);
                            cmd2.Parameters.AddWithValue("@alınan_mail_icerik", x.icerik);
                            cmd2.CommandType = CommandType.Text;
                            cmd2.ExecuteNonQuery();
                            cmd2.Parameters.Clear();
                        }

                        using (SqlCommand cmd3 = new SqlCommand())
                        {
                            cmd3.Connection = con;
                            cmd3.CommandText = "SELECT id FROM mail_get_user WHERE kisi_no=@kisi_no and yollayan_kisi=@yollayan_kisi and mail_alma_tarhi=@mail_alma_tarhi and alınan_mail_konu=@alınan_mail_konu";
                            cmd3.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                            cmd3.Parameters.AddWithValue("@yollayan_kisi", x.yollayan);
                            cmd3.Parameters.AddWithValue("@mail_alma_tarhi", x.tarih);
                            cmd3.Parameters.AddWithValue("@alınan_mail_konu", x.baslik);
                            cmd3.CommandType = CommandType.Text;
                            mail_id = Convert.ToInt32(cmd3.ExecuteScalar());
                            cmd3.Parameters.Clear();
                        }
                        foreach (var y in mail_tut_bodyfile)
                        {
                            if (x.id == y.id) 
                            {
                                using (SqlCommand cmd4 = new SqlCommand())
                                {
                                    cmd4.Connection = con;
                                    cmd4.CommandText = "insert into mail_get_user_bodyfile(alınan_mail_no, alınan_mail_bodyfile, width, height, kisi_no) values (@alınan_mail_no, @alınan_mail_bodyfile, @width, @height, @kisi_no)";
                                    cmd4.Parameters.AddWithValue("@alınan_mail_no", mail_id);
                                    cmd4.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                                    cmd4.Parameters.AddWithValue("@alınan_mail_bodyfile", y.alınan_mail_bodyfile);
                                    cmd4.Parameters.AddWithValue("@width", y.width);
                                    cmd4.Parameters.AddWithValue("@height", y.height);
                                    cmd4.CommandType = CommandType.Text;
                                    cmd4.ExecuteNonQuery();    
                                    cmd4.Parameters.Clear();
                                }
                            }

                        }
                       
                        foreach (var y in mail_tut_attechment)
                        {
                            if (x.id == y.id)
                            {
                                using (SqlCommand cmd5 = new SqlCommand())
                                {
                                    cmd5.Connection = con;
                                    cmd5.CommandText = "insert into mail_get_user_dosyalar(alınan_mail_no, kisi_no, alınan_mail_dosyalar, attachment_name) values (@alınan_mail_no, @kisi_no, @alınan_mail_dosyalar, @attachment_name)";
                                    cmd5.Parameters.AddWithValue("@alınan_mail_no", mail_id);
                                    cmd5.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                                    cmd5.Parameters.AddWithValue("@alınan_mail_dosyalar", y.alınan_mail_attachment);
                                    cmd5.Parameters.AddWithValue("@attachment_name", y.attachment_name);
                                    cmd5.CommandType = CommandType.Text;
                                    cmd5.ExecuteNonQuery();
                                    cmd5.Parameters.Clear();
                                }
                            }
                        }
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
            }
            int db_toplam_mesaj;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "Select count(kisi_no) from mail_get_user";
                cmd.CommandType = CommandType.Text;
                db_toplam_mesaj = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
            }
            if (toplam_mesaj < db_toplam_mesaj)
            {
                List<int> degertut_id = new List<int>();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT alınan_mail_konu,mail_alma_tarhi,yollayan_kisi,id from mail_get_user where kisi_no=@kisi_no";
                    cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        bool esittir = false;
                        for (int i = 0; i < toplam_mesaj; i++)
                        {
                            if (dr["alınan_mail_konu"].ToString() == mail_tut[i].baslik && (DateTimeOffset)dr["mail_alma_tarhi"] == mail_tut[i].tarih && dr["yollayan_kisi"].ToString() == mail_tut[i].yollayan)
                            {
                                esittir = true;
                            }
                        }
                        if (esittir != true)
                        {
                            degertut_id.Add((Int32)dr["id"]);
                        } 
                    }
                    dr.Close();
                    foreach (int tut in degertut_id)
                    {
                        using (SqlCommand cmd2 = new SqlCommand())
                        {
                            cmd2.Connection = con;
                            cmd2.CommandText = "Delete from mail_get_user where id=@id and kisi_no=@kisi_no";
                            cmd2.Parameters.AddWithValue("@id", tut);
                            cmd2.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                            cmd2.CommandType = CommandType.Text;
                            cmd2.ExecuteNonQuery();
                            cmd2.Parameters.Clear();
                        }
                        using (SqlCommand cmd3 = new SqlCommand())
                        {
                            cmd3.Connection = con;
                            cmd3.CommandText = "Delete from mail_get_user_dosyalar where id=@id";
                            cmd3.Parameters.AddWithValue("@id", tut);
                            cmd3.CommandType = CommandType.Text;
                            cmd3.ExecuteNonQuery();
                            cmd3.Parameters.Clear();
                        }
                        using (SqlCommand cmd4 = new SqlCommand())
                        {
                            cmd4.Connection = con;
                            cmd4.CommandText = "Delete from mail_get_user_bodyfile where id=@id";
                            cmd4.Parameters.AddWithValue("@id", tut);
                            cmd4.CommandType = CommandType.Text;
                            cmd4.ExecuteNonQuery();
                            cmd4.Parameters.Clear();
                        }
                    }
                    con.Close();
                }
            }
        }

        public void form3_db_ekleme_islemler_gonderilen_mesaj(int toplam_mesaj, List<mail_tut> mail_tut, List<mail_bodyfile_tut> mail_tut_bodyfile, List<mail_attachment_tut> mail_tut_attechment)
        {
            int mail_id;
            foreach (var x in mail_tut)
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT kisi_no,mail_yollama_tarhi,gonderilen_mail_konu,gonderilen_mail_icerik from mail_send_user where kisi_no = @kisi_no and mail_yollama_tarhi = @mail_yollama_tarhi and gonderilen_mail_konu = @gonderilen_mail_konu and gonderilen_mail_icerik = @gonderilen_mail_icerik";
                    cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                    cmd.Parameters.AddWithValue("@mail_yollama_tarhi", x.tarih);
                    cmd.Parameters.AddWithValue("@gonderilen_mail_konu", x.baslik);
                    cmd.Parameters.AddWithValue("@gonderilen_mail_icerik", x.icerik);
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        dr.Close();
                    }
                    else
                    {
                        dr.Close();
                        using (SqlCommand cmd2 = new SqlCommand())
                        {
                            cmd2.Connection = con;
                            cmd2.CommandText = "insert into mail_send_user(kisi_no, mail_yollama_tarhi, gonderilen_mail_konu, gonderilen_mail_icerik) values (@kisi_no, @mail_yollama_tarhi, @gonderilen_mail_konu, @gonderilen_mail_icerik)";
                            cmd2.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                            cmd2.Parameters.AddWithValue("@mail_yollama_tarhi", x.tarih);
                            cmd2.Parameters.AddWithValue("@gonderilen_mail_konu", x.baslik);
                            cmd2.Parameters.AddWithValue("@gonderilen_mail_icerik", x.icerik);
                            cmd2.CommandType = CommandType.Text;
                            cmd2.ExecuteNonQuery();
                            cmd2.Parameters.Clear();
                        }

                        using (SqlCommand cmd3 = new SqlCommand())
                        {
                            cmd3.Connection = con;
                            cmd3.CommandText = "SELECT id FROM mail_send_user WHERE kisi_no=@kisi_no and mail_yollama_tarhi=@mail_yollama_tarhi and gonderilen_mail_konu=@gonderilen_mail_konu";
                            cmd3.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                            cmd3.Parameters.AddWithValue("@mail_yollama_tarhi", x.tarih);
                            cmd3.Parameters.AddWithValue("@gonderilen_mail_konu", x.baslik);
                            cmd3.CommandType = CommandType.Text;
                            mail_id = Convert.ToInt32(cmd3.ExecuteScalar());
                            cmd3.Parameters.Clear();
                        }
                        foreach (var y in mail_tut_bodyfile)
                        {
                            if (x.id == y.id)
                            {
                                using (SqlCommand cmd4 = new SqlCommand())
                                {
                                    cmd4.Connection = con;
                                    cmd4.CommandText = "insert into mail_send_user_bodyfile(gonderilen_mail_no, kisi_no, gonderilen_mail_bodyfile, width, height) values (@gonderilen_mail_no, @kisi_no, @gonderilen_mail_bodyfile, @width, @height)";
                                    cmd4.Parameters.AddWithValue("@gonderilen_mail_no", mail_id);
                                    cmd4.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                                    cmd4.Parameters.AddWithValue("@gonderilen_mail_bodyfile", y.alınan_mail_bodyfile);
                                    cmd4.Parameters.AddWithValue("@width", y.width);
                                    cmd4.Parameters.AddWithValue("@height", y.height);
                                    cmd4.CommandType = CommandType.Text;
                                    cmd4.ExecuteNonQuery();
                                    cmd4.Parameters.Clear();
                                }
                            }

                        }

                        foreach (var y in mail_tut_attechment)
                        {
                            if (x.id == y.id)
                            {
                                using (SqlCommand cmd5 = new SqlCommand())
                                {
                                    cmd5.Connection = con;
                                    cmd5.CommandText = "insert into mail_send_user_dosyalar(gonderilen_mail_no, kisi_no, gonderilen_mail_dosyalar, attachment_name) values (@gonderilen_mail_no, @kisi_no, @gonderilen_mail_dosyalar, @attachment_name)";
                                    cmd5.Parameters.AddWithValue("@gonderilen_mail_no", mail_id);
                                    cmd5.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                                    cmd5.Parameters.AddWithValue("@gonderilen_mail_dosyalar", y.alınan_mail_attachment);
                                    cmd5.Parameters.AddWithValue("@attachment_name", y.attachment_name);
                                    cmd5.CommandType = CommandType.Text;
                                    cmd5.ExecuteNonQuery();
                                    cmd5.Parameters.Clear();
                                }
                            }
                        }
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
            }
            int db_toplam_mesaj;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "Select count(kisi_no) from mail_send_user";
                cmd.CommandType = CommandType.Text;
                db_toplam_mesaj = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
            }
            if (toplam_mesaj < db_toplam_mesaj)
            {
                List<int> degertut_id = new List<int>();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT gonderilen_mail_konu,mail_yollama_tarhi,id from mail_send_user where kisi_no=@kisi_no";
                    cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        bool esittir = false;
                        for (int i = 0; i < toplam_mesaj; i++)
                        {
                            if (dr["gonderilen_mail_konu"].ToString() == mail_tut[i].baslik && (DateTimeOffset)dr["mail_yollama_tarhi"] == mail_tut[i].tarih)
                            {
                                esittir = true;
                            }
                        }
                        if (esittir != true)
                        {
                            degertut_id.Add((Int32)dr["id"]);
                        }
                    }
                    dr.Close();
                    foreach (int tut in degertut_id)
                    {
                        using (SqlCommand cmd2 = new SqlCommand())
                        {
                            cmd2.Connection = con;
                            cmd2.CommandText = "Delete from mail_send_user where id=@id and kisi_no=@kisi_no";
                            cmd2.Parameters.AddWithValue("@id", tut);
                            cmd2.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                            cmd2.CommandType = CommandType.Text;
                            cmd2.ExecuteNonQuery();
                            cmd2.Parameters.Clear();
                        }
                        using (SqlCommand cmd3 = new SqlCommand())
                        {
                            cmd3.Connection = con;
                            cmd3.CommandText = "Delete from mail_send_user_dosyalar where id=@id";
                            cmd3.Parameters.AddWithValue("@id", tut);
                            cmd3.CommandType = CommandType.Text;
                            cmd3.ExecuteNonQuery();
                            cmd3.Parameters.Clear();
                        }
                        using (SqlCommand cmd4 = new SqlCommand())
                        {
                            cmd4.Connection = con;
                            cmd4.CommandText = "Delete from mail_send_user_bodyfile where id=@id";
                            cmd4.Parameters.AddWithValue("@id", tut);
                            cmd4.CommandType = CommandType.Text;
                            cmd4.ExecuteNonQuery();
                            cmd4.Parameters.Clear();
                        }
                    }
                    con.Close();
                }
            }
        }



        public void form3_db_ekleme_islemler_trash_mesaj(int toplam_mesaj, List<mail_tut> mail_tut, List<mail_bodyfile_tut> mail_tut_bodyfile, List<mail_attachment_tut> mail_tut_attechment)
        {
            int mail_id;
            foreach (var x in mail_tut)
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT kisi_no,yollayan_kisi,mail_alma_tarhi,alınan_mail_konu,alınan_mail_icerik from trash_get_user where kisi_no = @kisi_no and yollayan_kisi = @yollayan_kisi and mail_alma_tarhi = @mail_alma_tarhi and alınan_mail_konu = @alınan_mail_konu and alınan_mail_icerik = @alınan_mail_icerik";
                    cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                    cmd.Parameters.AddWithValue("@yollayan_kisi", x.yollayan);
                    cmd.Parameters.AddWithValue("@mail_alma_tarhi", x.tarih);
                    cmd.Parameters.AddWithValue("@alınan_mail_konu", x.baslik);
                    cmd.Parameters.AddWithValue("@alınan_mail_icerik", x.icerik);
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        dr.Close();
                    }
                    else
                    {
                        dr.Close();
                        using (SqlCommand cmd2 = new SqlCommand())
                        {
                            cmd2.Connection = con;
                            cmd2.CommandText = "insert into trash_get_user(kisi_no, yollayan_kisi, mail_alma_tarhi, alınan_mail_konu, alınan_mail_icerik) values (@kisi_no, @yollayan_kisi, @mail_alma_tarhi, @alınan_mail_konu, @alınan_mail_icerik)";
                            cmd2.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                            cmd2.Parameters.AddWithValue("@yollayan_kisi", x.yollayan);
                            cmd2.Parameters.AddWithValue("@mail_alma_tarhi", x.tarih);
                            cmd2.Parameters.AddWithValue("@alınan_mail_konu", x.baslik);
                            cmd2.Parameters.AddWithValue("@alınan_mail_icerik", x.icerik);
                            cmd2.CommandType = CommandType.Text;
                            cmd2.ExecuteNonQuery();
                            cmd2.Parameters.Clear();
                        }

                        using (SqlCommand cmd3 = new SqlCommand())
                        {
                            cmd3.Connection = con;
                            cmd3.CommandText = "SELECT id FROM trash_get_user WHERE kisi_no=@kisi_no and yollayan_kisi=@yollayan_kisi and mail_alma_tarhi=@mail_alma_tarhi and alınan_mail_konu=@alınan_mail_konu";
                            cmd3.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                            cmd3.Parameters.AddWithValue("@yollayan_kisi", x.yollayan);
                            cmd3.Parameters.AddWithValue("@mail_alma_tarhi", x.tarih);
                            cmd3.Parameters.AddWithValue("@alınan_mail_konu", x.baslik);
                            cmd3.CommandType = CommandType.Text;
                            mail_id = Convert.ToInt32(cmd3.ExecuteScalar());
                            cmd3.Parameters.Clear();
                        }
                        foreach (var y in mail_tut_bodyfile)
                        {
                            if (x.id == y.id)
                            {
                                using (SqlCommand cmd4 = new SqlCommand())
                                {
                                    cmd4.Connection = con;
                                    cmd4.CommandText = "insert into trash_get_user_bodyfile(alınan_mail_no, alınan_mail_bodyfile, width, height, kisi_no) values (@alınan_mail_no, @alınan_mail_bodyfile, @width, @height, @kisi_no)";
                                    cmd4.Parameters.AddWithValue("@alınan_mail_no", mail_id);
                                    cmd4.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                                    cmd4.Parameters.AddWithValue("@alınan_mail_bodyfile", y.alınan_mail_bodyfile);
                                    cmd4.Parameters.AddWithValue("@width", y.width);
                                    cmd4.Parameters.AddWithValue("@height", y.height);
                                    cmd4.CommandType = CommandType.Text;
                                    cmd4.ExecuteNonQuery();   
                                    cmd4.Parameters.Clear();
                                }
                            }

                        }

                        foreach (var y in mail_tut_attechment)
                        {
                            if (x.id == y.id)
                            {
                                using (SqlCommand cmd5 = new SqlCommand())
                                {
                                    cmd5.Connection = con;
                                    cmd5.CommandText = "insert into trash_get_user_dosyalar(alınan_mail_no, kisi_no, alınan_mail_dosyalar, attachment_name) values (@alınan_mail_no, @kisi_no, @alınan_mail_dosyalar, @attachment_name)";
                                    cmd5.Parameters.AddWithValue("@alınan_mail_no", mail_id);
                                    cmd5.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                                    cmd5.Parameters.AddWithValue("@alınan_mail_dosyalar", y.alınan_mail_attachment);
                                    cmd5.Parameters.AddWithValue("@attachment_name", y.attachment_name);

                                    cmd5.CommandType = CommandType.Text;
                                    cmd5.ExecuteNonQuery();
                                    cmd5.Parameters.Clear();
                                }
                            }
                        }
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
            }
            int db_toplam_mesaj;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "Select count(kisi_no) from trash_get_user";
                cmd.CommandType = CommandType.Text;
                db_toplam_mesaj = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();
            }
            if (toplam_mesaj < db_toplam_mesaj)
            {
                List<int> degertut_id = new List<int>();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "SELECT alınan_mail_konu,mail_alma_tarhi,yollayan_kisi,id from trash_get_user where kisi_no=@kisi_no";
                    cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        bool esittir = false;
                        for (int i = 0; i < toplam_mesaj; i++)
                        {
                            if (dr["alınan_mail_konu"].ToString() == mail_tut[i].baslik && (DateTimeOffset)dr["mail_alma_tarhi"] == mail_tut[i].tarih && dr["yollayan_kisi"].ToString() == mail_tut[i].yollayan)
                            {
                                esittir = true;
                            }
                        }
                        if (esittir != true)
                        {
                            degertut_id.Add((Int32)dr["id"]);
                        }
                    }
                    dr.Close();
                    foreach (int tut in degertut_id)
                    {
                        using (SqlCommand cmd2 = new SqlCommand())
                        {
                            cmd2.Connection = con;
                            cmd2.CommandText = "Delete from trash_get_user where id=@id and kisi_no=@kisi_no";
                            cmd2.Parameters.AddWithValue("@id", tut);
                            cmd2.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                            cmd2.CommandType = CommandType.Text;
                            cmd2.ExecuteNonQuery();
                            cmd2.Parameters.Clear();
                        }
                        using (SqlCommand cmd3 = new SqlCommand())
                        {
                            cmd3.Connection = con;
                            cmd3.CommandText = "Delete from trash_get_user_dosyalar where id=@id";
                            cmd3.Parameters.AddWithValue("@id", tut);
                            cmd3.CommandType = CommandType.Text;
                            cmd3.ExecuteNonQuery();
                            cmd3.Parameters.Clear();
                        }
                        using (SqlCommand cmd4 = new SqlCommand())
                        {
                            cmd4.Connection = con;
                            cmd4.CommandText = "Delete from trash_get_user_bodyfile where id=@id";
                            cmd4.Parameters.AddWithValue("@id", tut);
                            cmd4.CommandType = CommandType.Text;
                            cmd4.ExecuteNonQuery();
                            cmd4.Parameters.Clear();
                        }
                    }
                    con.Close();
                }
            }
        }



        public void form3_secilen_degeri_sil_1(int donen_id, List<mail_get_user> mail_get, List<mail_get_user_dosyalar> mail_get_attach, List<mail_get_user_bodyfile> mail_get_body)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "Delete from mail_get_user where id=@id and kisi_no=@kisi_no";
                cmd.Parameters.AddWithValue("@id", donen_id);
                cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                using (SqlCommand cmd1 = new SqlCommand())
                {
                    cmd1.Connection = con;
                    cmd1.CommandText = "Delete from mail_get_user_dosyalar where alınan_mail_no=@alınan_mail_no and kisi_no=@kisi_no";
                    cmd1.Parameters.AddWithValue("@alınan_mail_no", donen_id);
                    cmd1.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                    cmd1.CommandType = CommandType.Text;
                    cmd1.ExecuteNonQuery();
                    using (SqlCommand cmd2 = new SqlCommand())
                    {
                        cmd2.Connection = con;
                        cmd2.CommandText = "Delete from mail_get_user_bodyfile where alınan_mail_no=@alınan_mail_no and kisi_no=@kisi_no";
                        cmd2.Parameters.AddWithValue("@alınan_mail_no", donen_id);
                        cmd2.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                        cmd2.CommandType = CommandType.Text;
                        cmd2.ExecuteNonQuery();
                    }
                }
                con.Close();
            }
            int mail_id;
            using (SqlCommand cmd = new SqlCommand())
            {
                con.Open();
                mail_get_user x = mail_get.Find(result => result.id == donen_id);

                if (x.id == donen_id)
                {
                    cmd.Connection = con;
                    cmd.CommandText = "insert into trash_get_user(kisi_no, yollayan_kisi, mail_alma_tarhi, alınan_mail_konu, alınan_mail_icerik) values (@kisi_no, @yollayan_kisi, @mail_alma_tarhi, @alınan_mail_konu, @alınan_mail_icerik)";
                    cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                    cmd.Parameters.AddWithValue("@yollayan_kisi", x.yollayan_kisi);
                    cmd.Parameters.AddWithValue("@mail_alma_tarhi", x.mail_alma_tarhi);
                    cmd.Parameters.AddWithValue("@alınan_mail_konu", x.alınan_mail_konu);
                    cmd.Parameters.AddWithValue("@alınan_mail_icerik", x.alınan_mail_icerik);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    using (SqlCommand cmd0 = new SqlCommand())
                    {
                        cmd0.Connection = con;
                        cmd0.CommandText = "SELECT id FROM trash_get_user WHERE kisi_no=@kisi_no and yollayan_kisi=@yollayan_kisi and mail_alma_tarhi=@mail_alma_tarhi and alınan_mail_konu=@alınan_mail_konu";
                        cmd0.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                        cmd0.Parameters.AddWithValue("@yollayan_kisi", x.yollayan_kisi);
                        cmd0.Parameters.AddWithValue("@mail_alma_tarhi", x.mail_alma_tarhi);
                        cmd0.Parameters.AddWithValue("@alınan_mail_konu", x.alınan_mail_konu);
                        cmd0.CommandType = CommandType.Text;
                        mail_id = Convert.ToInt32(cmd0.ExecuteScalar());
                    }
                    foreach (var y in mail_get_attach)
                    {
                        if (x.id == y.alınan_mail_no)
                        {
                            using (SqlCommand cmd1 = new SqlCommand())
                            {
                                cmd1.Connection = con;
                                cmd1.CommandText = "insert into trash_get_user_dosyalar(alınan_mail_no, kisi_no, alınan_mail_dosyalar) values (@alınan_mail_no, @kisi_no, @alınan_mail_dosyalar)";
                                cmd1.Parameters.AddWithValue("@alınan_mail_no", mail_id);
                                cmd1.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                                cmd1.Parameters.AddWithValue("@alınan_mail_dosyalar", y.alınan_mail_dosyalar);
                                cmd1.CommandType = CommandType.Text;
                                cmd1.ExecuteNonQuery();
                                cmd1.Parameters.Clear();
                            }
                        }
                    }
                    foreach (var z in mail_get_body)
                    {
                        if (x.id == z.alınan_mail_no)
                        {
                            using (SqlCommand cmd2 = new SqlCommand())
                            {
                                cmd2.Connection = con;
                                cmd2.CommandText = "insert into trash_get_user_bodyfile(alınan_mail_no, alınan_mail_bodyfile, width, height, kisi_no) values (@alınan_mail_no, @alınan_mail_bodyfile, @width, @height, @kisi_no)";
                                cmd2.Parameters.AddWithValue("@alınan_mail_no", mail_id);
                                cmd2.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                                cmd2.Parameters.AddWithValue("@alınan_mail_bodyfile", z.alınan_mail_bodyfile);
                                cmd2.Parameters.AddWithValue("@width", z.width);
                                cmd2.Parameters.AddWithValue("@height", z.height);
                                cmd2.CommandType = CommandType.Text;
                                cmd2.ExecuteNonQuery();
                                cmd2.Parameters.Clear();
                            }
                        }
                    }
                }
                con.Close();
            }
        }



        public void form3_secilen_degeri_sil_2(int donen_id, List<mail_send_user> mail_get, List<mail_send_user_dosyalar> mail_get_attach, List<mail_send_user_bodyfile> mail_get_body)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "Delete from mail_send_user where id=@id and kisi_no=@kisi_no";
                cmd.Parameters.AddWithValue("@id", donen_id);
                cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                using (SqlCommand cmd1 = new SqlCommand())
                {
                    cmd1.Connection = con;
                    cmd1.CommandText = "Delete from mail_send_user_dosyalar where gonderilen_mail_no=@gonderilen_mail_no and kisi_no=@kisi_no";
                    cmd1.Parameters.AddWithValue("@gonderilen_mail_no", donen_id);
                    cmd1.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                    cmd1.CommandType = CommandType.Text;
                    cmd1.ExecuteNonQuery();
                    using (SqlCommand cmd2 = new SqlCommand())
                    {
                        cmd2.Connection = con;
                        cmd2.CommandText = "Delete from mail_send_user_bodyfile where gonderilen_mail_no=@gonderilen_mail_no and kisi_no=@kisi_no";
                        cmd2.Parameters.AddWithValue("@gonderilen_mail_no", donen_id);
                        cmd2.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                        cmd2.CommandType = CommandType.Text;
                        cmd2.ExecuteNonQuery();
                    }
                }
                con.Close();
            }
            int mail_id;
            using (SqlCommand cmd = new SqlCommand())
            {
                con.Open();
                mail_send_user x = mail_get.Find(result => result.id == donen_id);

                if (x.id == donen_id)
                {
                    cmd.Connection = con;
                    cmd.CommandText = "insert into trash_get_user(kisi_no, yollayan_kisi, mail_alma_tarhi, alınan_mail_konu, alınan_mail_icerik) values (@kisi_no, @yollayan_kisi, @mail_alma_tarhi, @alınan_mail_konu, @alınan_mail_icerik)";
                    cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                    cmd.Parameters.AddWithValue("@yollayan_kisi", login_user.Instance.Eposta);
                    cmd.Parameters.AddWithValue("@mail_alma_tarhi", x.mail_yollama_tarhi);
                    cmd.Parameters.AddWithValue("@alınan_mail_konu", x.gonderilen_mail_konu);
                    cmd.Parameters.AddWithValue("@alınan_mail_icerik", x.gonderilen_mail_icerik);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    using (SqlCommand cmd0 = new SqlCommand())
                    {
                        cmd0.Connection = con;
                        cmd0.CommandText = "SELECT id FROM trash_get_user WHERE kisi_no=@kisi_no and yollayan_kisi=@yollayan_kisi and mail_alma_tarhi=@mail_alma_tarhi and alınan_mail_konu=@alınan_mail_konu";
                        cmd0.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                        cmd0.Parameters.AddWithValue("@yollayan_kisi", login_user.Instance.Eposta);
                        cmd0.Parameters.AddWithValue("@mail_alma_tarhi", x.mail_yollama_tarhi);
                        cmd0.Parameters.AddWithValue("@alınan_mail_konu", x.gonderilen_mail_konu);
                        cmd0.CommandType = CommandType.Text;
                        mail_id = Convert.ToInt32(cmd0.ExecuteScalar());
                    }
                    foreach (var y in mail_get_attach)
                    {
                        if (x.id == y.gonderilen_mail_no)
                        {
                            using (SqlCommand cmd1 = new SqlCommand())
                            {
                                cmd1.Connection = con;
                                cmd1.CommandText = "insert into trash_get_user_dosyalar(alınan_mail_no, kisi_no, alınan_mail_dosyalar) values (@alınan_mail_no, @kisi_no, @alınan_mail_dosyalar)";
                                cmd1.Parameters.AddWithValue("@alınan_mail_no", mail_id);
                                cmd1.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                                cmd1.Parameters.AddWithValue("@alınan_mail_dosyalar", y.gonderilen_mail_dosyalar);
                                cmd1.CommandType = CommandType.Text;
                                cmd1.ExecuteNonQuery();
                                cmd1.Parameters.Clear();
                            }
                        }
                    }
                    foreach (var z in mail_get_body)
                    {
                        if (x.id == z.gonderilen_mail_no)
                        {
                            using (SqlCommand cmd2 = new SqlCommand())
                            {
                                cmd2.Connection = con;
                                cmd2.CommandText = "insert into trash_get_user_bodyfile(alınan_mail_no, alınan_mail_bodyfile, width, height, kisi_no) values (@alınan_mail_no, @alınan_mail_bodyfile, @width, @height, @kisi_no)";
                                cmd2.Parameters.AddWithValue("@alınan_mail_no", mail_id);
                                cmd2.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                                cmd2.Parameters.AddWithValue("@alınan_mail_bodyfile", z.gonderilen_mail_bodyfile);
                                cmd2.Parameters.AddWithValue("@width", z.width);
                                cmd2.Parameters.AddWithValue("@height", z.height);
                                cmd2.CommandType = CommandType.Text;
                                cmd2.ExecuteNonQuery();
                                cmd2.Parameters.Clear();
                            }
                        }
                    }
                }
                con.Close();
            }
        }


        public void form3_secilen_degeri_sil_3(int donen_id)
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "Delete from trash_get_user where id=@id and kisi_no=@kisi_no";
                cmd.Parameters.AddWithValue("@id", donen_id);
                cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                using (SqlCommand cmd1 = new SqlCommand())
                {
                    cmd1.Connection = con;
                    cmd1.CommandText = "Delete from trash_get_user_dosyalar where alınan_mail_no=@alınan_mail_no and kisi_no=@kisi_no";
                    cmd1.Parameters.AddWithValue("@alınan_mail_no", donen_id);
                    cmd1.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                    cmd1.CommandType = CommandType.Text;
                    cmd1.ExecuteNonQuery();
                    using (SqlCommand cmd2 = new SqlCommand())
                    {
                        cmd2.Connection = con;
                        cmd2.CommandText = "Delete from trash_get_user_bodyfile where alınan_mail_no=@alınan_mail_no and kisi_no=@kisi_no";
                        cmd2.Parameters.AddWithValue("@alınan_mail_no", donen_id);
                        cmd2.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                        cmd2.CommandType = CommandType.Text;
                        cmd2.ExecuteNonQuery();
                    }
                }
                con.Close();
            }
        }











        List<mail_get_user> dondur1 = new List<mail_get_user>();
        List<mail_get_user_dosyalar> dondur1_attachment = new List<mail_get_user_dosyalar>(); //
        List<mail_get_user_bodyfile> dondur1_bodyfile = new List<mail_get_user_bodyfile>();


        List<mail_send_user> dondur2 = new List<mail_send_user>();
        List<mail_send_user_dosyalar> dondur2_attachment = new List<mail_send_user_dosyalar>();
        List<mail_send_user_bodyfile> dondur2_bodyfile = new List<mail_send_user_bodyfile>();

        List<trash_get_user> dondur3 = new List<trash_get_user>();
        List<trash_get_user_dosyalar> dondur3_attachment = new List<trash_get_user_dosyalar>();
        List<trash_get_user_bodyfile> dondur3_bodyfile = new List<trash_get_user_bodyfile>();

        public List<mail_get_user> form3_db_cekme_islemler_gelen_mail()
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT alınan_mail_konu,mail_alma_tarhi,yollayan_kisi,id,alınan_mail_icerik from mail_get_user where kisi_no=@kisi_no order by mail_alma_tarhi DESC";
                cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dondur1.Add(new mail_get_user
                    {
                        id = (Int32)dr["id"],
                        kisi_no = login_user.Instance.id,
                        yollayan_kisi = dr["yollayan_kisi"].ToString(),
                        mail_alma_tarhi = (DateTimeOffset)dr["mail_alma_tarhi"],
                        alınan_mail_konu = dr["alınan_mail_konu"].ToString(),
                        alınan_mail_icerik = dr["alınan_mail_icerik"].ToString(),
                    });
                }
                dr.Close();
                con.Close();
            }
            return dondur1;
        }
        public List<mail_get_user_dosyalar> form3_db_cekme_islemler_gelen_mail_attachment()
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT id,alınan_mail_no,alınan_mail_dosyalar,attachment_name from mail_get_user_dosyalar where kisi_no=@kisi_no order by alınan_mail_no DESC";
                cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dondur1_attachment.Add(new mail_get_user_dosyalar
                    {
                        id = (Int32)dr["id"],
                        kisi_no = login_user.Instance.id,
                        alınan_mail_no = (Int32)dr["alınan_mail_no"],
                        alınan_mail_dosyalar = (byte[])dr["alınan_mail_dosyalar"],
                        attachment_name = dr["attachment_name"].ToString()
                    });
                }
                dr.Close();
                con.Close();
            }
            return dondur1_attachment;
        }
        public List<mail_get_user_bodyfile> form3_db_cekme_islemler_gelen_mail_bodyfile()
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT id,alınan_mail_no,alınan_mail_bodyfile,width,height from mail_get_user_bodyfile where kisi_no=@kisi_no order by alınan_mail_no DESC";
                cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dondur1_bodyfile.Add(new mail_get_user_bodyfile
                    {
                        id = (Int32)dr["id"],
                        kisi_no = login_user.Instance.id,
                        alınan_mail_no = (Int32)dr["alınan_mail_no"],
                        alınan_mail_bodyfile = (byte[])dr["alınan_mail_bodyfile"],
                        width = (Int32)dr["width"],
                        height = (Int32)dr["height"]
                    });
                }
                dr.Close();
                con.Close();
            }
            return dondur1_bodyfile;
        } 


        public List<mail_send_user> form3_db_cekme_islemler_giden_mail()
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT gonderilen_mail_konu,mail_yollama_tarhi,id,gonderilen_mail_icerik from mail_send_user where kisi_no=@kisi_no order by mail_yollama_tarhi DESC";
                cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dondur2.Add(new mail_send_user
                    {
                        id = (Int32)dr["id"],
                        kisi_no = login_user.Instance.id,
                        mail_yollama_tarhi = (DateTimeOffset)dr["mail_yollama_tarhi"],
                        gonderilen_mail_konu = dr["gonderilen_mail_konu"].ToString(),
                        gonderilen_mail_icerik = dr["gonderilen_mail_icerik"].ToString(),
                    });
                }
                dr.Close();
                con.Close();
            }
            return dondur2;
        }
        public List<mail_send_user_dosyalar> form3_db_cekme_islemler_giden_mail_attachment()
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT id,gonderilen_mail_no,gonderilen_mail_dosyalar,attachment_name from mail_send_user_dosyalar where kisi_no=@kisi_no order by gonderilen_mail_no DESC";
                cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dondur2_attachment.Add(new mail_send_user_dosyalar
                    {
                        id = (Int32)dr["id"],
                        kisi_no = login_user.Instance.id,
                        gonderilen_mail_no = (Int32)dr["gonderilen_mail_no"],
                        gonderilen_mail_dosyalar = (byte[])dr["gonderilen_mail_dosyalar"],
                        attachment_name = dr["attachment_name"].ToString()
                    });
                }
                dr.Close();
                con.Close();
            }
            return dondur2_attachment;
        }
        public List<mail_send_user_bodyfile> form3_db_cekme_islemler_giden_mail_bodyfile()
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT id,gonderilen_mail_no,gonderilen_mail_bodyfile,width,height from mail_send_user_bodyfile where kisi_no=@kisi_no order by gonderilen_mail_no DESC";
                cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dondur2_bodyfile.Add(new mail_send_user_bodyfile
                    {
                        id = (Int32)dr["id"],
                        kisi_no = login_user.Instance.id,
                        gonderilen_mail_no = (Int32)dr["gonderilen_mail_no"],
                        gonderilen_mail_bodyfile = (byte[])dr["gonderilen_mail_bodyfile"],
                        width = (Int32)dr["width"],
                        height = (Int32)dr["height"]
                    });
                }
                dr.Close();
                con.Close();
            }
            return dondur2_bodyfile;
        }




        public List<trash_get_user> form3_db_cekme_islemler_trash_mail()
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT alınan_mail_konu,mail_alma_tarhi,yollayan_kisi,id,alınan_mail_icerik from trash_get_user where kisi_no=@kisi_no order by mail_alma_tarhi DESC";
                cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dondur3.Add(new trash_get_user
                    {
                        id = (Int32)dr["id"],
                        kisi_no = login_user.Instance.id,
                        yollayan_kisi = dr["yollayan_kisi"].ToString(),
                        mail_alma_tarhi = (DateTimeOffset)dr["mail_alma_tarhi"],
                        alınan_mail_konu = dr["alınan_mail_konu"].ToString(),
                        alınan_mail_icerik = dr["alınan_mail_icerik"].ToString(),
                    });
                }
                dr.Close();
                con.Close();
            }
            return dondur3;
        }
        public List<trash_get_user_dosyalar> form3_db_cekme_islemler_trash_mail_attachment()
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT id,alınan_mail_no,alınan_mail_dosyalar,attachment_name from trash_get_user_dosyalar where kisi_no=@kisi_no order by alınan_mail_no DESC";
                cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dondur3_attachment.Add(new trash_get_user_dosyalar
                    {
                        id = (Int32)dr["id"],
                        kisi_no = login_user.Instance.id,
                        alınan_mail_no = (Int32)dr["alınan_mail_no"],
                        alınan_mail_dosyalar = (byte[])dr["alınan_mail_dosyalar"],
                        attachment_name = dr["attachment_name"].ToString()
                    });
                }
                dr.Close();
                con.Close();
            }
            return dondur3_attachment;
        }
        public List<trash_get_user_bodyfile> form3_db_cekme_islemler_trash_mail_bodyfile()
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "SELECT id,alınan_mail_no,alınan_mail_bodyfile,width,height from trash_get_user_bodyfile where kisi_no=@kisi_no order by alınan_mail_no DESC";
                cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id); ;
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dondur3_bodyfile.Add(new trash_get_user_bodyfile
                    {
                        id = (Int32)dr["id"],
                        kisi_no = login_user.Instance.id,
                        alınan_mail_no = (Int32)dr["alınan_mail_no"],
                        alınan_mail_bodyfile = (byte[])dr["alınan_mail_bodyfile"],
                        width = (Int32)dr["width"],
                        height = (Int32)dr["height"]
                    });
                }
                dr.Close();
                con.Close();
            }
            return dondur3_bodyfile;
        }



        public void form3_secilen_degeri_geri_yukle_1(int donen_id, List<trash_get_user> mail_get, List<trash_get_user_dosyalar> mail_get_attach, List<trash_get_user_bodyfile> mail_get_body)
        {
            int mail_id;
            using (SqlCommand cmd = new SqlCommand())
            {
                con.Open();
                trash_get_user x = mail_get.Find(result => result.id == donen_id);

                if (x.id == donen_id)
                {
                    cmd.Connection = con;
                    cmd.CommandText = "insert into mail_get_user(kisi_no, yollayan_kisi, mail_alma_tarhi, alınan_mail_konu, alınan_mail_icerik) values (@kisi_no, @yollayan_kisi, @mail_alma_tarhi, @alınan_mail_konu, @alınan_mail_icerik)";
                    cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                    cmd.Parameters.AddWithValue("@yollayan_kisi", x.yollayan_kisi);
                    cmd.Parameters.AddWithValue("@mail_alma_tarhi", x.mail_alma_tarhi);
                    cmd.Parameters.AddWithValue("@alınan_mail_konu", x.alınan_mail_konu);
                    cmd.Parameters.AddWithValue("@alınan_mail_icerik", x.alınan_mail_icerik);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    using (SqlCommand cmd0 = new SqlCommand())
                    {
                        cmd0.Connection = con;
                        cmd0.CommandText = "SELECT id FROM mail_get_user WHERE kisi_no=@kisi_no and yollayan_kisi=@yollayan_kisi and mail_alma_tarhi=@mail_alma_tarhi and alınan_mail_konu=@alınan_mail_konu";
                        cmd0.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                        cmd0.Parameters.AddWithValue("@yollayan_kisi", x.yollayan_kisi);
                        cmd0.Parameters.AddWithValue("@mail_alma_tarhi", x.mail_alma_tarhi);
                        cmd0.Parameters.AddWithValue("@alınan_mail_konu", x.alınan_mail_konu);
                        cmd0.CommandType = CommandType.Text;
                        mail_id = Convert.ToInt32(cmd0.ExecuteScalar());
                    }
                    foreach (var y in mail_get_attach)
                    {
                        if (x.id == y.alınan_mail_no)
                        {
                            using (SqlCommand cmd1 = new SqlCommand())
                            {
                                cmd1.Connection = con;
                                cmd1.CommandText = "insert into mail_get_user_dosyalar(alınan_mail_no, kisi_no, alınan_mail_dosyalar) values (@alınan_mail_no, @kisi_no, @alınan_mail_dosyalar)";
                                cmd1.Parameters.AddWithValue("@alınan_mail_no", mail_id);
                                cmd1.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                                cmd1.Parameters.AddWithValue("@alınan_mail_dosyalar", y.alınan_mail_dosyalar);
                                cmd1.CommandType = CommandType.Text;
                                cmd1.ExecuteNonQuery();
                                cmd1.Parameters.Clear();
                            }
                        }
                    }
                    foreach (var z in mail_get_body)
                    {
                        if (x.id == z.alınan_mail_no)
                        {
                            using (SqlCommand cmd2 = new SqlCommand())
                            {
                                cmd2.Connection = con;
                                cmd2.CommandText = "insert into mail_get_user_bodyfile(alınan_mail_no, alınan_mail_bodyfile, width, height, kisi_no) values (@alınan_mail_no, @alınan_mail_bodyfile, @width, @height, @kisi_no)";
                                cmd2.Parameters.AddWithValue("@alınan_mail_no", mail_id);
                                cmd2.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                                cmd2.Parameters.AddWithValue("@alınan_mail_bodyfile", z.alınan_mail_bodyfile);
                                cmd2.Parameters.AddWithValue("@width", z.width);
                                cmd2.Parameters.AddWithValue("@height", z.height);
                                cmd2.CommandType = CommandType.Text;
                                cmd2.ExecuteNonQuery();
                                cmd2.Parameters.Clear();
                            }
                        }
                    }
                }
                con.Close();
            }
        }


        public void form3_secilen_degeri_geri_yukle_2(int donen_id, List<trash_get_user> mail_get, List<trash_get_user_dosyalar> mail_get_attach, List<trash_get_user_bodyfile> mail_get_body)
        {
            int mail_id;
            using (SqlCommand cmd = new SqlCommand())
            {
                con.Open();
                trash_get_user x = mail_get.Find(result => result.id == donen_id);
                if (x.id == donen_id)
                {
                    cmd.Connection = con;
                    cmd.CommandText = "insert into mail_send_user(kisi_no, mail_yollama_tarhi, gonderilen_mail_konu, gonderilen_mail_icerik) values (@kisi_no, @mail_yollama_tarhi, @gonderilen_mail_konu, @gonderilen_mail_icerik)";
                    cmd.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                    cmd.Parameters.AddWithValue("@mail_yollama_tarhi", x.mail_alma_tarhi);
                    cmd.Parameters.AddWithValue("@gonderilen_mail_konu", x.alınan_mail_konu);
                    cmd.Parameters.AddWithValue("@gonderilen_mail_icerik", x.alınan_mail_icerik);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    using (SqlCommand cmd0 = new SqlCommand())
                    {
                        cmd0.Connection = con;
                        cmd0.CommandText = "SELECT id FROM mail_send_user WHERE kisi_no=@kisi_no and mail_yollama_tarhi=@mail_yollama_tarhi and gonderilen_mail_konu=@gonderilen_mail_konu";
                        cmd0.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                        cmd0.Parameters.AddWithValue("@mail_yollama_tarhi", x.mail_alma_tarhi);
                        cmd0.Parameters.AddWithValue("@gonderilen_mail_konu", x.alınan_mail_konu);
                        cmd0.CommandType = CommandType.Text;
                        mail_id = Convert.ToInt32(cmd0.ExecuteScalar());
                    }


                    foreach (var y in mail_get_attach)
                    {
                        if (x.id == y.alınan_mail_no)
                        {
                            using (SqlCommand cmd1 = new SqlCommand())
                            {
                                cmd1.Connection = con;
                                cmd1.CommandText = "insert into mail_send_user_dosyalar(gonderilen_mail_no, kisi_no, gonderilen_mail_dosyalar) values (@gonderilen_mail_no, @kisi_no, @gonderilen_mail_dosyalar)";
                                cmd1.Parameters.AddWithValue("@gonderilen_mail_no", mail_id);
                                cmd1.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                                cmd1.Parameters.AddWithValue("@gonderilen_mail_dosyalar", y.alınan_mail_dosyalar);
                                cmd1.CommandType = CommandType.Text;
                                cmd1.ExecuteNonQuery();
                                cmd1.Parameters.Clear();
                            }
                        }
                    }
                    foreach (var z in mail_get_body)
                    {
                        if (x.id == z.alınan_mail_no)
                        {
                            using (SqlCommand cmd2 = new SqlCommand())
                            {
                                cmd2.Connection = con;
                                cmd2.CommandText = "insert into mail_send_user_bodyfile(gonderilen_mail_no, kisi_no, gonderilen_mail_bodyfile, width, height) values (@gonderilen_mail_no, @kisi_no, @gonderilen_mail_bodyfile, @width, @height)";
                                cmd2.Parameters.AddWithValue("@gonderilen_mail_no", mail_id);
                                cmd2.Parameters.AddWithValue("@kisi_no", login_user.Instance.id);
                                cmd2.Parameters.AddWithValue("@gonderilen_mail_bodyfile", z.alınan_mail_bodyfile);
                                cmd2.Parameters.AddWithValue("@width", z.width);
                                cmd2.Parameters.AddWithValue("@height", z.height);
                                cmd2.CommandType = CommandType.Text;
                                cmd2.ExecuteNonQuery();
                                cmd2.Parameters.Clear();
                            }
                        }
                    }
                }
            }
            con.Close();

        }
    }
}