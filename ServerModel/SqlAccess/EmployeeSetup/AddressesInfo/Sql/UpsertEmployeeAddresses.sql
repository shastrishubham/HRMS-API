--DECLARE @json NVARCHAR(MAX) = 
--'[
--    {
--        "Id": "00000000-0000-0000-0000-000000000000",
--        "FormDate": "2025-08-14",
--        "MachineId": 101,
--        "MachineIp": "192.168.1.10",
--        "CompId": "00000000-0000-0000-0000-000000000000",
--        "EMP_Info_Id": "00000000-0000-0000-0000-000000000000",
--        "AddressTypeId": 1,
--        "AddressLine1": "Addr1",
--        "AddressLine2": "Addr2",
--        "FlatNo": "A-101",
--        "Premise": "Premise1",
--        "Road": "Road1",
--        "Area": "Area1",
--        "Town": "Town1",
--        "MS_State_Id": 10,
--        "MS_Country_Id": 100,
--        "PinCode": "123456",
--        "FullAddress": "Full Address 1",
--		   "AddressProofDoc": "AddressProofDoc",
--        "IsSameAsPresentAddress": 1,
--        "Active": 1
--    },
--    {
--        "Id": "F91FDF4E-FB78-4D4F-A8CE-BFCE87819441",
--        "FormDate": "2025-08-14",
--        "MachineId": 102,
--        "MachineIp": "192.168.1.11",
--        "CompId": "00000000-0000-0000-0000-000000000000",
--        "EMP_Info_Id": "00000000-0000-0000-0000-000000000000",
--        "AddressTypeId": 2,
--        "AddressLine1": "Addr3",
--        "AddressLine2": "Addr5",
--        "FlatNo": "B-302",
--        "Premise": "Premise2",
--        "Road": "Road2",
--        "Area": "Area2",
--        "Town": "Town2",
--        "MS_State_Id": 11,
--        "MS_Country_Id": 101,
--        "PinCode": "654321",
--        "FullAddress": "Full Address 2",
 --			"AddressProofDoc": "AddressProofDoc",
--        "IsSameAsPresentAddress": 0,
--        "Active": 1
--    }
--]';

-- Use OPENJSON to parse the JSON into rows
MERGE HRMS.dbo.EMP_Addr AS Target
USING OPENJSON(@json)
WITH (
    Id UNIQUEIDENTIFIER,
    FormDate DATE,
    MachineId INT,
    MachineIp NVARCHAR(50),
    CompId UNIQUEIDENTIFIER,
    EMP_Info_Id UNIQUEIDENTIFIER,
    AddressTypeId INT,
    AddressLine1 NVARCHAR(100),
    AddressLine2 NVARCHAR(100),
    FlatNo NVARCHAR(10),
    Premise NVARCHAR(50),
    Road NVARCHAR(100),
    Area NVARCHAR(150),
    Town NVARCHAR(100),
    MS_State_Id INT,
    MS_Country_Id INT,
    PinCode NVARCHAR(50),
    FullAddress NVARCHAR(MAX),
	AddressProofDoc NVARCHAR(MAX),
    IsSameAsPresentAddress BIT,
    Active BIT
) AS Source
ON (Target.Id = Source.Id AND Target.EMP_Info_Id = Source.EMP_Info_Id)

WHEN MATCHED THEN
    UPDATE SET 
        Target.FormDate = Source.FormDate,
        Target.MachineId = Source.MachineId,
        Target.MachineIp = Source.MachineIp,
        Target.CompId = Source.CompId,
        Target.AddressTypeId = Source.AddressTypeId,
        Target.AddressLine1 = Source.AddressLine1,
        Target.AddressLine2 = Source.AddressLine2,
        Target.FlatNo = Source.FlatNo,
        Target.Premise = Source.Premise,
        Target.Road = Source.Road,
        Target.Area = Source.Area,
        Target.Town = Source.Town,
        Target.MS_State_Id = Source.MS_State_Id,
        Target.MS_Country_Id = Source.MS_Country_Id,
        Target.PinCode = Source.PinCode,
        Target.FullAddress = Source.FullAddress,
		Target.AddressProofDoc = Source.AddressProofDoc,
        Target.IsSameAsPresentAddress = Source.IsSameAsPresentAddress,
        Target.Active = Source.Active

WHEN NOT MATCHED BY TARGET THEN
    INSERT (Id, FormDate, MachineId, MachineIp, CompId, EMP_Info_Id, AddressTypeId, 
            AddressLine1, AddressLine2, FlatNo, Premise, Road, Area, Town, 
            MS_State_Id, MS_Country_Id, PinCode, FullAddress, AddressProofDoc, IsSameAsPresentAddress, Active)
    VALUES (NEWID(), Source.FormDate, Source.MachineId, Source.MachineIp, Source.CompId, 
            Source.EMP_Info_Id, Source.AddressTypeId, Source.AddressLine1, Source.AddressLine2, 
            Source.FlatNo, Source.Premise, Source.Road, Source.Area, Source.Town, 
            Source.MS_State_Id, Source.MS_Country_Id, Source.PinCode, Source.FullAddress, Source.AddressProofDoc,
            Source.IsSameAsPresentAddress, Source.Active);
