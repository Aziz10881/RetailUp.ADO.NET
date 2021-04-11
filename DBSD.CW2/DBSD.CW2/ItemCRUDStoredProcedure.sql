/*
** CRUD For Item To Sell **
*/



/*
** Insert New Item **
*/


create procedure udspCreateNewItemToSell (
	 @ItemCategoryID int
	,@ItemName nvarchar(200)
	,@ItemBrand nvarchar(50)
	,@ItemDescription nvarchar(100)
	,@ItemModel nvarchar(50)
	,@ItemAddedDate date
	,@ItemImage varchar(100)
	,@IsActive bit
	,@ItemRemained int
)
as
begin
	set nocount on
	insert into dbo.ItemToSell (
	ItemCategoryID
	,ItemName
	,ItemBrand
	,ItemDescription
	,ItemModel
	,ItemAddedDate
	,ItemImage
	,IsActive
	,ItemRemained
	) values (
	 @ItemCategoryID
	,@ItemName
	,@ItemBrand
	,@ItemDescription
	,@ItemModel
	,@ItemAddedDate
	,@ItemImage
	,@IsActive
	,@ItemRemained
)
end;
--------------------------------------------------------------------

go
/*
** Update Item To Sell By ID **
*/


create procedure udspUpdateItemToSellBYID (
	 @ID int
	,@ItemCategoryID int
	,@ItemName nvarchar(200)
	,@ItemBrand nvarchar(50)
	,@ItemDescription nvarchar(100)
	,@ItemModel nvarchar(50)
	,@ItemAddedDate date
	,@ItemImage varchar(100)
	,@IsActive bit
	,@ItemRemained int
)
as
begin
	set nocount on

	update dbo.ItemToSell
	set 
	 ItemCategoryID = @ItemCategoryID
	,ItemName = @ItemName
	,ItemBrand = @ItemBrand
	,ItemDescription = @ItemDescription
	,ItemModel = @ItemModel
	,ItemAddedDate = @ItemAddedDate
	,ItemImage = @ItemImage
	,IsActive = @IsActive
	,ItemRemained = @ItemRemained
	where ID = @ID

end

-----------------------------------------------------

go

/*
** Delete Item By ID **
*/

create procedure udspDeleteItemToSellByID (@ID int)
as 
begin
	delete from dbo.ItemToSell where ID = @ID
end

------------------------------------------------------
go
/*
** Filter Received Items BY Item Name, Category, Received Date, Supplier Name **
*/


create procedure udspFilterItems (
	@ItemName nvarchar(200)
	, @Category varchar(75)
	, @receivedDate date
	, @Supplier text
	, @Rows int
	, @pageSize int
	)
as
begin
	select
		  s.Supplier_Name
		, ot.OrderDate
		, ot.ReceivedDate
		, its.ItemName
		, its.ItemBrand
		, ic.CategoryName
		, oi.ItemReceivedCost
		, oi.ItemReceivedPrice
	from OrderTo ot
	join OrderItem oi on ot.OrderId = oi.OrderId
	join Suppliers s on ot.SupplierId = s.SupplierId
	join ItemToSell its on oi.ItemToSellId = its.ID
	join ItemCategory ic on its.ItemCategoryID = ic.ID
	where ot.IsReceived = 'True' and its.ItemName like @ItemName and ic.CategoryName like @Category and ot.ReceivedDate =
	@receivedDate and s.SupplierName like @Supplier
	order by ot.ReceivedDate desc
	offset @rows rows fetch next @pageSize rows only
end

-----------------------------------------------------

