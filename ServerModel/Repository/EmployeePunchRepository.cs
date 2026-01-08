using ServerModel.Data;
using ServerModel.Database;
using ServerModel.Interfaces;
using ServerModel.Model.Employee;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Repository
{
    public class EmployeePunchRepository
    {
        private IRespository<EMP_Punch> respository;

        public EmployeePunchRepository()
        {
            this.respository = new Repository<EMP_Punch>();
        }

        public void AddUpdateEmployeePunch(EmployeePunchInformation employeePunchInformation)
        {
            if(employeePunchInformation.PunchDate == null)
            {
                employeePunchInformation.PunchDate = DateTime.Now;
            }

            if(employeePunchInformation.PunchTime == null)
            {
                employeePunchInformation.PunchTime = TimeSpan.Parse(DateTime.Now.ToString());
            }

            EMP_Punch punchInfoDb = GetEmpPunchInfoDbFromEmpPunchInformation(employeePunchInformation);
            this.respository.Insert(punchInfoDb);
            this.respository.Save();
        }


        public List<EmployeePunchInformation> GetEmployeesPunchesByComIdAndShiftId(Guid companyId, int shiftId, DateTime filterDate)
        {
            List<EmployeePunchInformation> empPunches = new List<EmployeePunchInformation>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = SqlAccess.Punch.PunchSql.GetEmployeesPunchesByComIdAndShiftId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", companyId);
                    command.Parameters.AddWithValue("@shiftId", shiftId);
                    command.Parameters.AddWithValue("@filterDate", filterDate);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var employeePunchInformation = new EmployeePunchInformation
                            {
                                EmpId = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                //EmpId = reader["EMP_Info_Id"] != DBNull.Value ? Guid.Parse(reader["EMP_Info_Id"].ToString()) : Guid.Empty,
                                //MS_SLHeads_Id = reader["MS_SLHeads_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_SLHeads_Id"]) : 0,

                                EmpName = reader["EmployeeName"] != DBNull.Value ? reader["EmployeeName"].ToString() : "",
                                DesignationName = reader["DesignationName"] != DBNull.Value ? reader["DesignationName"].ToString() : "",
                                BranchName = reader["BranchName"] != DBNull.Value ? reader["BranchName"].ToString() : "",
                                ShiftName = reader["ShiftName"] != DBNull.Value ? reader["ShiftName"].ToString() : "",
                                StartFrom = reader["StartFrom"] != DBNull.Value ? Convert.ToDateTime(reader["StartFrom"]).Date : DateTime.MinValue.Date,
                                EndTo = reader["EndTo"] != DBNull.Value ? Convert.ToDateTime(reader["EndTo"]).Date : DateTime.MaxValue.Date,

                                IsPermanentShift = reader["IsPermanentShift"] != DBNull.Value ? Convert.ToBoolean(reader["IsPermanentShift"].ToString()) : true,
                                PunchDate = reader["PunchDate"] != DBNull.Value ? Convert.ToDateTime(reader["PunchDate"]).Date : DateTime.MinValue.Date,
                             
                            };

                            if (!TimeSpan.TryParse(reader["InTime"].ToString(), out TimeSpan Intime))
                            {
                                // handle validation error
                            };
                            employeePunchInformation.Intime = Intime;

                            if (!TimeSpan.TryParse(reader["OutTime"].ToString(), out TimeSpan Outtime))
                            {
                                // handle validation error
                            };
                            employeePunchInformation.OutTime = Outtime;

                            empPunches.Add(employeePunchInformation);
                        }
                    }
                }
            }

            return empPunches;
        }

        public static List<EmployeePunchInformation> GetPresentEmpByBranch(DateTime attendaceDate, int branchId)
        {
            List<EmployeePunchInformation> employeeAttendanceInfo = new List<EmployeePunchInformation>();
            string strConString = DbContext.ConnectionString;

            //using (var connection = new SqlConnection(strConString))
            //{
            //    connection.Open();
            //   string sql = PunchSql.GetPresentEmpByBranch;
            //    using (var command = new SqlCommand(sql, connection))
            //    {
            //        command.Parameters.AddWithValue("@attendaceDate", attendaceDate.Date);
            //        command.Parameters.AddWithValue("@branchId", branchId);

            //        using (var reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                var empAttendanceInfo = new EmployeePunchInformation
            //                {
            //                    //EmpId = Guid.Parse(reader["EMP_Info_Id"].ToString()),
            //                    //FullName = reader["FullName"].ToString(),
            //                    //DesignationName = reader["DesignationName"].ToString(),

            //                    //BranchName = reader["BranchName"].ToString(),
            //                    //ShiftName = reader["ShiftName"].ToString(),
            //                    //AttendanceDate = Convert.ToDateTime(reader["AttendanceDate"]).Date
            //                };


            //                if (!TimeSpan.TryParse(reader["PunchIn"].ToString(), out TimeSpan Intime))
            //                {
            //                    // handle validation error
            //                };
            //               // empAttendanceInfo.PunchIntime = Intime;

            //                if (!TimeSpan.TryParse(reader["PunchOut"].ToString(), out TimeSpan Outtime))
            //                {
            //                    // handle validation error
            //                };
            //              //  empAttendanceInfo.PunchOuttime = Outtime;

            //                employeeAttendanceInfo.Add(empAttendanceInfo);
            //            }
            //        }
            //    }
            //}

            return employeeAttendanceInfo;
        }


        #region DB Model to Server Model Data Binding
        private EMP_Punch GetEmpPunchInfoDbFromEmpPunchInformation(EmployeePunchInformation employeePunchInformation)
        {
            EMP_Punch punchInfoDb = new EMP_Punch();
            punchInfoDb.MachineId = "AJINKYA-PC";
            punchInfoDb.MachineIp = "0.0.0.0";
            punchInfoDb.CompId = employeePunchInformation.CompId;
            punchInfoDb.EMP_Info_Id = employeePunchInformation.EMP_Info_Id;
            punchInfoDb.PunchTime = employeePunchInformation.PunchTime;
            punchInfoDb.PunchDate = employeePunchInformation.PunchDate;
            punchInfoDb.Status = employeePunchInformation.Status;
            return punchInfoDb;
        }

       
        #endregion
    }
}
