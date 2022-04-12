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
create Table Cart

(
CartId INT IDENTITY(1,1) PRIMARY KEY,
Quantity INT
);

alter table cart add UserId int  references Users (UserId)
alter table cart add BookId int  references Books (BookId)

select * from Cart;
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
Insert Into Cart(Quantity,UserId, BookId)
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
update Cart 
set BookId = @BookId,
Quantity = @Quantity 
where CartId = @CartId;
End;

---Create procedure to deletecart
alter procedure DeleteCart
(
@CartId int
)
as
BEGIN
Delete Cart 
where CartId = @CartId 
End;

--create procedure to getcartbyuserid
create Proc GetCartbyUserId
(
@UserId int
)
as
BEGIN
Select CartId, Quantity, UserId,c.BookId,
bookName, authorName, discountPrice, originalPrice, bookImage
from Cart c
join Books b on
c.BookId = b.bookId
where UserId = @UserId;
End;

select * from Users
EXEC GetCartbyUserId 3;

Alter PROC GetCartbyUserId
(
@userId INT
)
AS
BEGIN
SELECT 
c.cartId,
b.BookName,
b.AuthorName,
b.discountPrice,
b.DiscountPrice,
b.OriginalPrice,
b.BookImage
FROM [Cart] AS c
LEFT JOIN [Books] AS b ON c.BookId = b.BookId
WHERE c.UserId = @userId 
END

--create address type table
create Table AddressTypeT
(
	TypeId INT IDENTITY(1,1) PRIMARY KEY,
	TypeName varchar(255)
);

select * from AddressTypeT

---insert record for addresstype table
insert into AddressTypeT values('Home'),('Office'),('Other');

---create address table
create Table AddressTable
(
AddressId INT IDENTITY(1,1) PRIMARY KEY,
FullAddress varchar(255),
City varchar(100),
State varchar(100),
TypeId int 
FOREIGN KEY (TypeId) REFERENCES AddressTypeT(TypeId),
UserId INT FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
select * from AddressTable

--create procedure to AddAddress
-- Procedure To Add Address
alter procedure AddAddress
(
@FullAddress varchar(max),
@City varchar(100),
@State varchar(100),
@TypeId int,
@UserId int
)
as
BEGIN
If Exists (select * from AddressTypeT where TypeId = @TypeId)
begin
Insert into AddressTable 
values(@FullAddress, @City, @State, @TypeId, @UserId);
end
Else
begin
select 2
end
End;
select * from Users
select * from Books
select * from Cart

sp_help users

SET IDENTITY_INSERT Addresses ON 
delete from Addresses where AddressId = 0

--create procedure for updateAddress
alter proc UpdateAddress
(
@AddressId int,
@FullAddress varchar(max),
@City varchar(100),
@State varchar(100),
@TypeId int
--@UserId int
)
as
BEGIN
If Exists (select * from AddressTypeT where TypeId = @TypeId)
begin
Update AddressTable set
FullAddress = @FullAddress, City = @City,
State = @State , TypeId = @TypeId
where AddressId = @AddressId
end
Else
begin
select 2
end
End;

--create procedure to delete address
create Procedure DeleteAddress
(
@AddressId int
)
as
BEGIN
Delete AddressTable where AddressId = @AddressId 
End;

-- Procedure To Get All Address
create Procedure GetAllAddress
(
@UserId int
)
as
BEGIN
Select FullAddress, City, State,a1.UserId, a2.TypeId
from AddressTable a1
Inner join AddressTypeT a2 on a2.TypeId = a1.TypeId 
where UserId = @UserId;
END;

---create wishlist table
create Table Wishlist
(
WishlistId int identity(1,1) primary key,
UserId INT FOREIGN KEY REFERENCES Users(UserId),
bookId INT FOREIGN KEY REFERENCES Books(bookId)
);

select * from Wishlist
---create procedure to Add in Wishlist
create procedure AddInWishlist
(
	@UserId int,
	@BookId int
)
as
BEGIN
If Exists (Select * from Wishlist where UserId = @UserId and bookId = @BookId)
begin 
select 2;
end
Else
begin 
if Exists (select * from Books where bookId = @BookId)
begin
Insert into Wishlist(UserId, bookId) values (@UserId , @BookId);
end
else
begin
select 1;
end
end
End;

--create procedure to delete from wishlist
create procedure DeleteFromWishlist
(
@WishlistId int,
@UserId int
)
as
BEGIN 
Delete Wishlist where WishlistId = @WishlistId and UserId = @UserId;
End;	 

---create procedure to get all records from wishlist
Alter procedure GetAllRecordFromWishlist
(
@UserId int
)
as
BEGIN
select w.WishlistId,w.UserId,w.bookId,
b.bookName,b.authorName,b.discountPrice,b.originalPrice,
b.bookImage
from Wishlist w
right join Books b
on w.bookId = b.bookId
where UserId = @UserId;
END;

---create order table
create table OrderTable
(
OrderId int primary key identity(1,1),
TotalPrice int,
BookQuantity int,
OrderDate Date,
BookCount int,
UserId int FOREIGN KEY (UserId) REFERENCES Users(UserId),
bookId int FOREIGN KEY (bookId) REFERENCES Books(bookId),
AddressId int FOREIGN KEY (AddressId) REFERENCES AddressTable(AddressId)
);

select * from OrderTable
--create procedure for order API
--create procedure for AddOrder API
create procedure AddOrder
(
@BookQuantity int,
@UserId int,
@BookId int,
@AddressId int,
@BookCount int
)
as
Declare @TotalPrice int
BEGIN
set @TotalPrice = (select discountPrice from Books where bookId = @BookId);
If(Exists(Select * from Books where bookId = @BookId))
BEGIN
If(Exists (Select * from Users where UserId = @UserId))
BEGIN
Begin try
Begin Transaction
Insert Into OrderTable (TotalPrice, BookQuantity, OrderDate, UserId, bookId, AddressId, BookCount)
Values(@TotalPrice*@BookQuantity, @BookQuantity, GETDATE(), @UserId, @BookId, @AddressId,@BookCount);
Update OrderTable set BookCount=BookCount
where bookId = @BookId;
Delete from Cart where BookId = @BookId and UserId = @UserId;
select * from OrderTable;
commit Transaction
End try
Begin Catch
rollback;
End Catch
END
Else
Begin
Select 3;
End
End
Else
Begin
Select 2;
End
END;


--create procedure for get all orders
alter procedure GetAllOrders
(
@UserId int
)
as
begin
Select 
O.OrderId, O.UserId, O.AddressId, b.bookId,
O.TotalPrice, O.BookQuantity, O.OrderDate,
b.bookName, b.AuthorName, b.bookImage
FROM Books b
left join OrderTable O on O.bookId = b.bookId 
where 
O.UserId = @UserId;
END

select * from OrderTable
select * from Books
Select * from Cart
Select * from AddressTable
Select * from Wishlist


--create feedback table 
create Table FeedbackTable
(
FeedbackId INT PRIMARY KEY IDENTITY(1,1),
Comment varchar(255),
Rating int,
Totalrating int,
bookId int FOREIGN KEY (bookId) REFERENCES Books(bookId),
UserId int FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
select * from FeedbackTable
alter Procedure AddFeedback
(
@Comment varchar(255),
@Rating int,
@Totalrating int,
@BookId int,
@UserId int
)
as
Declare @AverageRating int;
BEGIN
IF (EXISTS(SELECT * FROM FeedbackTable WHERE bookId = @BookId and UserId=@UserId))
select 1;
Else
Begin
IF (EXISTS(SELECT * FROM Books WHERE bookId = @BookId))
Begin  select * from FeedbackTable
Begin try
Begin transaction
Insert into FeedbackTable(Comment, Rating, bookId, UserId) values (@Comment, @Rating, @BookId, @UserId);		
set @AverageRating = (Select AVG(Rating) from FeedbackTable where bookId = @BookId);
Update FeedbackTable set rating = @AverageRating, Totalrating = @Totalrating + 1 
where  bookId = @BookId;
Commit Transaction
End Try
Begin catch
Rollback transaction
End catch
End
Else
Begin
Select 2; 
End
End
END;

---create procedure to update feedback
Alter procedure UpdateFeedback
(
@Comment varchar(255),
@Rating int,
@BookId int,
--@UserId int,
@FeedbackId int
)
as
Declare @AverageRating int;
BEGIN
IF (EXISTS(SELECT * FROM FeedbackTable WHERE FeedbackId = @FeedbackId))
Begin
Begin try
begin transaction
Update FeedbackTable set Comment = @Comment, Rating = @Rating, bookId = @BookId
where FeedbackId = @FeedbackId;	
set @AverageRating = (select AVG(Rating) from FeedbackTable 
where bookId = @BookId);
Update FeedbackTable set rating = @AverageRating,  totalRating = totalRating+1 
where bookId = @BookId;
Commit Transaction
End Try
Begin catch
Rollback transaction
End catch
End
Else
Begin
Select 2; 
End
END;

-- create Procedure to Delete Feedback 
Alter procedure DeleteFeedback
(
@FeedbackId int,
@UserId int
)
as
BEGIN
Delete FeedbackTable
where FeedbackId = @FeedbackId and UserId = @UserId;
END;

--create procedure to get all feedback
alter Proc GetAllFeedback
(
@BookId int
)
as
BEGIN
Select FeedbackId, Comment, Rating, bookId, u.FullName
From Users u
left Join FeedbackTable f
on f.UserId = u.UserId
where
	BookId = @BookId;
END;

---create Admin Table
create Table AdminTable
(
AdminId int primary key identity(1,1),
FullName varchar(255),
Email varchar(255),
Password varchar(255),
PhoneNumber Bigint
);

--insert value Admin table
Insert into AdminTable values('Admin', 'usagu2020@gmail.com', 'Sagu@123', '9112241587');
select * from AdminTable

---alter book table
Alter table Books Add AdminId int Foreign key(AdminId) References AdminTable(AdminId);

---create procedure for admin login
create procedure LoginAdmin
(
@Email varchar(255),
@Password varchar(255)
)
as
BEGIN
If(Exists(select * from AdminTable where Email = @Email and Password = @Password))
Begin
select AdminId, FullName, Email, PhoneNumber from AdminTable;
end
Else
Begin
select 2;
End
END;