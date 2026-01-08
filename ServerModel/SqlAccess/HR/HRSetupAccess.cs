using ServerModel.Data;
using ServerModel.Model.Base;
using ServerModel.Model.HR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ServerModel.SqlAccess.HR
{
    public class HRSetupAccess
    {
        public static Guid UpsertHRLoanRequest(HRLoanRequest hrLoanRequest)
        {
            try
            {
                string sql = HRSqls.UpsertHRLoanRequest;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", hrLoanRequest.Id);
                    cmd.Parameters.AddWithValue("@CompId", hrLoanRequest.CompId);
                    cmd.Parameters.AddWithValue("@EMP_LNReq_Id", hrLoanRequest.EMP_LNReq_Id);
                    cmd.Parameters.AddWithValue("@ApproverId", hrLoanRequest.ApproverId);
                    cmd.Parameters.AddWithValue("@Status", hrLoanRequest.Status);
                    cmd.Parameters.AddWithValue("@Remark", hrLoanRequest.Remark);

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

        public static List<HRLoanRequest> GetHRLoanRequestsByStatus(Guid compId, LoanStatusTypes loanStatus)
        {
            List<HRLoanRequest> hRLoanRequests = new List<HRLoanRequest>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = HRSqls.GetHRLoanRequestsByStatus;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);
                    command.Parameters.AddWithValue("@status", loanStatus.ToString());

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            HRLoanRequest loanRequest = new HRLoanRequest
                            {
                                Id = reader["Id"] != DBNull.Value ? Guid.Parse(reader["Id"].ToString()) : Guid.Empty,
                                FormDate = reader["FormDate"] != DBNull.Value ? Convert.ToDateTime(reader["FormDate"].ToString()) : DateTime.Now,
                                CompId = reader["CompId"] != DBNull.Value ? Guid.Parse(reader["CompId"].ToString()) : compId,
                                EMP_LNReq_Id = reader["EMP_LNReq_Id"] != DBNull.Value ? Guid.Parse(reader["EMP_LNReq_Id"].ToString()) : Guid.Empty,
                                TenureMonths = reader["TenureMonths"] != DBNull.Value ? Convert.ToDecimal(reader["TenureMonths"].ToString()) : 0,
                                LNTypeName = reader["LNTypeName"] != DBNull.Value ? reader["LNTypeName"].ToString() : string.Empty,
                                InterestRate = reader["InterestRate"] != DBNull.Value ? Convert.ToDecimal(reader["InterestRate"].ToString()) : 0,
                                MaxAmount = reader["MaxAmount"] != DBNull.Value ? Convert.ToDecimal(reader["MaxAmount"].ToString()) : 0,
                                IsMaxAmtManual = reader["IsMaxAmtManual"] != DBNull.Value ? Convert.ToBoolean(reader["IsMaxAmtManual"].ToString()) : false,
                                SalaryHeadName = reader["SalaryHeadName"] != DBNull.Value ? reader["SalaryHeadName"].ToString() : string.Empty,
                                MS_SLHead_Id = reader["MS_SLHead_Id"] != DBNull.Value ? Convert.ToInt32(reader["MS_SLHead_Id"].ToString()) : 0,
                                Percentage = reader["Percentage"] != DBNull.Value ? Convert.ToDecimal(reader["Percentage"].ToString()) : 0,
                                ReqAmt = reader["ReqAmt"] != DBNull.Value ? Convert.ToDecimal(reader["ReqAmt"].ToString()) : 0,
                                LoanApplicantStatus = reader["LoanApplicantStatus"] != DBNull.Value ? reader["LoanApplicantStatus"].ToString() : LoanStatusTypes.Pending.ToString(),
                                LoanApplicantRemark = reader["LoanApplicantRemark"] != DBNull.Value ? reader["LoanApplicantRemark"].ToString() : string.Empty,
                                LoanApplicant = reader["LoanApplicant"] != DBNull.Value ? reader["LoanApplicant"].ToString() : string.Empty,
                                LoanApplicantId = reader["LoanApplicantId"] != DBNull.Value ? Guid.Parse(reader["LoanApplicantId"].ToString()) : Guid.Empty,
                                ApproverId = reader["ApproverId"] != DBNull.Value ? Guid.Parse(reader["ApproverId"].ToString()) : Guid.Empty,
                                Approver = reader["Approver"] != DBNull.Value ? reader["Approver"].ToString() : string.Empty,
                                Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : string.Empty,
                                Remark = reader["Remark"] != DBNull.Value ? reader["Remark"].ToString() : string.Empty,
                            };

                            hRLoanRequests.Add(loanRequest);
                        }
                    }
                }
            }

            return hRLoanRequests;
        }

        public static List<LoanRepaymentScheduleInfo> GetRePaymentScheduleByAmtIntesterAndTenure(decimal loanAmount, decimal interest, decimal tenure)
        {
            List<LoanRepaymentScheduleInfo> loanRepayments = new List<LoanRepaymentScheduleInfo>();
            string strConString = DbContext.ConnectionString;

            DateTime today = DateTime.Now.Date;
            DateTime startDate = new DateTime(today.Year, today.Month, 1).AddMonths(1);

            using (SqlConnection conn = new SqlConnection(strConString))
            {
                conn.Open();

                string sql = @"SELECT * FROM dbo.fn_GetLoanSchedule(@StartDate, @TenureMonths, @LoanAmount, @InterestRate)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@TenureMonths", tenure);
                    cmd.Parameters.AddWithValue("@LoanAmount", loanAmount);
                    cmd.Parameters.AddWithValue("@InterestRate", interest);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var repayment = new LoanRepaymentScheduleInfo()
                            {
                                InstallmentNo = Convert.ToDecimal(reader["InstallmentNo"].ToString()),
                                DueDate = Convert.ToDateTime(reader["DueDate"].ToString()),
                                PrincipalAmt = Convert.ToDecimal(reader["PrincipalAmt"].ToString()),
                                InterestAmt = Convert.ToDecimal(reader["InterestAmt"].ToString()),
                                EMIAmt = Convert.ToDecimal(reader["EMIAmt"].ToString())
                            };
                            loanRepayments.Add(repayment);
                        }
                    }
                }
            }

            return loanRepayments;
        }

        public static LoanValidationInfo ValidateLoanRequestByEmpId(Guid empId, decimal loanAmount, int tenure, decimal interest)
        {

            LoanValidationInfo loanValidationInfo = null;
            string strConString = DbContext.ConnectionString;

            using (SqlConnection conn = new SqlConnection(strConString))
            {
                conn.Open();

                string sql = @"SELECT * FROM dbo.fn_ValidateLoanEligibility(@EmpId, @RequestedLoanAmount, @RequestedTenure, @AnnualInterestRate)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EmpId", empId);
                    cmd.Parameters.AddWithValue("@RequestedLoanAmount", loanAmount);
                    cmd.Parameters.AddWithValue("@RequestedTenure", tenure);
                    cmd.Parameters.AddWithValue("@AnnualInterestRate", interest);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            loanValidationInfo = new LoanValidationInfo()
                            {
                                IsEligible = Convert.ToBoolean(reader["IsEligible"].ToString()),
                                Reason = reader["Reason"].ToString(),
                                NetSalary = Convert.ToDecimal(reader["NetSalary"].ToString()),
                                TotalOutstanding = Convert.ToDecimal(reader["TotalOutstanding"].ToString()),
                                CurrentEMI = Convert.ToDecimal(reader["CurrentEMI"].ToString()),
                                MaxAllowedEMI = Convert.ToDecimal(reader["MaxAllowedEMI"].ToString()),
                                CalculatedEMI = Convert.ToDecimal(reader["CalculatedEMI"].ToString())
                            };
                        }
                    }
                }
            }
            return loanValidationInfo;
        }

        public static bool UpsertLoanDisbursementAndScheduleRepayment(LoanDisbursementInfo loanDisbursement)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(loanDisbursement.repaymentScheduleInfos);

            string strConString = DbContext.ConnectionString;

            using (SqlConnection conn = new SqlConnection(strConString))
            using (SqlCommand cmd = new SqlCommand("sp_CreateLoanDisbursement", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompId", loanDisbursement.CompId);
                cmd.Parameters.AddWithValue("@EmpId", loanDisbursement.EMP_Info_Id);
                cmd.Parameters.AddWithValue("@EMP_LNReq_Id", loanDisbursement.EMP_LNReq_Id);
                cmd.Parameters.AddWithValue("@Amount", loanDisbursement.Amount);
                cmd.Parameters.AddWithValue("@Tenure", loanDisbursement.Tenure);
                cmd.Parameters.AddWithValue("@ReferenceNo", loanDisbursement.ReferenceNo);
                cmd.Parameters.AddWithValue("@JsonRepaySchedule", jsonString);

                conn.Open();
                var resultObj = cmd.ExecuteScalar();

                if (resultObj != null && resultObj != DBNull.Value)
                {
                    bool result = Convert.ToBoolean(resultObj);
                    return result;
                }
                else
                {
                    return false; // or throw an exception depending on your logic
                }
            }
        }
    }
}
