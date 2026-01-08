using ServerModel.Data;
using ServerModel.Model.Base;
using ServerModel.Model.Employee;
using ServerModel.Model.ESS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.ESS
{
    public class EssSetupAccess
    {
        public static List<EmployeeLeaveRequestInformation> GetEmployeeLeaveRequestsByCompId(Guid compId)
        {
            List<EmployeeLeaveRequestInformation> employeeLeaves = new List<EmployeeLeaveRequestInformation>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = EssSql.GetEmployeeLeaveReqsByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var empLeave = new EmployeeLeaveRequestInformation
                            {
                                Id = Guid.Parse(reader["Id"].ToString()),
                                FormDate = Convert.ToDateTime(reader["FormDate"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                CreatedBy = reader["CreatedBy"] != DBNull.Value ? Guid.Parse(reader["CreatedBy"].ToString()) : Guid.Empty,
                                EMP_Info_Id = reader["EMP_Info_Id"] != DBNull.Value ? Guid.Parse(reader["EMP_Info_Id"].ToString()) : Guid.Empty,
                                EmployeeName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : "",
                                MS_Leave_Id = reader["MS_Leave_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Leave_Id"].ToString()) : 0,
                                LeaveName = reader["LeaveName"] != DBNull.Value ? reader["LeaveName"].ToString() : "",
                                FromDate = reader["FromDate"] != DBNull.Value ? Convert.ToDateTime(reader["FromDate"].ToString()) : DateTime.MinValue,
                                ToDate = reader["ToDate"] != DBNull.Value ? Convert.ToDateTime(reader["ToDate"].ToString()) : DateTime.MinValue,
                                TotalDays = reader["TotalDays"] != DBNull.Value ? Convert.ToDecimal(reader["TotalDays"].ToString()) : 0,
                                LeaveFor = reader["LeaveFor"] != DBNull.Value ? Convert.ToInt32(reader["LeaveFor"].ToString()) : 0,
                                LeaveReason = reader["LeaveReason"] != DBNull.Value ? reader["LeaveReason"].ToString() : string.Empty,
                                IsApprovedBy = reader["IsApprovedBy"] != DBNull.Value ? Guid.Parse(reader["IsApprovedBy"].ToString()) : Guid.Empty,
                                ApproverName = reader["ApproverName"] != DBNull.Value ? reader["ApproverName"].ToString() : "",
                                LeaveStatus = reader["LeaveStatus"] != DBNull.Value ? Convert.ToInt32(reader["LeaveStatus"].ToString()) : 0,
                                Docs = reader["Docs"] != DBNull.Value ? reader["LeaveStatus"].ToString() : string.Empty,

                            };

                            employeeLeaves.Add(empLeave);
                        }
                    }
                }
            }

            return employeeLeaves;
        }

        public static EmployeeLeaveRequestInformation GetEmployeeLeaveRequestById(Guid requestId)
        {
            EmployeeLeaveRequestInformation empLeaveRequest = null;
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = EssSql.GetEmployeeLeaveReqById;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", requestId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            empLeaveRequest = new EmployeeLeaveRequestInformation
                            {
                                Id = Guid.Parse(reader["Id"].ToString()),
                                FormDate = Convert.ToDateTime(reader["FormDate"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                CreatedBy = reader["CreatedBy"] != DBNull.Value ? Guid.Parse(reader["CreatedBy"].ToString()) : Guid.Empty,
                                EMP_Info_Id = reader["EMP_Info_Id"] != DBNull.Value ? Guid.Parse(reader["EMP_Info_Id"].ToString()) : Guid.Empty,
                                EmployeeName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : "",
                                MS_Leave_Id = reader["MS_Leave_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Leave_Id"].ToString()) : 0,
                                LeaveName = reader["LeaveName"] != DBNull.Value ? reader["LeaveName"].ToString() : "",
                                FromDate = reader["FromDate"] != DBNull.Value ? Convert.ToDateTime(reader["FromDate"].ToString()) : DateTime.MinValue,
                                ToDate = reader["ToDate"] != DBNull.Value ? Convert.ToDateTime(reader["ToDate"].ToString()) : DateTime.MinValue,
                                TotalDays = reader["TotalDays"] != DBNull.Value ? Convert.ToDecimal(reader["TotalDays"].ToString()) : 0,
                                LeaveFor = reader["LeaveFor"] != DBNull.Value ? Convert.ToInt32(reader["LeaveFor"].ToString()) : 0,
                                LeaveReason = reader["LeaveReason"] != DBNull.Value ? reader["LeaveReason"].ToString() : string.Empty,
                                IsApprovedBy = reader["IsApprovedBy"] != DBNull.Value ? Guid.Parse(reader["IsApprovedBy"].ToString()) : Guid.Empty,
                                ApproverName = reader["ApproverName"] != DBNull.Value ? reader["ApproverName"].ToString() : "",
                                LeaveStatus = reader["LeaveStatus"] != DBNull.Value ? Convert.ToInt32(reader["LeaveStatus"].ToString()) : 0,
                                Docs = reader["Docs"] != DBNull.Value ? reader["LeaveStatus"].ToString() : string.Empty,

                            };
                        }
                    }
                }
            }

            return empLeaveRequest;
        }

        public static List<EmployeeLeaves> GetEmployeeAvailableLeavesByEmpId(Guid empId)
        {
            List<EmployeeLeaves> employeeLeaves = new List<EmployeeLeaves>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = EssSql.GetEmployeeAvailableLeavesById;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@empId", empId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EmployeeLeaves employeeLeave = new EmployeeLeaves
                            {
                                Id = Guid.Parse(reader["Id"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                EMP_Info_Id = reader["EMP_Info_Id"] != DBNull.Value ? Guid.Parse(reader["EMP_Info_Id"].ToString()) : Guid.Empty,
                                EmployeeName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : "",
                                MS_Leave_Id = reader["MS_Leave_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Leave_Id"].ToString()) : 0,
                                LeaveName = reader["LeaveName"] != DBNull.Value ? reader["LeaveName"].ToString() : "",
                                TotalLeaves = reader["TotalLeaves"] != DBNull.Value ? Convert.ToDecimal(reader["TotalLeaves"].ToString()) : 0,
                                AvailableLeaves = reader["AvailableLeaves"] != DBNull.Value ? Convert.ToDecimal(reader["AvailableLeaves"].ToString()) : 0
                            };

                            employeeLeaves.Add(employeeLeave);
                        }
                    }
                }
            }

            return employeeLeaves;
        }

        public static bool AddUpdateEmployeeLeaves(EmployeeLeaves employeeLeave)
        {
            try
            {
                string sql = EssSql.AddUpdateEmployeeLeaves;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    // Insert Emp Leaves
                    cmd.Parameters.AddWithValue("@employeeId", employeeLeave.EmployeeId);
                    cmd.Parameters.AddWithValue("@compId", employeeLeave.CompId);

                    // Update Emp Leave
                    cmd.Parameters.AddWithValue("@id", employeeLeave.Id);
                    cmd.Parameters.AddWithValue("@createdBy", employeeLeave.CreatedBy);
                    cmd.Parameters.AddWithValue("@MS_Leave_Id", employeeLeave.MS_Leave_Id);
                    cmd.Parameters.AddWithValue("@TotalLeaves", employeeLeave.TotalLeaves);
                    cmd.Parameters.AddWithValue("@AvailableLeves", employeeLeave.AvailableLeaves);

                    // cmd.ExecuteNonQuery();
                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        //Guid id = Guid.Parse(returnObj.ToString());
                        return true;
                    }

                    return false;

                    // var ddd = employeeBankInformation.Id;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool UpdateEmployeeLeaveRequestStatusByIdAndStatus(Guid approverEmpId, Guid requestId, LeaveStatusType leaveStatusType)
        {
            try
            {
                string sql = EssSql.UpdateEmployeeLeaveRequestStatusByIdAndStatus;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@approverId", approverEmpId);
                    cmd.Parameters.AddWithValue("@id", requestId);
                    cmd.Parameters.AddWithValue("@leaveStatus", (int)leaveStatusType);

                    // cmd.ExecuteNonQuery();
                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        //Guid id = Guid.Parse(returnObj.ToString());
                        return true;
                    }

                    return false;

                    // var ddd = employeeBankInformation.Id;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static List<EmployeeLeaveRequestInformation> GetEmployeeLeaveReqByLeaveStatusAndCompId(Guid compId, LeaveStatusType leaveStatus)
        {
            List<EmployeeLeaveRequestInformation> employeeLeaves = new List<EmployeeLeaveRequestInformation>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = EssSql.GetEmployeeLeaveReqByLeaveStatusAndCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);
                    command.Parameters.AddWithValue("@leaveStatus", (int)leaveStatus);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EmployeeLeaveRequestInformation employeeLeave = new EmployeeLeaveRequestInformation
                            {
                                Id = Guid.Parse(reader["Id"].ToString()),
                                EMP_Info_Id = reader["EMP_Info_Id"] != DBNull.Value ? Guid.Parse(reader["EMP_Info_Id"].ToString()) : Guid.Empty,
                                EmployeeName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : "",
                                MS_Leave_Id = reader["MS_Leave_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Leave_Id"].ToString()) : 0,
                                LeaveName = reader["LeaveName"] != DBNull.Value ? reader["LeaveName"].ToString() : "",
                                FromDate = reader["FromDate"] != DBNull.Value ? Convert.ToDateTime(reader["FromDate"].ToString()) : DateTime.MinValue.Date,
                                ToDate = reader["ToDate"] != DBNull.Value ? Convert.ToDateTime(reader["ToDate"].ToString()) : DateTime.MinValue.Date,
                                TotalDays = reader["Applied Days"] != DBNull.Value ? Convert.ToDecimal(reader["Applied Days"].ToString()) : 0,
                                AvailableLeaves = reader["AvailableLeaves"] != DBNull.Value ? Convert.ToDecimal(reader["AvailableLeaves"].ToString()) : 0,
                                EMP_Leaves_Id = reader["EMP_Leaves_Id"] != DBNull.Value ? Guid.Parse(reader["EMP_Leaves_Id"].ToString()) : Guid.Empty,
                                IsApprovedBy = reader["IsApprovedBy"] != DBNull.Value ? Guid.Parse(reader["IsApprovedBy"].ToString()) : Guid.Empty,
                                Approver = reader["Approver"] != DBNull.Value ? reader["Approver"].ToString() : "HR Dept",
                                LeaveStatus = reader["LeaveStatus"] != DBNull.Value ? Convert.ToInt32(reader["LeaveStatus"].ToString()) : 0
                            };
                            employeeLeaves.Add(employeeLeave);
                        }
                    }
                }
            }
            return employeeLeaves;
        }

        #region Reimbursement

        public static List<ReimbursementClaims> GetReimbursementClaimsByCompId(Guid compId)
        {
            List<ReimbursementClaims> reimbursementClaims = new List<ReimbursementClaims>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = EssSql.GetReimbursementClaimsByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ReimbursementClaims reimbursementClaim = GetReimbursementClaimsObject(reader);
                            reimbursementClaims.Add(reimbursementClaim);
                        }
                    } 
                }
            }

            return reimbursementClaims;
        }

        public static List<ReimbursementClaims> GetReimbursementClaimsByEmployeeId(Guid employeeId)
        {
            List<ReimbursementClaims> reimbursementClaims = new List<ReimbursementClaims>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = EssSql.GetReimbursementClaimsByEmpId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EmpId", employeeId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ReimbursementClaims reimbursementClaim = GetReimbursementClaimsObject(reader);
                            reimbursementClaims.Add(reimbursementClaim);
                        }
                    }
                }
            }

            return reimbursementClaims;
        }

        public static List<ReimbursementClaims> GetReimbursementClaimsByCompIdAndBranchId(Guid compId, int branchId)
        {
            List<ReimbursementClaims> reimbursementClaims = new List<ReimbursementClaims>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = EssSql.GetReimbursementClaimsByCompIdAndBranchId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", compId);
                    command.Parameters.AddWithValue("@BranchId", branchId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ReimbursementClaims reimbursementClaim = GetReimbursementClaimsObject(reader);
                            reimbursementClaims.Add(reimbursementClaim);
                        }
                    }
                }
            }

            return reimbursementClaims;
        }

        public static Guid UpsertReimbursementClaims(ReimbursementClaims reimbursementClaim)
        {
            try
            {
                string sql = EssSql.UpsertReimbursementClaims;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", reimbursementClaim.Id);
                    cmd.Parameters.AddWithValue("@CompId", reimbursementClaim.CompId);
                    cmd.Parameters.AddWithValue("@EMP_Info_Id", reimbursementClaim.EMP_Info_Id);
                    cmd.Parameters.AddWithValue("@MS_Reim_Types_Id", reimbursementClaim.MS_Reim_Types_Id);
                    cmd.Parameters.AddWithValue("@ClaimDate", reimbursementClaim.ClaimDate);
                    cmd.Parameters.AddWithValue("@GSTRegNos", (object)reimbursementClaim.GSTRegNos ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CGST", (object)reimbursementClaim.CGST ?? 0);
                    cmd.Parameters.AddWithValue("@SGST", (object)reimbursementClaim.SGST ?? 0);
                    cmd.Parameters.AddWithValue("@Amount", reimbursementClaim.Amount);
                    cmd.Parameters.AddWithValue("@Description", (object)reimbursementClaim.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@BillPath", (object)reimbursementClaim.BillPath ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", reimbursementClaim.Status);
                    cmd.Parameters.AddWithValue("@SubmittedDate", reimbursementClaim.SubmittedDate);
                    cmd.Parameters.AddWithValue("@ApprovedDate", (object)reimbursementClaim.ApprovedDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PaidDate", (object)reimbursementClaim.PaidDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", reimbursementClaim.Active != null ? reimbursementClaim.Active : true);

                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        Guid id = Guid.Parse(returnObj.ToString());
                        return id;
                    }

                    return Guid.Empty;
                }
            }
            catch (Exception ex)
            {
                var abc = ex;
                return Guid.Empty;
            }
        }

        public static Guid ApprovedReimbursementClaim(ReimbursementApproverModel reimbursementApproverModel)
        {
            try
            {
                string sql = EssSql.ApproveReimbursementClaims;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", reimbursementApproverModel.Id);
                    cmd.Parameters.AddWithValue("@CompId", reimbursementApproverModel.CompId);
                    cmd.Parameters.AddWithValue("@MachineId", (object)reimbursementApproverModel.MachineId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MachineIp", (object)reimbursementApproverModel.MachineIp ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Reim_Claims_Id", reimbursementApproverModel.Reim_Claims_Id);
                    cmd.Parameters.AddWithValue("@Approver_Emp_Id", reimbursementApproverModel.Approver_Emp_Id);
                    cmd.Parameters.AddWithValue("@Status", reimbursementApproverModel.Status);
                    cmd.Parameters.AddWithValue("@Comment", (object)reimbursementApproverModel.Comment ?? DBNull.Value);

                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        Guid id = Guid.Parse(returnObj.ToString());
                        return id;
                    }

                    return Guid.Empty;
                }
            }
            catch (Exception ex)
            {
                var abc = ex;
                return Guid.Empty;
            }
        }

        private static ReimbursementClaims GetReimbursementClaimsObject(SqlDataReader reader)
        {
            return new ReimbursementClaims
            {
                Id = Guid.Parse(reader["Id"].ToString()),
                CompId = Guid.Parse(reader["CompId"].ToString()),
                EMP_Info_Id = reader["EMP_Info_Id"] != DBNull.Value ? Guid.Parse(reader["EMP_Info_Id"].ToString()) : Guid.Empty,
                FullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : string.Empty,
                MS_Branch_Id = reader["MS_Branch_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Branch_Id"].ToString()) : 0,
                BranchName = reader["BranchName"] != DBNull.Value ? reader["BranchName"].ToString() : string.Empty,
                MS_Reim_Types_Id = reader["MS_Reim_Types_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_Reim_Types_Id"].ToString()) : 0,
                ReimbursementType = reader["ReimbursementType"] != DBNull.Value ? reader["ReimbursementType"].ToString() : string.Empty,
                ClaimDate = reader["ClaimDate"] != DBNull.Value ? Convert.ToDateTime(reader["ClaimDate"].ToString()) : DateTime.MinValue,
                GSTRegNos = reader["GSTRegNos"] != DBNull.Value ? reader["GSTRegNos"].ToString() : string.Empty,
                CGST = reader["CGST"] != DBNull.Value ? Convert.ToDecimal(reader["CGST"].ToString()) : 0,
                SGST = reader["SGST"] != DBNull.Value ? Convert.ToDecimal(reader["SGST"].ToString()) : 0,
                Amount = reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"].ToString()) : 0,
                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty,
                BillPath = reader["BillPath"] != DBNull.Value ? reader["BillPath"].ToString() : string.Empty,
                Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : string.Empty,
                SubmittedDate = reader["SubmittedDate"] != DBNull.Value ? Convert.ToDateTime(reader["SubmittedDate"].ToString()) : DateTime.MinValue,
                ApprovedDate = reader["ApprovedDate"] != DBNull.Value  ? Convert.ToDateTime(reader["ApprovedDate"]) : (DateTime?)null,
                PaidDate = reader["PaidDate"] != DBNull.Value ? Convert.ToDateTime(reader["PaidDate"]) : (DateTime?)null
            };
        }
        #endregion


        #region Loan

        public static Guid UpsertEmployeeLoanRequest(EmpLoanRequest empLoanRequest)
        {
            try
            {
                string sql = EssSql.UpsertEmployeeLoanRequest;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", empLoanRequest.Id);
                    cmd.Parameters.AddWithValue("@CompId", empLoanRequest.CompId);
                    cmd.Parameters.AddWithValue("@MachineId", (object)empLoanRequest.MachineId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@MachineIp", (object)empLoanRequest.MachineIp ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EMP_Info_Id", empLoanRequest.EMP_Info_Id);
                    cmd.Parameters.AddWithValue("@MS_LN_ID", empLoanRequest.MS_LN_Id);
                    cmd.Parameters.AddWithValue("@ReqAmt", empLoanRequest.ReqAmt);
                    cmd.Parameters.AddWithValue("@ApprAmt", empLoanRequest.ApprAmt != null ? empLoanRequest.ApprAmt : 0);
                    cmd.Parameters.AddWithValue("@TenureMonths", (object)empLoanRequest.TenureMonths ?? 0);
                    cmd.Parameters.AddWithValue("@Status", (object)empLoanRequest.Status ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Remark", empLoanRequest.Remark);
                    cmd.Parameters.AddWithValue("@Active", empLoanRequest.Active != null ? empLoanRequest.Active : true);

                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        Guid id = Guid.Parse(returnObj.ToString());
                        return id;
                    }

                    return Guid.Empty;
                }
            }
            catch (Exception ex)
            {
                var abc = ex;
                return Guid.Empty;
            }
        }

        public static List<EmpLoanRequest> GetEmployeeLoanRequestsByCompId(Guid compId)
        {
            List<EmpLoanRequest> empLoanRequests = new List<EmpLoanRequest>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = EssSql.GetEmployeeLoanRequestByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EmpLoanRequest empLoanRequest = GetEmpLoanRequest(reader);
                            empLoanRequests.Add(empLoanRequest);
                        }
                    }
                }
            }

            return empLoanRequests;
        }

        private static EmpLoanRequest GetEmpLoanRequest(SqlDataReader reader)
        {
            return new EmpLoanRequest
            {
                Id = Guid.Parse(reader["Id"].ToString()),
                FormDate = Convert.ToDateTime(reader["FormDate"].ToString()),
                CompId = Guid.Parse(reader["CompId"].ToString()),
                EMP_Info_Id = reader["EMP_Info_Id"] != DBNull.Value ? Guid.Parse(reader["EMP_Info_Id"].ToString()) : Guid.Empty,
                FullName = reader["FullName"] != DBNull.Value ? reader["FullName"].ToString() : string.Empty,
                MS_LN_Id = reader["MS_LN_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_LN_Id"].ToString()) : 0,
                LNTypeName = reader["LNTypeName"] != DBNull.Value ? reader["LNTypeName"].ToString() : string.Empty,
                InterestRate = reader["InterestRate"] != DBNull.Value ? Convert.ToDecimal(reader["InterestRate"].ToString()) : 0,
                ReqAmt = reader["ReqAmt"] != DBNull.Value ? Convert.ToDecimal(reader["ReqAmt"].ToString()) : 0,
                ApprAmt = reader["ApprAmt"] != DBNull.Value ? Convert.ToDecimal(reader["ApprAmt"].ToString()) : 0,
                TenureMonths = reader["TenureMonths"] != DBNull.Value ? Convert.ToInt32(reader["TenureMonths"].ToString()) : 0,
                Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : string.Empty,
                Remark = reader["Remark"] != DBNull.Value ? reader["Remark"].ToString() : string.Empty,
            };
        }

        #endregion
    }
}
