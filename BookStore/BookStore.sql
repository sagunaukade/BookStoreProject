----Create database
create database BookStore;

---use bookstore database
use BookStore

---create Users table
create table Users(
UserId int primary key identity(1,1),
FullName varchar(255),
Email varchar(255),
Password varchar(255),
MobileNumber bigint)

drop table Users;
select * from Users;

----stored procedures for User Api
---Create procedured for User Registration
Alter procedure UserRegister(
@FullName varchar(255),
@Email varchar(255),
@Password varchar(255),
@MobileNumber bigint)
As
Begin
insert into Users(FullName,Email,Password,MobileNumber) values(@FullName,@Email,@Password,@MobileNumber);
end

---Create procedured for User Login
create procedure UserLogin
(
@Email varchar(255),
@Password varchar(255)
)
as
begin
select * from Users
where Email = @Email and Password = @Password
End;

---Create procedured for User FOrgot Password
Create procedure UserForgotPassword
(
@Email varchar(Max)
)
as
begin
Update Users 
set Password = 'Null'
where Email = @Email;
select * from Users where Email = @Email;
End;

---create procedure for user reset password 
CREATE procedure UserResetPassword
(
@Email varchar(Max),
@Password varchar(Max)
)
AS
BEGIN
UPDATE Users 
SET 
Password = @Password 
WHERE Email = @Email;
End;

---Create book table
create table books(
BookId int identity (1,1)primary key,
BookName varchar(255),
AuthorName varchar(255),
Rating int,
TotalView int,
OriginalPrice int,
DiscountPrice int,
BookDetails varchar(500),
BookImage varchar(500))

select * from books;

----stored procedures for Book Api
---procedured to add book
create procedure AddBook
(
@bookName varchar(Max),
@authorName varchar(250),
@rating int,
@totalView int,
@originalPrice Decimal,
@discountPrice Decimal,
@BookDetails varchar(Max),
@bookImage varchar(250)
)
as
BEGIN
Insert into Books (bookName, authorName, rating, totalview, originalPrice, 
discountPrice, BookDetails, bookImage)
values (@bookName, @authorName, @rating, @totalView ,@originalPrice, @discountPrice,
@BookDetails, @bookImage);
End;

--procedure to updatebook
create procedure UpdateBook
(
@bookId int,
@bookName varchar(Max),
@authorName varchar(250),
@originalPrice Decimal,
@discountPrice Decimal,
@BookDetails varchar(Max),
@bookImage varchar(250)
)
as
BEGIN
Update Books set bookName = @bookName, 
authorName = @authorName,
originalPrice= @originalPrice,
discountPrice = @discountPrice,
BookDetails = @BookDetails,
bookImage =@bookImage
where bookId = @bookId;
End;

---Procedure to deletebook
create procedure DeleteBook
(
@bookId int
)
as
BEGIN
Delete Books 
where bookId = @bookId;
End;

---create procedure to getbookbybookid
create procedure GetBookByBookId
(
@bookId int
)
as
BEGIN
select * from Books
where bookId = @bookId;
End;