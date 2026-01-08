


--- GetEmployeeAddressByAddressId.sql.Sql
/*
DECLARE @empAddressId UNIQUEIDENTIFIER = '3D360BA8-2B12-4DB7-BAC9-9ADFD3553E1E'

*/


SELECT [Id]
      ,[FormDate]
      ,[MachineId]
      ,[MachineIp]
      ,[CompId]
      ,[EMP_Info_Id]
      ,[AddressTypeId]
      ,[AddressLine1]
      ,[AddressLine2]
      ,[FlatNo]
      ,[Premise]
      ,[Road]
      ,[Area]
      ,[Town]
      ,[MS_State_Id]
      ,[MS_Country_Id]
      ,[PinCode]
      ,[FullAddress]
      ,[AddressProofDoc]
      ,[IsSameAsPresentAddress]
      ,[Active]
  FROM EMP_Addr 
  WHERE Id = @empAddressId