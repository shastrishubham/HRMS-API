using ServerModel.Data;
using ServerModel.Model.Employee;
using ServerModel.Model.Masters;
using ServerModel.Model.Reports;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ServerModel.SqlAccess.Reports
{
    public class ReportAccess
    {
        public static List<BranchWiseEmpReportInfo> branchWiseEmpReports(Guid compId, int branchId)
        {
            List<BranchWiseEmpReportInfo> branchWiseEmps = new List<BranchWiseEmpReportInfo>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = Reports.Sql.BranchWiseEmpReports;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);
                    command.Parameters.AddWithValue("@branchId", branchId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BranchWiseEmpReportInfo branchWise = new BranchWiseEmpReportInfo
                            {
                                EmployeeInfo = new EmployeeInformation
                                {
                                    Id = Guid.Parse(reader["Id"].ToString()),
                                    EmpId = reader["EmpId"] != DBNull.Value ? reader["EmpId"].ToString() : string.Empty,
                                    EmpType = reader["EmpType"] != DBNull.Value ? reader["EmpType"].ToString() : string.Empty,
                                    FullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : string.Empty,
                                    Gender = reader["Gender"] != DBNull.Value ? reader["Gender"].ToString() : string.Empty,
                                    DOB = reader["DOB"] != DBNull.Value ? Convert.ToDateTime(reader["DOB"].ToString()) : DateTime.MinValue,
                                    PANNo = reader["PANNo"] != DBNull.Value ? reader["PANNo"].ToString() : string.Empty,
                                    AadharNo = reader["AadharNo"] != DBNull.Value ? reader["AadharNo"].ToString() : string.Empty,
                                    MS_Designation_Id = reader["MS_Designation_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Designation_Id"].ToString()) : 0,
                                    DesignationName = reader["DesignationName"] != DBNull.Value ? reader["DesignationName"].ToString() : string.Empty,
                                    MS_Dept_Id = reader["MS_Dept_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Dept_Id"].ToString()) : 0,
                                    MS_Branch_Id = reader["MS_Branch_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Branch_Id"].ToString()) : 0,
                                },
                                DepartmentInfo = new DepartmentInfo()
                                {
                                    Id = reader["MS_Dept_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Dept_Id"].ToString()) : 0,
                                    DepartmentName = reader["DepartmentName"] != DBNull.Value ? reader["DepartmentName"].ToString() : string.Empty,
                                    DepartmentCode = reader["DepartmentCode"] != DBNull.Value ? reader["DepartmentCode"].ToString() : string.Empty,
                                },
                                BranchInfo = new BranchInfo()
                                {
                                    Id = reader["MS_Branch_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Branch_Id"].ToString()) : 0,
                                    BranchCode = reader["BranchCode"] != DBNull.Value ? reader["BranchCode"].ToString() : string.Empty,
                                    BranchName = reader["BranchName"] != DBNull.Value ? reader["BranchName"].ToString() : string.Empty,
                                    MailAlias = reader["MailAlias"] != DBNull.Value ? reader["MailAlias"].ToString() : string.Empty,
                                    IsMainBranch = reader["IsMainBranch"] != DBNull.Value ? Convert.ToBoolean(reader["IsMainBranch"].ToString()) : false,
                                },
                                ShiftInfo = new ShiftInfo()
                                {
                                    ShiftName = reader["ShiftName"] != DBNull.Value ? reader["ShiftName"].ToString() : string.Empty,
                                    ShiftCode = reader["ShiftCode"] != DBNull.Value ? reader["ShiftCode"].ToString() : string.Empty,
                                    StartTime = reader["StartTime"] != DBNull.Value ? TimeSpan.Parse(reader["StartTime"].ToString()) : new TimeSpan(),
                                    EndTime = reader["EndTime"] != DBNull.Value ? TimeSpan.Parse(reader["EndTime"].ToString()) : new TimeSpan(),
                                    WeeklyOffDay = reader["WeeklyOffDay"] != DBNull.Value ? reader["WeeklyOffDay"].ToString() : string.Empty
                                },
                                DesignationInfo = new DesignationInfo()
                                {
                                    Id = reader["MS_Designation_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Designation_Id"].ToString()) : 0,
                                    DesignationName = reader["DesignationName"] != DBNull.Value ? reader["DesignationName"].ToString() : string.Empty,
                                    DesignationCode = reader["DesignationCode"] != DBNull.Value ? reader["DesignationCode"].ToString() : string.Empty,
                                }
                            };

                            branchWiseEmps.Add(branchWise);
                        }
                    }
                }
            }

            return branchWiseEmps;
        }
    }
}
