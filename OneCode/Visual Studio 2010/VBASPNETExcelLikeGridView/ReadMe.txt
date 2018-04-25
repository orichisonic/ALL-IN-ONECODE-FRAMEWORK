========================================================================
         ASP.NET APPLICATION : VBASPNETExcelLikeGridView Project Overview
========================================================================

/////////////////////////////////////////////////////////////////////////////
Use and Introduction:

  The project illustrates how to do a batch insert,delete and update instead
  of inserting,delting,updating row by row.

/////////////////////////////////////////////////////////////////////////////

Demo:

 1) Copy what's in the SQLScript.sql in the App_Folder
  into Sql Expression or Sql standard Sever IDE, than run it to create the 
  database for tesing.
 2) Open the project and right click the "Default.aspx", choose "View In Browser";
 3) When you check a checkbox inside a GridView to mark the row to be deleted,
  the Cell will change to red.
 4) When you add a new row by clicking the Add button, the new row is green
  by default.
 5) When you change a value from a cell inside the GridView, the background
  of the cell will turn blue.
 6) When you click the Save button, all the changes (including modified,
  deleted as well as added data) will be batch executed.
 7)Attention: Because the file will save some temporary data onto your PC,
  so please make sure that you have got the Administrator right to run it.
Code Logical:

Step1. Create a VB ASP.NET Web Application in Visual Studio 2010 RC /
Visual Web Developer 2010 and name it as VBASPNETExcelLikeGridView.

[ NOTE: You can download the free Web Developer from:
 http://www.microsoft.com/express/Web/ ]

Step2. Delete the following default folders and files created automatically 
by Visual Studio.

Account folder
Script folder
Style folder
About.aspx file
Default.aspx file
Global.asax file
Site.Master file

Step3. Add a new web form page to the website and name it as "Default.aspx",
Then Create a folder named "App_Data", then create a textfile inside,
copy the following codes into it and save, rename it as "SQLScript.sql".

[NOTE] You should write a sql script to create a database and a datatable
so as to do the experiment, the codes are:

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

Then open your SqlExpress or Sql Server management studio, log in and drag and drop 
the text file there, press F5 to create the database with the datatable. You can 
download the free SqlExpress from http://www.microsoft.com/express/Database/ .

Step4. Add a connection string in web.config file:
<connectionStrings>
    <add name="MyConn" 
	connectionString="server=.\SQLEXPRESS;DataBase=db_Persons;
	integrated security=true"/>
</connectionStrings>

Step5. Right click the mouse button to create a class named "DBProcess.cs". Create
a class like what you can see in the file to process with DB. This "DBProcess.cs"
will create a "table.dat" serialized file to keep the state of datatable so as to
memorized it (For more functions please see the detail comments).

Step6. Drag and drop a GridView, add some template fields, some checkboxes as well
as an "Add" button, a "Save" button and set all properties as what I've mentioned
in the aspx markups.

Step7. In order to implement non-refresh modification symbol (cell backcolors).
We should write some JQuery functions. You can find them in the same Default.aspx
HTML markups with detail comments.

/////////////////////////////////////////////////////////////////////////////
References:

ASP.NET QuickStart Torturial:

1）http://www.asp.net/data-access/tutorials/batch-deleting-cs

2）http://www.asp.net/data-access/tutorials/batch-updating-vb

3）http://www.asp.net/data-access/tutorials/batch-inserting-vb

/////////////////////////////////////////////////////////////////////////////