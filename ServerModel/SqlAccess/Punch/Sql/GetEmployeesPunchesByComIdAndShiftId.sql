---- Sql.GetEmployeesPunchesByComIdAndShiftId

/*
DECLARE @compId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000000'
DECLARE @shiftId INT = 4005
DECLARE @filterDate DATE = '2025-09-01'  -- Pass the date to filter by month & year

*/

;WITH PunchesRanked AS (
    SELECT
        attd.EMP_Info_Id,
        attd.punchDate,
        attd.punchTime,
        ROW_NUMBER() OVER (PARTITION BY attd.EMP_Info_Id, attd.punchDate ORDER BY attd.punchTime ASC) AS rank_in,
        ROW_NUMBER() OVER (PARTITION BY attd.EMP_Info_Id, attd.punchDate ORDER BY attd.punchTime DESC) AS rank_out
    FROM
        EMP_Punch AS attd
    WHERE 
        attd.PunchTime IS NOT NULL
        AND attd.CompId = @compId
        AND MONTH(attd.punchDate) = MONTH(@filterDate)
        AND YEAR(attd.punchDate) = YEAR(@filterDate)
)
, FirstIn AS (
    SELECT
        EMP_Info_Id,
        punchDate,
        punchTime AS first_in_time
    FROM PunchesRanked
    WHERE rank_in = 1
)
, LastOut AS (
    SELECT
        EMP_Info_Id,
        punchDate,
        punchTime AS last_out_time
    FROM PunchesRanked
    WHERE rank_out = 1
)
SELECT
    f.EMP_Info_Id,
    emp.FullName AS EmployeeName,
    desg.DesignationName,
    branch.BranchName,
    shiftMS.ShiftName,
    eShift.StartFrom,
    eShift.EndTo,
    eShift.IsPermanentShift,
    f.punchDate AS PunchDate,
    f.first_in_time AS InTime,
    l.last_out_time AS OutTime
FROM
    FirstIn AS f
    LEFT JOIN LastOut l ON f.EMP_Info_Id = l.EMP_Info_Id AND f.punchDate = l.punchDate
    INNER JOIN EMP_Info AS emp ON emp.Id = f.EMP_Info_Id
    INNER JOIN MS_Designation AS desg ON desg.Id = emp.MS_Designation_Id
    INNER JOIN MS_Branch AS branch ON branch.Id = emp.MS_Branch_Id
    INNER JOIN EMP_Shift AS eShift ON eShift.EMP_Info_Id = f.EMP_Info_Id AND eShift.IsAmmend = 0
    INNER JOIN MS_Shift AS shiftMS ON eShift.MS_Shift_Id = shiftMS.Id
WHERE
    eShift.MS_Shift_Id = @shiftId
ORDER BY
    f.EMP_Info_Id,
    f.punchDate;
