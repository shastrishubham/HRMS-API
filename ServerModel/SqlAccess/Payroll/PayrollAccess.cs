using ServerModel.Data;
using ServerModel.Model.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace ServerModel.SqlAccess.Payroll
{
    public static class PayrollAccess
    {
        public static List<dynamic> GetEmpPayrollDetailsByBranchId(int year, int month, int branchId, Guid employeeId)
        {
            List<dynamic> employeesPayrollDetails = new List<dynamic>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = Payroll.Sql.GetEmpPayrollByBranchMonth;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@Year", year);
                    command.Parameters.AddWithValue("@Month", month);
                    command.Parameters.AddWithValue("@BranchId", branchId);
                    command.Parameters.AddWithValue("@empId", employeeId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dynamic row = new ExpandoObject();
                            var rowDict = (IDictionary<string, object>)row;

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                rowDict[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            }

                            employeesPayrollDetails.Add(row);
                        }
                    }
                }
            }

            return employeesPayrollDetails;
        }

        public static List<PayrollInformation> GetCalculatedPayrollDetailsByBranchId(int year, int month, int branchId, Guid compId)
        {
            List<PayrollInformation> payrolls = new List<PayrollInformation>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = Payroll.Sql.GetCalculatedPayrollDetailsByBranch_ByShiftAllowance;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Year", year);
                    command.Parameters.AddWithValue("@Month", month);
                    command.Parameters.AddWithValue("@BranchId", branchId);
                    command.Parameters.AddWithValue("@CompId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PayrollInformation payroll = new PayrollInformation
                            {
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                EmployeeName = reader["FullName"].ToString(),
                                EMP_Info_MSBranchId = reader["BranchId"] != DBNull.Value ? Convert.ToInt32(reader["BranchId"].ToString()) : 0,
                                BranchName = reader["BranchName"] != DBNull.Value ? reader["BranchName"].ToString() : "",
                                TotalMonthDays = reader["TotalMonthDays"] != DBNull.Value ? Convert.ToInt32(reader["TotalMonthDays"].ToString()) : 0,
                                PunchDays = reader["PunchDays"] != DBNull.Value ? Convert.ToInt32(reader["PunchDays"].ToString()) : 0,
                                PaidLeaveDays = reader["PaidLeaveDays"] != DBNull.Value ? Convert.ToDecimal(reader["PaidLeaveDays"].ToString()) : 0,
                                UnpaidLeaveDays = reader["UnpaidLeaveDays"] != DBNull.Value ? Convert.ToDecimal(reader["UnpaidLeaveDays"].ToString()) : 0,
                                HolidayDays = reader["HolidayDays"] != DBNull.Value ? Convert.ToInt32(reader["HolidayDays"].ToString()) : 0,
                                TotalPaidDays = reader["PaidDays"] != DBNull.Value ? Convert.ToInt32(reader["PaidDays"]) : 0,
                                TotalEarning = reader["TotalEarnings"] != DBNull.Value ? Convert.ToDecimal(reader["TotalEarnings"]) : 0,
                                TotalDeduction = reader["TotalDeductions"] != DBNull.Value ? Convert.ToDecimal(reader["TotalDeductions"]) : 0,
                                ShiftAllowanceDays = reader["ShiftAllowanceDays"] != DBNull.Value ? Convert.ToDecimal(reader["ShiftAllowanceDays"].ToString()) : 0,
                                TotalShiftAllowance = reader["TotalShiftAllowance"] != DBNull.Value ? Convert.ToDecimal(reader["TotalShiftAllowance"].ToString()) : 0,
                                LoanDeductions = reader["LoanDeductions"] != DBNull.Value ? Convert.ToDecimal(reader["LoanDeductions"]) : 0,
                                NetPay = reader["FinalPay"] != DBNull.Value ? Convert.ToDecimal(reader["FinalPay"]) : 0,
                                PayrollStatus = reader["PayrollStatus"] != DBNull.Value ? reader["PayrollStatus"].ToString() : "NotProcessed",
                                Comment = reader["Comment"] != DBNull.Value ? reader["Comment"].ToString() : ""
                            };

                            payrolls.Add(payroll);
                        }
                    }
                }
            }

            return payrolls;
        }

        public static List<EmployeePayrollInformation> GetEmployeePayrollInformation(Guid employeeId, int month, int year)
        {
            List<EmployeePayrollInformation> payrolls = new List<EmployeePayrollInformation>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = Payroll.Sql.GetEmployeeSalarySlipByMonthAndId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EmpId", employeeId);
                    command.Parameters.AddWithValue("@Year", year);
                    command.Parameters.AddWithValue("@Month", month);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EmployeePayrollInformation payroll = new EmployeePayrollInformation
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                FormDate = Convert.ToDateTime(reader["FormDate"]),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                CreatedBy = reader["CreatedBy"] != DBNull.Value ? Guid.Parse(reader["CreatedBy"].ToString()) : Guid.Empty,
                                EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                PR_CRT_Id = reader["PR_CRT_Id"] != DBNull.Value ? Convert.ToInt32(reader["PR_CRT_Id"]) : 0,
                                PayrollCreationDt = reader["PayrollCreationDt"] != DBNull.Value ? Convert.ToDateTime(reader["PayrollCreationDt"]) : DateTime.MinValue,
                                Year = reader["Year"] != DBNull.Value ? Convert.ToInt32(reader["Year"]) : 0,
                                Month = reader["Month"] != DBNull.Value ? Convert.ToInt32(reader["Month"]) : 0,
                                MS_SLHeads_Id = reader["MS_SLHeads_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_SLHeads_Id"]) : 0,
                                IsEarningComponent = reader["IsEarningComponent"] != DBNull.Value ? Convert.ToBoolean(reader["IsEarningComponent"]) : false,
                                IsShowInSalarySlip = reader["IsShowInSalarySlip"] != DBNull.Value ? Convert.ToBoolean(reader["IsShowInSalarySlip"]) : false,
                                SalaryHeadOrder = reader["SalaryHeadOrder"] != DBNull.Value ? Convert.ToInt32(reader["SalaryHeadOrder"]) : 999,
                                Amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"]) : 0,
                                EmpId = reader["EmpId"] != DBNull.Value ? reader["EmpId"].ToString() : "",
                                BankName = reader["BankName"] != DBNull.Value ? reader["BankName"].ToString() : "",
                                DepartmentName = reader["DepartmentName"] != DBNull.Value ? reader["DepartmentName"].ToString() : "",
                                FullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : "",
                                AccountNo = reader["AccountNo"] != DBNull.Value ? reader["AccountNo"].ToString() : "",
                                PFNo = reader["FullName"] != DBNull.Value ? reader["PFNo"].ToString() : "",
                                SalaryHeadName = reader["SalaryHeadName"] != DBNull.Value ? reader["SalaryHeadName"].ToString() : ""
                            };

                            payrolls.Add(payroll);
                        }
                    }
                }
            }

            return payrolls;
        }

        public static List<EmployeePayrollInformation> GetEmployeeSalaryHeadsDetails(Guid employeeId, int month, int year)
        {
            try
            {
                List<EmployeePayrollInformation> payrolls = new List<EmployeePayrollInformation>();
                string strConString = DbContext.ConnectionString;

                using (var connection = new SqlConnection(strConString))
                {
                    connection.Open();
                    string sql = Payroll.Sql.GetEmployeeSalaryHeadsDetails;
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@EmpId", employeeId);
                        command.Parameters.AddWithValue("@Year", year);
                        command.Parameters.AddWithValue("@Month", month);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmployeePayrollInformation payroll = new EmployeePayrollInformation
                                {
                                   // Id = Guid.Parse(reader["Id"].ToString()),
                                    FormDate = Convert.ToDateTime(reader["FormDate"]),
                                    CompId = Guid.Parse(reader["CompId"].ToString()),
                                    CreatedBy = reader["CreatedBy"] != DBNull.Value ? Guid.Parse(reader["CreatedBy"].ToString()) : Guid.Empty,
                                    EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                  //  PR_CRT_Id = reader["PR_CRT_Id"] != DBNull.Value ? Convert.ToInt32(reader["PR_CRT_Id"]) : 0,
                                 //   PayrollCreationDt = reader["PayrollCreationDt"] != DBNull.Value ? Convert.ToDateTime(reader["PayrollCreationDt"]) : DateTime.MinValue,
                                 //   Year = reader["Year"] != DBNull.Value ? Convert.ToInt32(reader["Year"]) : 0,
                                 //   Month = reader["Month"] != DBNull.Value ? Convert.ToInt32(reader["Month"]) : 0,
                                    MS_SLHeads_Id = reader["MS_SLHeads_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_SLHeads_Id"]) : 0,
                                    IsEarningComponent = reader["IsEarningComponent"] != DBNull.Value ? Convert.ToBoolean(reader["IsEarningComponent"]) : false,
                                    IsShowInSalarySlip = reader["IsShowInSalarySlip"] != DBNull.Value ? Convert.ToBoolean(reader["IsShowInSalarySlip"]) : false,
                                    SalaryHeadOrder = reader["SalaryHeadOrder"] != DBNull.Value ? Convert.ToInt32(reader["SalaryHeadOrder"]) : 999,
                                    Amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"]) : 0,
                                    EmpId = reader["EmpId"] != DBNull.Value ? reader["EmpId"].ToString() : "",
                                    BankName = reader["BankName"] != DBNull.Value ? reader["BankName"].ToString() : "",
                                    DepartmentName = reader["DepartmentName"] != DBNull.Value ? reader["DepartmentName"].ToString() : "",
                                    DesignationName = reader["DesignationName"] != DBNull.Value ? reader["DesignationName"].ToString() : "",
                                    FullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : "",
                                    AccountNo = reader["AccountNo"] != DBNull.Value ? reader["AccountNo"].ToString() : "",
                                    PANNo = reader["PANNo"] != DBNull.Value ? reader["PANNo"].ToString() : "",
                                    UAN = reader["UAN"] != DBNull.Value ? reader["UAN"].ToString() : "",
                                    ESINo = reader["ESINo"] != DBNull.Value ? reader["ESINo"].ToString() : "",
                                    BranchName = reader["BranchName"] != DBNull.Value ? reader["BranchName"].ToString() : "",
                                    IFSC = reader["IFSC"] != DBNull.Value ? reader["IFSC"].ToString() : "",
                                    DateOfJoining = reader["DateOfJoining"] != DBNull.Value ? Convert.ToDateTime(reader["DateOfJoining"].ToString()) : DateTime.MinValue.Date,
                                    PFNo = reader["FullName"] != DBNull.Value ? reader["PFNo"].ToString() : "",
                                    SalaryHeadName = reader["SalaryHeadName"] != DBNull.Value ? reader["SalaryHeadName"].ToString() : ""
                                };

                                payrolls.Add(payroll);
                            }
                        }
                    }
                }

                return payrolls;
            }catch(Exception ex)
            {
                return null;
            }
        }

        public static List<PayrollReimbursement> GetEmployeeReimbursementsByBranchAndMonth(int year, int month, int branchId, Guid compId)
        {
            List<PayrollReimbursement> payrollReimbursements = new List<PayrollReimbursement>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = Payroll.Sql.GetEmployeeReimbursementsByBranchAndMonth;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Year", year);
                    command.Parameters.AddWithValue("@Month", month);
                    command.Parameters.AddWithValue("@BranchId", branchId);
                    command.Parameters.AddWithValue("@CompId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PayrollReimbursement payrollReimbursement = new PayrollReimbursement
                            {
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                Reim_Claims_Id = Guid.Parse(reader["Reim_Claims_Id"].ToString()),
                                EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                EmployeeName = reader["FullName"].ToString(),
                                EMP_Info_MSBranchId = reader["EMP_Info_MSBranchId"] != DBNull.Value ? Convert.ToInt32(reader["EMP_Info_MSBranchId"].ToString()) : 0,
                                ClaimDate = Convert.ToDateTime(reader["ClaimDate"]),
                                CGST = reader["CGST"] != DBNull.Value ? Convert.ToDecimal(reader["CGST"].ToString()) : 0,
                                SGST = reader["SGST"] != DBNull.Value ? Convert.ToDecimal(reader["SGST"].ToString()) : 0,
                                Amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"].ToString()) : 0,
                                ReimbursementType = reader["ReimbursementType"] != DBNull.Value ? reader["ReimbursementType"].ToString() : "",
                                ApprovedDate = Convert.ToDateTime(reader["ApprovedDate"].ToString()),
                                Approver_Emp_Id = Guid.Parse(reader["Approver_Emp_Id"].ToString()),
                                Approver = reader["Approver"] != DBNull.Value ? reader["Approver"].ToString() : "",
                                PayrollStatus = reader["PayrollStatus"] != DBNull.Value ? reader["PayrollStatus"].ToString() : "NotProcessed",
                                Comment = reader["Comment"] != DBNull.Value ? reader["Comment"].ToString() : ""                                
                                
                            };

                            payrollReimbursements.Add(payrollReimbursement);
                        }
                    }
                }
            }

            return payrollReimbursements;
        }

        public static List<SalaryAdjustment> GetSalaryAdjustmentEmployeesByCompId(Guid compId)
        {
            List<SalaryAdjustment> salaryAdjustments = new List<SalaryAdjustment>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = Payroll.Sql.GetSalaryAdjustmentEmployeesByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);
                   
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SalaryAdjustment salaryAdjustment = new SalaryAdjustment
                            {
                                PayrollMonthYear = Convert.ToDateTime(reader["PayrollMonthYear"]),
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty,
                                Amount = reader["TotalAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TotalAmount"].ToString()) : 0,
                                MS_Payroll_AdjstType_Id = reader["MS_Payroll_AdjstType_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Payroll_AdjstType_Id"].ToString()) : 0,
                                AdjustmentType = reader["AdjustmentType"] != DBNull.Value ? reader["AdjustmentType"].ToString() : string.Empty,
                                IsEarningHead = Convert.ToBoolean(reader["IsEarningHead"].ToString()),
                                Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : string.Empty
                            };

                            salaryAdjustments.Add(salaryAdjustment);
                        }
                    }
                }
            }

            return salaryAdjustments;
        }

        public static List<SalaryAdjustment> GetSalaryAdjustmentsByPayrollMonthYear(DateTime payrollDate)
        {
            try
            {
                List<SalaryAdjustment> salaryAdjustments = new List<SalaryAdjustment>();
                string strConString = DbContext.ConnectionString;

                using (var connection = new SqlConnection(strConString))
                {
                    connection.Open();
                    string sql = Payroll.Sql.GetSalaryAdjustmentsByPayrollMonthYear;
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@date", payrollDate);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SalaryAdjustment salaryAdjustment = new SalaryAdjustment
                                {
                                    Id = Guid.Parse(reader["Id"].ToString()),
                                    FormDate = Convert.ToDateTime(reader["FormDate"].ToString()),
                                    CompId = Guid.Parse(reader["CompId"].ToString()),
                                    EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                    FullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : string.Empty,
                                    PayrollMonthYear = Convert.ToDateTime(reader["PayrollMonthYear"].ToString()),
                                    Amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"].ToString()) : 0,
                                    MS_Payroll_AdjstType_Id = reader["MS_Payroll_AdjstType_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Payroll_AdjstType_Id"].ToString()) : 0,
                                    AdjustmentType = reader["AdjustmentType"] != DBNull.Value ? reader["AdjustmentType"].ToString() : string.Empty,
                                    IsRuleBased = Convert.ToBoolean(reader["IsRuleBased"].ToString()),
                                    Rule = reader["Rule"] != DBNull.Value ? reader["Rule"].ToString() : string.Empty,
                                    Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty,
                                    Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : string.Empty
                                };

                                salaryAdjustments.Add(salaryAdjustment);
                            }
                        }
                    }
                }

                return salaryAdjustments;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public static List<SalaryAdjustment> GetCalcSalaryAdjustmentsEmployeesByAdjustmentId(List<Guid> employeeIds, int adjustmentId)
        {
            string empIdsCsv = string.Join(",", employeeIds); 

            List<SalaryAdjustment> salaryAdjustments = new List<SalaryAdjustment>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = Payroll.Sql.GetCalculatedSalaryAdjustmentByAdjustmentIdByEmployees;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EmpIds", empIdsCsv);
                    command.Parameters.AddWithValue("@AdjustmentTypeId", adjustmentId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SalaryAdjustment salaryAdjustment = new SalaryAdjustment
                            {
                                EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                FullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : string.Empty,
                                AdjustmentType = reader["AdjustmentType"] != DBNull.Value ? reader["AdjustmentType"].ToString() : string.Empty,
                                IsRuleBased = reader["IsRuleBased"] != DBNull.Value ? Convert.ToBoolean(reader["IsRuleBased"].ToString()) : false,
                                Amount = reader["PercentageAmt"] != DBNull.Value ? Convert.ToDecimal(reader["PercentageAmt"].ToString()) : 0,
                                SalaryHeadName = reader["SalaryHeadName"] != DBNull.Value ? reader["SalaryHeadName"].ToString() : string.Empty,
                                SalaryHeadAmount = reader["SalaryHeadAmount"] != DBNull.Value ? Convert.ToDecimal(reader["SalaryHeadAmount"].ToString()) : 0,
                                CalculatedAdjustmentAmount = reader["CalculatedAdjustmentAmount"] != DBNull.Value ? Convert.ToDecimal(reader["CalculatedAdjustmentAmount"].ToString()) : 0,
                                Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : string.Empty,
                            };

                            salaryAdjustments.Add(salaryAdjustment);
                        }
                    }
                }
            }

            return salaryAdjustments;
        }

        public static int UpsertPayrollCreation(List<PayrollInformation> payrolls)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(payrolls);

            string sql = Payroll.Sql.UpsertPayrollCreation;
            string strConString = DbContext.ConnectionString;

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = sql;

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@json1", jsonString);

                return cmd.ExecuteNonQuery();
            }
        }

        public static bool UpsertSalaryAdjustment(List<SalaryAdjustment> salaryAdjustments)
        {
            DataTable dtAdjustments = new DataTable();
            dtAdjustments.Columns.Add("Id", typeof(Guid));
            dtAdjustments.Columns.Add("CompId", typeof(Guid));
            dtAdjustments.Columns.Add("CreatedBy", typeof(Guid));
            dtAdjustments.Columns.Add("EMP_Info_Id", typeof(Guid));
            dtAdjustments.Columns.Add("PayrollMonthYear", typeof(DateTime));
            dtAdjustments.Columns.Add("Amount", typeof(decimal));
            dtAdjustments.Columns.Add("MS_Payroll_AdjstType_Id", typeof(int));
            dtAdjustments.Columns.Add("Description", typeof(string));
            dtAdjustments.Columns.Add("Status", typeof(string));

            foreach(SalaryAdjustment salaryAdjustment in salaryAdjustments)
            {
                dtAdjustments.Rows.Add(salaryAdjustment.Id, salaryAdjustment.CompId, salaryAdjustment.CreatedBy, salaryAdjustment.EMP_Info_Id,
                        salaryAdjustment.PayrollMonthYear, salaryAdjustment.Amount, salaryAdjustment.MS_Payroll_AdjstType_Id, salaryAdjustment.Description, salaryAdjustment.Status);
            }

            string sql = Payroll.Sql.UpsertEmpSalaryAdjustment;
            string strConString = DbContext.ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(strConString))
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text; // Not a stored procedure

                    SqlParameter tvpParam = cmd.Parameters.AddWithValue("@Adjustments", dtAdjustments);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "dbo.SalaryAdjustmentType";

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                return true;

            }
            catch (Exception ex)
            {
                // Optionally log ex.Message or ex.ToString()
                return false; // failed
            }
        }

        public static int UpsertPayrollReimbursements(List<PayrollReimbursement> payrollReimbursements)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(payrollReimbursements);

            string sql = Payroll.Sql.UpsertPayrollReimbursements;
            string strConString = DbContext.ConnectionString;

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = sql;

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@json1", jsonString);

                return cmd.ExecuteNonQuery();
            }
        }
    }
}
