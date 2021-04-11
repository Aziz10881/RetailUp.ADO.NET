/*
*      2nd Business Rule For Calculating Cost And Price Of Each Item In The Order Item Table
*	   Calculates by division of all expenses by the number of items in one order and adds the divided value to the manufacturer price to get the cost
*	   Then to calculate Price of an item adds 30 percent interest to the total cost of an item
*/


create trigger udtCalculatePrice
on dbo.OrderTo after update
as
begin

	declare @OrderStatus varchar(10);
	declare @orderID int;
	declare @AllExpense decimal(20)
	declare @expense decimal(20)
	set @AllExpense = 0

	set @OrderStatus = (select IsReceived From inserted)
	set @OrderID = (select OrderId from inserted)

	if(@orderStatus = 1)
	begin
		declare db_cursor cursor for

		select ExpenseAmount
		from dbo.Expense e
		join OrderExpense oe on e.ID = oe.ExpenseId
		Where oe.OrderId = @orderId

		open db_cursor
		fetch next from db_cursor into @expense

		while @@FETCH_STATUS = 0
		begin
			
			set @AllExpense += @expense
			fetch next from db_cursor into @expense

		end
		close db_cursor
		deallocate db_cursor

		update OrderItem set ItemReceivedCost = @AllExpense / (select count(*) from OrderItem where OrderId = @orderId) + ItemManufacturerPrice  where OrderId = @orderId
		update OrderItem set ItemReceivedPrice = ItemReceivedCost + (ItemReceivedCost / 100 * 30)  where OrderId = @orderId
	end
	else
	begin
		update OrderItem set ItemReceivedCost = 0 where OrderId = @orderId
		update OrderItem set ItemReceivedPrice = 0 where OrderId = @orderId
	end


end