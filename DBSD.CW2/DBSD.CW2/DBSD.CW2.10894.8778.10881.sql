create database RetailUP

go
use RetailUP

go 

CREATE TABLE [ItemCategory](
	[ID] int identity(1,1) primary key,
	[CategoryName] varchar(75),
);

CREATE TABLE [ItemToSell](
	[ID] int identity(1,1) Primary key,
	[ItemCategoryID] int,
	[ItemName] nvarchar(200) not null,
	[ItemBrand] nvarchar(50),
	[ItemDescription] nvarchar(100),
	[ItemModel] nvarchar(50),
	[ItemAddedDate] DATE not null,
	[ItemImage] varchar(100),
	[IsActive] bit default 'True',
	[ItemRemained] int
	constraint fk_category_item foreign key (ItemCategoryID) references ItemCategory(ID) on delete no action on update no action
);



create table [Suppliers] (
	[SupplierId] int PRIMARY KEY IDENTITY (1, 1) NOT NULL,
	[Supplier_Name] text,
	[Supplier_Phone] int not null,
	[Supplier_Country] varchar(55),
	[Supplier_City] varchar(55),
	[District] varchar(55),
	[Supplier_Street] varchar(55),
	[Supplier_Email] varchar(55) not null,
	[Supplier_Description] text,
);


create table OrderTo (
OrderId int PRIMARY KEY IDENTITY (1, 1) NOT NULL,
SupplierId int,
OrderDate DateTime,
OrderNumber varchar(25),
IsReceived bit default 'False',
ReceivedDate date null,
constraint fk_supplier_ID foreign key (SupplierId) references Suppliers(SupplierId)  on delete no action on update no action
);



create table OrderItem (
Id int PRIMARY KEY IDENTITY (1, 1) NOT NULL,
OrderId int,
ItemToSellId int,
Amount decimal(20),
ItemManufacturerPrice decimal(20),
ItemReceivedCost decimal(20),
ItemReceivedPrice decimal(20),
constraint fk_Order_ID foreign key (OrderId) references OrderTo(OrderId)  on delete no action on update no action,
constraint fk_Item_ID foreign key (ItemToSellId) references ItemToSell(ID)  on delete no action on update no action,
);


create table Expense(
	ID int identity(1,1) primary key,
	ExpenseAmount decimal(20) not null,
	ExpenseDescription nvarchar(150) null,
	ExpenseDate Date not null
	);

create table OrderExpense(
	OrderId int,
	ExpenseId int,
	constraint fk_OrderTO_ID_foreign foreign key (OrderId) references OrderTo(OrderId)  on delete no action on update no action,
	constraint fk_Expense_ID_foreign foreign key (ExpenseId) references Expense(ID)  on delete no action on update no action
);

create table DisallowedDates(
	DisallowedDate date not null,
	DisallowedDateDescription nvarchar(50)
);
