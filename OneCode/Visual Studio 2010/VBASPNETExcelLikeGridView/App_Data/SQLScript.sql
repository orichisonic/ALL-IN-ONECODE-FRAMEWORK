--Create a database called "db_Persons"

use master
if exists (select [name] from sysdatabases where [name]='db_Persons')
drop database db_Persons
create database db_Persons
go

--Open the database and create a table called "tb_personInfo"

use db_Persons
go
create table tb_personInfo
(
	Id int primary key identity(1,1),
	PersonName varchar(20) not null,
	PersonAddress varchar(50)
)
go
--Data for tesing create 10 records..

declare @counter int
set @counter = 1

while(@counter<=10)
  begin
  insert into tb_personInfo(PersonName,PersonAddress) values ('Person'+convert(varchar(3),@counter),'Address'+convert(varchar(3),@counter))
  set @counter = @counter+1
end 