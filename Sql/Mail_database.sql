use Mail

create table login_user(
id int PRIMARY KEY IDENTITY(1,1),
Eposta varchar(50) UNIQUE NOT NULL,
sifre varchar(16) NOT NULL,
IPV4 varchar(40) NOT NULL,
pc_user_name varchar(40) NOT NULL,
tarih datetime NOT NULL DEFAULT GETDATE(),
)
/* çöp kutusu gönderilenler kutusu iþaretliler kutusu vb için kullnýlacak...

create table mail_send_user(
id int PRIMARY KEY IDENTITY(1,1),
yollayan_kisi_no int NOT NULL,
yolladýgýmýz_kisi varchar(100) NOT NULL,
mail_yollama_tarhi datetimeoffset NOT NULL default GETDATE(),

gonderilen_mail_konu varchar(333) NOT NULL,
gonderilen_mail_icerik varchar(999) NOT NULL,
)
create table mail_send_user_dosyalar(
id int PRIMARY KEY IDENTITY(1,1),
gonderilen_mail_no int NOT NULL,
gonderilen_mail_dosyalar varbinary(max) NULL,
)

*/

create table mail_get_user(
id int PRIMARY KEY IDENTITY(1,1),
alan_kisi_no int NOT NULL,
yollayan_kisi varchar(100) NOT NULL,
mail_alma_tarhi datetimeoffset NOT NULL default GETDATE(),

alýnan_mail_konu varchar(1000) NOT NULL,
alýnan_mail_icerik nvarchar(max) NOT NULL,
)
create table mail_get_user_dosyalar(
id int PRIMARY KEY IDENTITY(1,1),
alýnan_mail_no int NOT NULL,
alýnan_mail_dosyalar varbinary(max) NULL,
)


select * from login_user
select * from mail_send_user
select * from mail_get_user 

select * from mail_send_user_dosyalar
select * from mail_get_user_dosyalar
