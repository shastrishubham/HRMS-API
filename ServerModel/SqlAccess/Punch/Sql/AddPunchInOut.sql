


--AddPunchInOut


--DECLARE @machineId nvarchar(50) = ''
--DECLARE @machineIp nvarchar(50) = ''
--DECLARE @compId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'
--DECLARE @createdBy UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'
--DECLARE @date DATE = CAST(GETDATE() AS date)
--DECLARE @branchId INT = 1
--DECLARE @empId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'
--DECLARE @inTime TIME(7) = '09:30:00.000'
--DECLARE @outTime TIME(7) = '18:30:00.000'



INSERT INTO Punch_InOutTime
	(Id, FormDate, MachineId, MachineIp, CompId, CreatedBy, Date, MS_Branch_Id, EMP_Info_Id, Intime, Outtime)
	VALUES
	(NEWID(), GETDATE(), @machineId, @machineIp, @compId, @createdBy, @date, @branchId, @empId, @inTime, @outTime)