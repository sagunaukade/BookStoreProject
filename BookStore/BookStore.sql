----Create database bookstore
Create database BookStore;

---Create user table
Create Table Users(
UserId int Identity(1,1) primary key,
FullName Varchar(255) not null,
Email varchar(255) not null,
Password varchar(255) not null,
MobileNumber varchar(30) 
);

select * from Users