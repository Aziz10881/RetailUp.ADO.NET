/*
*	Disallowed Modification on ItemToSell Table
*/
create trigger udtrDisallowDataModification
on [dbo].[ItemToSell] after update,delete
as
begin

	if not getdate() in (select [DisallowedDate] from [dbo].[DisallowedDates])
	begin
		rollback transaction;
		throw 51000,N'Operation not allowed',1;
	end


end
GO

/*
*	Disallowed Modification on OrderTo Table
*/
create trigger udtrDisallowDataModificationOnOrderTO
on [dbo].OrderTO after update,delete
as
begin

	if not getdate() in (select [DisallowedDate] from [dbo].[DisallowedDates])
	begin
		rollback transaction;
		throw 51000,N'Operation not allowed',1;
	end


end
go
/*
*	Disallowed Modification on Expense Table
*/
create trigger udtrDisallowDataModificationOnExpense
on [dbo].[Expense] after update,delete
as
begin

	if not getdate() in (select [DisallowedDate] from [dbo].[DisallowedDates])
	begin
		rollback transaction;
		throw 51000,N'Operation not allowed',1;
	end


end