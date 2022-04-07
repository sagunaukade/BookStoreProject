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

---Create cart table
create Table Carts
(
CartId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
Quantity INT default 1,
UserId INT FOREIGN KEY REFERENCES Users(UserId),
BookId INT FOREIGN KEY REFERENCES Books(BookId) 
);
drop table Carts
select * from Carts;

---create procedure to addcart
Alter Proc AddCart
(
@Quantity int,
@UserId int,
@BookId int

)
as
BEGIN
if(Exists (select * from Books where bookId = @BookId))
begin
Insert Into Carts (Quantity,UserId, BookId)
Values (@Quantity,@UserId, @BookId);
end
else
begin
select 1
end			 
End;

---create procedure to UpdateCart
create procedure UpdateCart
(
@Quantity int,
@BookId int,
@CartId int
)
as
BEGIN
update Carts 
set BookId = @BookId,
Quantity = @Quantity 
where CartId = @CartId;
End;