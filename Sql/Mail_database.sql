use Mail
use deneme
create database Mail
drop database Mail


create table login_user(
id int PRIMARY KEY IDENTITY(1,1),
Eposta varchar(50) UNIQUE NOT NULL,
sifre varchar(16) NOT NULL,
IPV4 varchar(40) NOT NULL,
pc_user_name varchar(40) NOT NULL,
tarih datetime NOT NULL DEFAULT GETDATE(),
)


create table mail_get_user(		
id int PRIMARY KEY IDENTITY(1,1),
kisi_no int NOT NULL,
yollayan_kisi varchar(100) NOT NULL,
mail_alma_tarhi datetimeoffset NOT NULL default GETDATE(),
alýnan_mail_konu varchar(1000) NOT NULL,
alýnan_mail_icerik nvarchar(max) NOT NULL,
)
create table mail_get_user_dosyalar(		
id int PRIMARY KEY IDENTITY(1,1),
kisi_no int NOT NULL,
alýnan_mail_no int NOT NULL,
alýnan_mail_dosyalar varbinary(max) NULL,
)
create table mail_get_user_bodyfile(		
id int PRIMARY KEY IDENTITY(1,1),
kisi_no int NOT NULL,
alýnan_mail_no int NOT NULL,
alýnan_mail_bodyfile varbinary(max) NULL,
width int NULL,
height int NULL,
)
create table mail_send_user(	
id int PRIMARY KEY IDENTITY(1,1),
kisi_no int NOT NULL,
mail_yollama_tarhi datetimeoffset NOT NULL default GETDATE(),
gonderilen_mail_konu varchar(1000) NOT NULL,
gonderilen_mail_icerik nvarchar(max) NOT NULL,
)
create table mail_send_user_dosyalar(
id int PRIMARY KEY IDENTITY(1,1),
kisi_no int NOT NULL,
gonderilen_mail_no int NOT NULL,
gonderilen_mail_dosyalar varbinary(max) NULL,
)
create table mail_send_user_bodyfile(
id int PRIMARY KEY IDENTITY(1,1),
kisi_no int NOT NULL,
gonderilen_mail_no int NOT NULL,
gonderilen_mail_bodyfile varbinary(max) NULL,
width int NULL,
height int NULL,
)





select * from login_user
select * from mail_get_user 
select * from mail_get_user_dosyalar 
select * from mail_get_user_bodyfile 

select * from mail_send_user
select * from mail_send_user_dosyalar 
select * from mail_send_user_bodyfile 

delete mail_send_user
delete mail_send_user_dosyalar
delete mail_send_user_bodyfile


select * from mail_send_user
select * from mail_send_user_dosyalar
select * from mail_get_user_dosyalar
 