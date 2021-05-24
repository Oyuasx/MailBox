drop database Mail
create database Mail

use Mail
use deneme

drop table login_user;
drop table mail_send_user;
drop table mail_get_user;

create table login_user(
id int PRIMARY KEY IDENTITY(1,1),
Eposta varchar(50) UNIQUE NOT NULL,
sifre varchar(16) NOT NULL,
IPV4 varchar(40) NOT NULL,
pc_user_name varchar(40) NOT NULL,
tarih datetime NOT NULL DEFAULT GETDATE(),
)


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


insert into login_user (eposta,sifre,IPV4,pc_user_name,tarih) values ('admin@ho1tmail.com','12345678','1.1.1.1.1.1','admin',GETDATE())
insert into mail_send_user (yollayan_kisi_no,yolladýgýmýz_kisi,gonderilen_mail_konu,gonderilen_mail_icerik) values ('1','ahmet@gmail.com','Bu bir deneme konusu','Bu konu hakkýnda neler düþündüðünü merak ediyorum. Acaba bana geri dönüþ yapabilirmisin(mail üzerinden)')
insert into mail_get_user (alan_kisi_no,yollayan_kisi,mail_alma_tarhi,alýnan_mail_konu,alýnan_mail_icerik) values ('1','mehmet@gmail.com','12.12.2000','Bu bir deneme konusu','Bu konu hakkýnda neler düþündüðünü merak ediyorum. Acaba bana geri dönüþ yapabilirmisin(mail üzerinden)')


select * from login_user
select * from mail_send_user
select * from mail_get_user 

select * from mail_send_user_dosyalar
select * from mail_get_user_dosyalar

Delete from mail_get_user where alan_kisi_no='2'