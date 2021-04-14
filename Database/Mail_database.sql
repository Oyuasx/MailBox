<<<<<<< HEAD
drop database deneme1
create database deneme1
=======
use Mail
>>>>>>> 0742c2b (Update.2)

drop table login_user
drop table mail_send_user
drop table mail_get_user
drop table mail_send_document
drop table mail_get_document

create table login_user(
id int PRIMARY KEY IDENTITY(1,1),
Eposta varchar(50) UNIQUE NOT NULL,
sifre varchar(16) NOT NULL,
giris_tarih datetime NOT NULL default GETDATE(),
IPV4 varchar(16) NOT NULL,
pc_user_name varchar(16) NOT NULL,
)
create table mail_send_user(
id int PRIMARY KEY IDENTITY(1,1),
yolladýgýmýz_kisi varchar(50) NOT NULL,
mail_yollama_tarhi datetime NOT NULL default GETDATE(),
)
create table mail_get_user(
id int PRIMARY KEY IDENTITY(1,1),
yollayan_kisi varchar(50) NOT NULL,
mail_alma_tarhi datetime NOT NULL default GETDATE(),
)
create table mail_send_document(
id int PRIMARY KEY IDENTITY(1,1),
gonderilen_mail_icerik varchar(999),
)
create table mail_get_document(
id int PRIMARY KEY IDENTITY(1,1),
gelen_mail_icerik varchar(999),
)

<<<<<<< HEAD
insert into login_user (eposta,sifre,IPV4,pc_user_name) values ('admin@hotmail.com','12345678','1.1.1.1.1.1','admin')
insert into mail_send_user (yolladýgýmýz_kisi) values ('bu_bir_Deneme')
insert into mail_get_user (yollayan_kisi) values ('bu_bir_Deneme')
insert into mail_send_document (gonderilen_mail_icerik) values ('bu büyük bir denemedir. herkesin bilgisine')
insert into mail_get_document (gelen_mail_icerik) values ('bu büyük bir denemedir. herkesin bilgisine')

select * from login_user
select * from mail_send_user
select * from mail_get_user
select * from mail_send_document
select * from mail_get_document
=======
>>>>>>> 0742c2b (Update.2)
