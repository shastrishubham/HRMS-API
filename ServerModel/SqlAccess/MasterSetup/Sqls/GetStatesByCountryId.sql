


/*
---GetStatesByCountryId

DECLARE @countryId int = 1;
*/

SELECT s.Id
	  ,s.StateName
	  ,s.StateCode
	  ,s.MS_Country_Id
      ,c.CountryName
      ,CountryCode
  FROM MS_States as s
	INNER JOIN MS_Country as c on c.Id = s.MS_Country_Id
  WHERE s.MS_Country_Id = @countryId