using ServerModel.Data;
using ServerModel.Model.Punch;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ServerModel.SqlAccess.Attendance
{
    public static class AttendanceSetupAccess
    {
        public static List<EmployeeAttendanceInfo> GetEmployeePunchesById(Guid employeeId, int month, int year)
        {
            List<EmployeeAttendanceInfo> empPunches = new List<EmployeeAttendanceInfo>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = Attendance.Sql.GetEmployeePunchesById;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EmpId", employeeId);
                    command.Parameters.AddWithValue("@Year", year);
                    command.Parameters.AddWithValue("@Month", month);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EmployeeAttendanceInfo attendanceInfo = new EmployeeAttendanceInfo
                            {
                                WorkDate = reader["WorkDate"] != DBNull.Value ? Convert.ToDateTime(reader["WorkDate"].ToString()) : DateTime.MinValue,
                                PunchIn = reader["PunchIn"] != DBNull.Value && TimeSpan.TryParse(reader["PunchIn"].ToString(), out var punchInTime) ? punchInTime : TimeSpan.Zero,
                                PunchOut = reader["PunchOut"] != DBNull.Value && TimeSpan.TryParse(reader["PunchOut"].ToString(), out var punchOutTime) ? punchOutTime  : TimeSpan.Zero,
                                LeaveType = reader["LeaveType"] != DBNull.Value ? reader["LeaveType"].ToString() : "",
                                PublicHolidayName = reader["PublicHoliday"] != DBNull.Value ? reader["PublicHoliday"].ToString() : ""
                            };

                            empPunches.Add(attendanceInfo);
                        }
                    }
                }
            }

            return empPunches;
        }
    }
}
