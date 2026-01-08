


/*
---GetTicketCategoriesByCompId.Sql

DECLARE @compId uniqueidentifier = '00000000-0000-0000-0000-000000000000';
*/

SELECT s.Id
	  ,s.FormDate
	  ,s.CompId
	  ,s.TicketCategory
  FROM MS_TicketCat as s
  WHERE s.CompId = @compId