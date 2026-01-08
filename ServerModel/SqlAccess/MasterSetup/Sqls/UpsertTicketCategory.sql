
/*
--UpsertTicketCategory.Sql

--select * from MS_TicketCat

DECLARE @Id int = 1;
DECLARE @CompId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
DECLARE @TicketCategory nvarchar(MAX) = 'name23';


*/

MERGE INTO MS_TicketCat as Target
USING (select @Id as Id, @TicketCategory as TicketCategory) AS source
    ON (Target.Id = source.Id) 

WHEN MATCHED THEN
	UPDATE SET
		TicketCategory = @TicketCategory


WHEN NOT MATCHED THEN
        INSERT (
             CompId
			,TicketCategory)
        VALUES
           (
            @CompId
		   ,@TicketCategory)

OUTPUT INSERTED.Id;


