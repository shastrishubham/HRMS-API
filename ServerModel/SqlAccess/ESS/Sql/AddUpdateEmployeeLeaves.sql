

---Sql.AddUpdateEmployeeLeaves

/*

DECLARE @employeeId uniqueidentifier = '00000000-0000-0000-0000-000000000000'
DECLARE @compId uniqueidentifier = '00000000-0000-0000-0000-000000000000'
DECLARE @createdBy uniqueidentifier =  '00000000-0000-0000-0000-000000000000'
DECLARE @id uniqueidentifier = '00000000-0000-0000-0000-000000000000'
DECLARE @MS_Leave_Id INT = 0;
DECLARE @TotalLeaves DECIMAL(18, 4) = 0;
DECLARE @AvailableLeves DECIMAL(18, 4) = 0;
 */


If(@id = '00000000-0000-0000-0000-000000000000')
BEGIN
	-- Create Employee Leaves
	SET @id = NEWID();
	INSERT INTO EMP_Leaves (Id, FormDate, CompId, CreatedBy, EMP_Info_Id, MS_Leave_Id, TotalLeaves, AvailableLeaves, Active)
	SELECT
				NEWID() AS Id,
				GETDATE() AS FormDate,
				@compId AS CompId,
				@createdBy AS CreatedBy,
				@employeeId AS EMP_Info_Id,
				Id AS MS_Leave_Id,
				Unit AS TotalLeaves,
				Unit AS AvailableLeaves,
				CAST(1 AS BIT) AS 'Active'
		FROM MS_Leave 
		WHERE CompId = @compId
				AND Active = 1

END
ELSE
BEGIN
	-- Update Employee Leave

	UPDATE EMP_Leaves 
		SET MS_Leave_Id = @MS_Leave_Id
			, TotalLeaves = @TotalLeaves
			, AvailableLeaves = @AvailableLeves
		WHERE Id = @id
END



-- Return the value of @id
SELECT @id AS Id;

