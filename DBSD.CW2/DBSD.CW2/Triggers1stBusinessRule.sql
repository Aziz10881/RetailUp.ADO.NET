/*
*
* Trigger for updating Item to sell reminder
*
*
*
*
*/



create trigger udtUpdateItemReminder
ON dbo.OrderTo after update
as
begin
	declare @OrderStatus varchar(10);
	declare @orderID int;
	declare @ItemID int
	declare @ItemAmount int

	set @OrderStatus = (select IsReceived From inserted)
	set @OrderID = (select OrderId from inserted)
	
	if(@OrderStatus = 1)
	begin
		update OrderTo set ReceivedDate = GETDATE()  where OrderId = @orderID
		declare db_cursor cursor for
		select ItemToSellId, Amount from OrderItem where OrderId = @orderID

		open db_cursor
		fetch next from db_cursor into @ItemID, @ItemAmount

		while @@FETCH_STATUS = 0
		begin
			update ItemToSell set ItemRemained = ItemRemained + @ItemAmount where ID = @ItemID
			fetch next from db_cursor into @ItemID, @ItemAmount
		end 
		close db_cursor
		deallocate db_cursor
	end
	else
	begin
		update OrderTo set ReceivedDate = null where OrderId = @orderID
		declare db_cursor cursor for
		select ItemToSellId, Amount from OrderItem where OrderId = @orderID

		open db_cursor
		fetch next from db_cursor into @ItemID, @ItemAmount

		while @@FETCH_STATUS = 0
		begin
			update ItemToSell 
			set ItemRemained = ItemRemained - @ItemAmount where ID = @ItemID
			fetch next from db_cursor into @ItemID, @ItemAmount
		end 
		close db_cursor
		deallocate db_cursor
	end
end



update [dbo].[OrderTo] set IsReceived = 'False' where OrderId = 2
update ItemToSell set ItemRemained = 0
update OrderTo set ReceivedDate = null where OrderId = 7