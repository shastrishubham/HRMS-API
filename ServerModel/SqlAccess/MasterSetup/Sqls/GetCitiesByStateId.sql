
/*
---GetCitiesByStateId

DECLARE @stateId int = 1;

*/

SELECT c1.Id
	  ,c1.CityName
	  ,c1.CityCode
	  ,c1.MS_States_Id
	  ,s.StateName
	  ,s.StateCode
	  ,c1.MS_Country_id
      ,c.CountryName
      ,CountryCode
  FROM MS_Cities as c1
	INNER JOIN MS_Country as c on c.Id = c1.MS_Country_Id
	INNER JOIN MS_States as s on s.Id = c1.MS_States_Id
  WHERE c1.MS_States_Id = @stateId