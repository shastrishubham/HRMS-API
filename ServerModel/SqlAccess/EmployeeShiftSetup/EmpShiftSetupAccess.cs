using ServerModel.Data;
using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ServerModel.SqlAccess.EmployeeShiftSetup
{
    public class EmpShiftSetupAccess
    {
        public static List<EmployeeShiftInformation> GetEmlployeeShiftByBranchAndShift(int branchId, int shiftId)
        {
            List<EmployeeShiftInformation> empShifts= new List<EmployeeShiftInformation>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = EmployeeShiftSetup.Sql.GetEmployeeShiftsByBranchIdAndShiftId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@branchId", branchId);
                    command.Parameters.AddWithValue("@shiftId", shiftId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var salarySetup = new EmployeeShiftInformation
                            {
                                Id = Guid.Parse(reader["Id"].ToString()),
                                FormDate = Convert.ToDateTime(reader["FormDate"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                EMP_Info_Id = reader["EMP_Info_Id"] != DBNull.Value ? Guid.Parse(reader["EMP_Info_Id"].ToString()) : Guid.Empty,
                                EmployeeFullName = reader["EmployeeFullName"] != DBNull.Value ? reader["EmployeeFullName"].ToString() : "",
                                MS_Shift_Id = reader["MS_Shift_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Shift_Id"].ToString()) : 0,
                                ShiftName = reader["ShiftName"] != DBNull.Value ? reader["ShiftName"].ToString() : "",
                                StartFrom = reader["StartFrom"] != DBNull.Value ? Convert.ToDateTime(reader["StartFrom"].ToString()) : DateTime.MinValue,
                                EndTo = reader["EndTo"] != DBNull.Value ? Convert.ToDateTime(reader["EndTo"].ToString()) : DateTime.MinValue,
                                IsPermanentShift = reader["IsPermanentShift"] != DBNull.Value ? Convert.ToBoolean(reader["IsPermanentShift"].ToString()) : false,
                                BranchName = reader["BranchName"] != DBNull.Value ? reader["BranchName"].ToString() : "",
                                EmployeeBranchId = reader["EmployeeBranchId"] != DBNull.Value ? Convert.ToInt32(reader["EmployeeBranchId"].ToString()) : 0,
                            };

                            empShifts.Add(salarySetup);
                        }
                    }
                }
            }

            return empShifts;
        }

        public static List<EmployeeShiftInformation> GetEmployeeShiftDetailByBranchId(Guid compId, int branchId)
        {
            List<EmployeeShiftInformation> empShifts = new List<EmployeeShiftInformation>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = EmployeeShiftSetup.Sql.GetEmployeeShiftDetailByBranchId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@branchId", branchId);
                    command.Parameters.AddWithValue("@compId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var shift = new EmployeeShiftInformation
                            {
                                EMP_Info_Id = reader["EMP_Info_Id"] != DBNull.Value ? Guid.Parse(reader["EMP_Info_Id"].ToString()) : Guid.Empty,
                                EmployeeFullName = reader["EmployeeName"] != DBNull.Value ? reader["EmployeeName"].ToString() : "",
                                BranchName = reader["BranchName"] != DBNull.Value ? reader["BranchName"].ToString() : "",
                                DesignationName = reader["DesignationName"] != DBNull.Value ? reader["DesignationName"].ToString() : "",
                                DepartmentName = reader["DepartmentName"] != DBNull.Value ? reader["DepartmentName"].ToString() : "",
                                Id = reader["Id"] != DBNull.Value ? Guid.Parse(reader["Id"].ToString()) : Guid.Empty,
                                ShiftName = reader["ShiftName"] != DBNull.Value ? reader["ShiftName"].ToString() : "",
                                StartFrom = reader["StartFrom"] != DBNull.Value ? Convert.ToDateTime(reader["StartFrom"].ToString()) : DateTime.MinValue,
                                EndTo = reader["EndTo"] != DBNull.Value ? Convert.ToDateTime(reader["EndTo"].ToString()) : DateTime.MinValue,
                                IsPermanentShift = reader["IsPermanentShift"] != DBNull.Value ? Convert.ToBoolean(reader["IsPermanentShift"].ToString()) : false,
                                BranchId = reader["MS_Branch_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Branch_Id"].ToString()) : 0,
                                MS_Shift_Id = reader["MS_Shift_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Shift_Id"].ToString()) : 0,
                            };

                            empShifts.Add(shift);
                        }
                    }
                }
            }

            return empShifts;
        }
    }
}
