/*
*
*
*
*	Tracks inserted new items on the table
*
*
*
*/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER udtrAuditWhenInserted
   ON  dbo.ItemToSell
   AFTER INSERT not for replication
AS 
BEGIN
	declare @itemId bigint 

	select @itemId = ID from inserted

	insert into dbo.ItemToSellAudit
		([ItemId]
      ,[ItemCategoryID]
      ,[ItemName]
      ,[ItemBrand]
      ,[ItemDescription]
      ,[ItemModel]
      ,[ItemAddedDate]
      ,[ItemImage]
      ,[IsActive]
      ,[ItemRemained]
      ,[Operation],
	   [CreatedOn])
	  select 
		[ID]
      ,[ItemCategoryID]
      ,[ItemName]
      ,[ItemBrand]
      ,[ItemDescription]
      ,[ItemModel]
      ,[ItemAddedDate]
      ,[ItemImage]
      ,[IsActive]
      ,[ItemRemained]
	  ,'inserted'
	  ,GetDate()
  FROM [RetailUP].[dbo].[ItemToSell] where ID = @itemId
END
GO


/*
*
*
*	Tracks updates and deletes on the tables
*
*
*
*/



create trigger udtrUpdateDeleteAuditItemToSell
on ItemToSell after update,delete
as 
begin
	
	if exists(select 1 from inserted) and exists(select 1 from deleted)
		insert into dbo.ItemToSellAudit
		([ItemId]
      ,[ItemCategoryID]
      ,[ItemName]
      ,[ItemBrand]
      ,[ItemDescription]
      ,[ItemModel]
      ,[ItemAddedDate]
      ,[ItemImage]
      ,[IsActive]
      ,[ItemRemained]
      ,[Operation],
	   [CreatedOn])
	   select 
		[ID]
      ,[ItemCategoryID]
      ,[ItemName]
      ,[ItemBrand]
      ,[ItemDescription]
      ,[ItemModel]
      ,[ItemAddedDate]
      ,[ItemImage]
      ,[IsActive]
      ,[ItemRemained]
	  ,'update'
	  ,GetDate()
	  from deleted
	else if not exists(select 1 from inserted) and exists(select 1 from deleted)
		insert into dbo.ItemToSellAudit
		([ItemId]
      ,[ItemCategoryID]
      ,[ItemName]
      ,[ItemBrand]
      ,[ItemDescription]
      ,[ItemModel]
      ,[ItemAddedDate]
      ,[ItemImage]
      ,[IsActive]
      ,[ItemRemained]
      ,[Operation],
	   [CreatedOn])
	   select 
		[ID]
      ,[ItemCategoryID]
      ,[ItemName]
      ,[ItemBrand]
      ,[ItemDescription]
      ,[ItemModel]
      ,[ItemAddedDate]
      ,[ItemImage]
      ,[IsActive]
      ,[ItemRemained]
	  ,'deleted'
	  ,GetDate()
	  from DELETED

end