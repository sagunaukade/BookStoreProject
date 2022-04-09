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

select * from Users;

----stored procedures for User Api
---Create procedured for User Registration
Create procedure UserRegister(
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
create procedure UserForgotPassword
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
create procedure UserResetPassword
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
create table Books(
BookId int identity (1,1)primary key,
BookName varchar(255),
AuthorName varchar(255),
Rating int,
TotalView int,
OriginalPrice decimal,
DiscountPrice decimal,
BookDetails varchar(255),
BookImage varchar(255),
);

drop table Books
select * from  Books
----stored procedures for Book Api
---procedured to add book
alter procedure AddBook
(
@BookName varchar(255),
@authorName varchar(255),
@rating int,
@totalView int,
@originalPrice Decimal,
@discountPrice Decimal,
@BookDetails varchar(255),
@bookImage varchar(255)
)
as
BEGIN
Insert into Books(BookName, authorName, rating, totalview, originalPrice, 
discountPrice, BookDetails, bookImage)
values (@bookName, @authorName, @rating, @totalView ,@originalPrice, @discountPrice,
@BookDetails, @bookImage);
End;


--procedure to updatebook
create procedure UpdateBook
(
@bookId int,
@bookName varchar(255),
@authorName varchar(255),
@originalPrice Decimal,
@discountPrice Decimal,
@BookDetails varchar(255),
@bookImage varchar(255)
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
	CartId INT IDENTITY(1,1) PRIMARY KEY,
	Quantity INT,
	UserId INT FOREIGN KEY REFERENCES Users(UserId),
	BookId INT FOREIGN KEY REFERENCES Books(bookId)
);
select * from Carts;
drop table Carts
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
Alter procedure UpdateCart
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

---Create procedure to deletecart
alter procedure DeleteCart
(
@CartId int
--@UserId int
)
as
BEGIN
Delete Carts 
where CartId = @CartId 
--and UserId = @UserId;; 
End;

--create procedure to getcartbyuserid
alter procedure GetCartbyUserId
(
@UserId int
)
as
BEGIN
Select c.CartId, c.Quantity, c.UserId,c.BookId,
b.bookName, b.authorName, b.discountPrice, b.originalPrice, b.bookImage
from Carts c
inner join Books b on c.BookId = b.bookId
where UserId = @UserId;
End;

EXEC GetCartbyUserId 3;

--create address type table
create Table AddressTypeTab
(
	TypeId INT IDENTITY(1,1) PRIMARY KEY,
	AddressType varchar(255)
);
drop table AddressType
select * from AddressTypeTab
---insert record for addresstype table
--insert into AddressType values('Home'),('Office'),('Other');
insert into AddressTypeTab values ('Home')
insert into AddressTypeTab values ('Office')
insert into AddressTypeTab values ('Other')


---create address table
create table AddressTab(
AddressId int identity(1,1) primary key,
FullAddress varchar(255),
AddressType int,
City varchar(255),
State varchar(255),
TypeId int foreign key (TypeId) References AddressTypeTab(TypeId),
UserId INT FOREIGN KEY (UserId) REFERENCES users(UserId),
);

select * from AddressTab

drop table AddressTab

---alter table AddressTab add constraint AddressTab_AddressType_FK foreign key (AddressType) References AddressTypeTab (AddressId)


--create procedure to AddAddress
-- Procedure To Add Address
alter procedure AddAddress
(
	@FullAddress varchar(255),
	@City varchar(255),
	@State varchar(255),
	@TypeId int,
	@UserId int
)
as
BEGIN
If Exists (select * from AddressType where TypeId = @TypeId)
begin
Insert into Addresses values(@FullAddress, @City, @State, @TypeId, @UserId);
end
Else
begin
select 2
end
End;

select * from Users
select * from Books
select * from Carts
select * from Addresses

sp_help users

insert into Addresses(AddressId, FullAddress, City, State, TypeId, UserId) values(1,'Shivane','Pune','MH',1,2);

SET IDENTITY_INSERT Addresses ON 
delete from Addresses where AddressId = 0

select * from Users
select * from Books
select * from Carts


