using Microsoft.SqlServer.Server;
using ServerModel.Data;
using ServerModel.Entity.PMS;
using ServerModel.Model.Base;
using ServerModel.Model.Employee;
using ServerModel.Model.PMS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Xml.Linq;

namespace ServerModel.SqlAccess.PMS
{
    public class PMSAccess
    {
        public static List<PMSEmpGoals> GetEmployeeGoalsByEmpId(Guid empId, int cycleId)
        {
            List<PMSEmpGoals> empGoals = new List<PMSEmpGoals>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PMSSqls.GetEmployeeGoalsByEmpId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EmpId", empId);
                    command.Parameters.AddWithValue("@cycleId", cycleId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var empGoal = new PMSEmpGoals
                            {
                                GoalId = Convert.ToInt32(reader["GoalId"].ToString()),
                                FormDate = Convert.ToDateTime(reader["FormDate"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                EmployeeName = reader["FullName"].ToString(),
                                MS_PerformanceCycles_Id = Convert.ToInt32(reader["MS_PerformanceCycles_Id"].ToString()),
                                CycleName = reader["CycleName"].ToString(),
                                MS_GoalCategories_CategoryId = Convert.ToInt32(reader["MS_GoalCategories_CategoryId"].ToString()),
                                GoalCategoryName = reader["CategoryName"].ToString(),
                                GoalTitle = reader["GoalTitle"].ToString(),
                                GoalDescription = reader["GoalDescription"].ToString(),
                                Weightage = Convert.ToDecimal(reader["Weightage"].ToString()),
                                TargetValue = reader["TargetValue"].ToString(),
                                AchievementCriteria = reader["AchievementCriteria"].ToString(),
                                Status = reader["Status"].ToString()
                            };

                            empGoals.Add(empGoal);
                        }
                    }
                }
            }

            return empGoals;
        }

        public static List<PMSEmpGoals> GetEmployeesGoalsByCompId(Guid compId, int cycleId)
        {
            List<PMSEmpGoals> empGoals = new List<PMSEmpGoals>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PMSSqls.GetEmployeesGoalsByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", compId);
                    command.Parameters.AddWithValue("@cycleId", cycleId);


                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var empGoal = new PMSEmpGoals
                            {
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                EmployeeName = reader["FullName"].ToString()
                            };

                            empGoals.Add(empGoal);
                        }
                    }
                }
            }

            return empGoals;
        }

        public static bool UpsertEmployeeGoals(List<PMSEmployeeGoals> employeeGoals)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(employeeGoals);

            string sql = PMSSqls.UpsertEmployeeGoals;
            string strConString = DbContext.ConnectionString;

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = sql;

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@json", jsonString);

                int res = cmd.ExecuteNonQuery();

                if (res > 0)
                    return true;
                else
                    return false;
            }
        }

        public static List<PMSEmpGoalApproval> GetGoalApprovals(Guid compId, DateTime fromDt, DateTime toDt)
        {
            List<PMSEmpGoalApproval> pmsGoalApprovals = new List<PMSEmpGoalApproval>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PMSSqls.GetGoalsApprovalsByCompId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", compId);
                    command.Parameters.AddWithValue("@FromDt", fromDt);
                    command.Parameters.AddWithValue("@ToDt", toDt);


                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pmsGoalApproval = new PMSEmpGoalApproval
                            {
                                ApprovalId = Convert.ToInt32(reader["ApprovalId"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                ApproverId = Guid.Parse(reader["ApproverId"].ToString()),
                                ApproverName = reader["ApproverName"].ToString(),
                                MS_PerformanceCycles_Id = Convert.ToInt32(reader["MS_PerformanceCycles_Id"].ToString()),
                                CycleName = reader["CycleName"].ToString(),
                                MS_GoalCategories_CategoryId = Convert.ToInt32(reader["MS_GoalCategories_CategoryId"].ToString()),
                                CategoryName = reader["CategoryName"].ToString(),
                                PMS_EmployeeGoals_GoalId = Convert.ToInt32(reader["PMS_EmployeeGoals_GoalId"].ToString()),
                                GoalTitle = reader["GoalTitle"].ToString(),
                                ApprovalStatus = reader["ApprovalStatus"].ToString(),
                                Comment = reader["Comment"].ToString(),
                                Active = Convert.ToBoolean(reader["CategoryName"].ToString()),
                            };

                            pmsGoalApprovals.Add(pmsGoalApproval);
                        }
                    }
                }
            }

            return pmsGoalApprovals;
        }

        public static List<PMSEmpGoalApproval> GetEmpGoalsApprovalStatusByCycleId(Guid compId, int cycleId)
        {
            List<PMSEmpGoalApproval> pmsGoalApprovals = new List<PMSEmpGoalApproval>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PMSSqls.GetEmpGoalsApprovalStatusByCycleId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@cycleId", cycleId);
                    command.Parameters.AddWithValue("@compId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var pmsGoalApproval = new PMSEmpGoalApproval
                            {
                                PMS_EmployeeGoals_GoalId = Convert.ToInt32(reader["PMS_EmployeeGoals_GoalId"].ToString()),
                                FormDate = Convert.ToDateTime(reader["FormDate"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                EmployeeName = reader["EmployeeName"].ToString(),
                                MS_PerformanceCycles_Id = Convert.ToInt32(reader["MS_PerformanceCycles_Id"].ToString()),
                                CycleName = reader["CycleName"].ToString(),
                                MS_GoalCategories_CategoryId = Convert.ToInt32(reader["MS_GoalCategories_CategoryId"].ToString()),
                                CategoryName = reader["CategoryName"].ToString(),
                                GoalTitle = reader["GoalTitle"].ToString(),
                                GoalStatus = reader["Status"].ToString(),
                                ApproverId = reader["ApproverId"] == DBNull.Value ? Guid.Empty : Guid.Parse(reader["ApproverId"].ToString()),
                                ApproverName = reader["ApproverName"].ToString(),
                                ApprovalStatus = reader["ApprovalStatus"].ToString(),
                                Comment = reader["Comment"].ToString()
                            };

                            pmsGoalApprovals.Add(pmsGoalApproval);
                        }
                    }
                }
            }

            return pmsGoalApprovals;
        }
        
        public static GoalDetails GetGoalDetailsByGoadId(Guid compId, int goalId)
        {
            GoalDetails pmsGoal = new GoalDetails();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PMSSqls.GetGoalDetailsByGoadId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@goalId", goalId);
                    command.Parameters.AddWithValue("@compId", compId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pmsGoal = new GoalDetails
                            {
                                GoalId = Convert.ToInt32(reader["GoalId"].ToString()),
                                FormDate = Convert.ToDateTime(reader["FormDate"].ToString()),
                                CompId = Guid.Parse(reader["CompId"].ToString()),
                                EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                EmployeeName = reader["EmployeeName"].ToString(),
                                EmpId = reader["EmpId"].ToString(),
                                DepartmentName = reader["DepartmentName"].ToString(),
                                DesignationName = reader["DesignationName"].ToString(),
                                BranchName = reader["BranchName"].ToString(),
                                MS_PerformanceCycles_Id = Convert.ToInt32(reader["MS_PerformanceCycles_Id"].ToString()),
                                CycleName = reader["CycleName"].ToString(),
                                CycleStartDate = Convert.ToDateTime(reader["StartDate"].ToString()),
                                CycleEndDate = Convert.ToDateTime(reader["EndDate"].ToString()),
                                MS_GoalCategories_CategoryId = Convert.ToInt32(reader["MS_GoalCategories_CategoryId"].ToString()),
                                GoalCategoryName = reader["CategoryName"].ToString(),
                                GoalTitle = reader["GoalTitle"].ToString(),
                                GoalDescription = reader["GoalDescription"].ToString(),
                                TargetValue = reader["TargetValue"].ToString(),
                                AchievementCriteria = reader["AchievementCriteria"].ToString(),
                                Weightage = Convert.ToDecimal(reader["Weightage"].ToString()),
                                Status = reader["Status"].ToString(),
                                ApproverId = reader["ApproverId"] == DBNull.Value ? Guid.Empty : Guid.Parse(reader["ApproverId"].ToString()),
                                ApproverName = reader["ApproverName"].ToString(),
                                ApprovalStatus = reader["ApprovalStatus"].ToString(),
                                ApprovalComment = reader["Comment"].ToString()
                            };

                            return pmsGoal;
                        }
                    }
                }
            }

            return pmsGoal;
        }

        public static List<EmployeeInformation> GetEmployeesByCycleIdAndStatus(int cycleId, string status)
        {
            List<EmployeeInformation> employees = new List<EmployeeInformation>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PMSSqls.GetEmployeesByCycleIdAndStatus;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@cycleId", cycleId);
                    command.Parameters.AddWithValue("@status", status);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var employee = new EmployeeInformation
                            {
                                Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                FullName = reader["FullName"].ToString()
                            };

                            employees.Add(employee);
                        }
                    }
                }
            }

            return employees;
        }

        public static bool UpsertPMSGoalApprovals(List<PMSGoalApprovals> goalApprovals)
        {
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(goalApprovals);

            string sql = PMSSqls.UpsertEmpGoalApproval;
            string strConString = DbContext.ConnectionString;

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = sql;

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@json", jsonString);

                var result = cmd.ExecuteScalar();

                return result != null && Convert.ToInt32(result) == 1;
            }
        }

        public static int UpsertPMSCheckIns(PMSPerformanceCheckIns performanceCheckIns)
        {
            try
            {
                string sql = PMSSqls.UpsertEmpGoalProgress;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@CheckInId", performanceCheckIns.CheckInId);
                    cmd.Parameters.AddWithValue("@MachineId", performanceCheckIns.MachineId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MachineIp", performanceCheckIns.MachineIp ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CompId", performanceCheckIns.CompId);
                    cmd.Parameters.AddWithValue("@PMS_EmployeeGoals_GoalId", performanceCheckIns.PMS_EmployeeGoals_GoalId);
                    cmd.Parameters.AddWithValue("@ProgressPercentage", performanceCheckIns.ProgressPercentage);
                    cmd.Parameters.AddWithValue("@Comments", performanceCheckIns.Comments ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AttachmentPath", performanceCheckIns.AttachmentPath ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Active", performanceCheckIns.Active);
                    cmd.Parameters.AddWithValue("@CreatedBy", performanceCheckIns.CreatedBy);
                    cmd.Parameters.AddWithValue("@ModifiedBy", performanceCheckIns.ModifiedBy);

                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        int id = int.Parse(returnObj.ToString());
                        return id;
                    }

                    return 0;
                }
            }
            catch (Exception ex)
            {
                var abc = ex;
                return 0;
            }
        }

        public static List<PMSEmpCheckIns> GetEmpGoalProgressByGoalId(Guid compId, int goalId)
        {
            List<PMSEmpCheckIns> empGoalProgresses = new List<PMSEmpCheckIns>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PMSSqls.GetEmpGoalProgressByGoalId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", compId);
                    command.Parameters.AddWithValue("@goalId", goalId);


                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var empGoalProgress = new PMSEmpCheckIns
                            {
                                FormDate = Convert.ToDateTime(reader["FormDate"].ToString()),
                                MS_PerformanceCycles_Id = Convert.ToInt32(reader["MS_PerformanceCycles_Id"].ToString()),
                                CycleName = reader["CycleName"].ToString(),
                                GoalTitle = reader["GoalTitle"].ToString(),
                                MS_GoalCategories_CategoryId = Convert.ToInt32(reader["MS_GoalCategories_CategoryId"].ToString()),
                                CategoryName = reader["CategoryName"].ToString(),
                                ProgressPercentage = Convert.ToDecimal(reader["ProgressPercentage"].ToString()),
                                Comments = reader["Comments"].ToString(),
                                EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                FullName = reader["EmployeeName"].ToString()
                            };

                            empGoalProgresses.Add(empGoalProgress);
                        }
                    }
                }
            }

            return empGoalProgresses;
        }

        public static List<PMSEmpCheckIns> GetEmpGoalCheckIns(Guid compId, Guid empId)
        {
            List<PMSEmpCheckIns> empGoalProgresses = new List<PMSEmpCheckIns>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PMSSqls.GetEmpGoalProgressByEmpId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CompId", compId);
                    command.Parameters.AddWithValue("@EmpId", empId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var empGoalProgress = new PMSEmpCheckIns
                            {
                                CheckInId = Convert.ToInt32(reader["CheckInId"].ToString()),
                                EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                GoalTitle = reader["GoalTitle"].ToString(),
                                GoalDescription = reader["GoalDescription"].ToString(),
                                Weightage = Convert.ToDecimal(reader["Weightage"].ToString()),
                                TargetValue = reader["TargetValue"].ToString(),
                                ProgressPercentage = Convert.ToDecimal(reader["ProgressPercentage"].ToString()),
                                Comments = reader["Comments"].ToString()
                            };

                            empGoalProgresses.Add(empGoalProgress);
                        }
                    }
                }
            }

            return empGoalProgresses;
        }

        public static bool UpsertSelfEvaluations(EmpSelfEvaluations selfEvaluation)
        {
            string strConString = DbContext.ConnectionString;
            using (SqlConnection conn = new SqlConnection(strConString))
            {
                conn.Open();

                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // SCRIPT 1
                    using (SqlCommand cmd = new SqlCommand(PMSSqls.UpsertEmpSelfEvaluation, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@SelfEvaluationId", selfEvaluation.SelfEvaluationId);
                        cmd.Parameters.AddWithValue("@MachineId", selfEvaluation.MachineId);
                        cmd.Parameters.AddWithValue("@MachineIp", selfEvaluation.MachineIp);
                        cmd.Parameters.AddWithValue("@CompId", selfEvaluation.CompId);
                        cmd.Parameters.AddWithValue("@MS_PerformanceCycles_CycleId", selfEvaluation.MS_PerformanceCycles_CycleId);
                        cmd.Parameters.AddWithValue("@EMP_Info_Id", selfEvaluation.EMP_Info_Id);
                        cmd.Parameters.AddWithValue("@SubmittedDate", selfEvaluation.SubmittedDate == DateTime.MinValue? (object)DBNull.Value: selfEvaluation.SubmittedDate);
                        cmd.Parameters.AddWithValue("@OverallComments", selfEvaluation.OverallComments);
                        cmd.Parameters.AddWithValue("@Status", selfEvaluation.Status);
                        cmd.Parameters.AddWithValue("@Active", selfEvaluation.Active);
                        cmd.Parameters.AddWithValue("@CreatedBy", selfEvaluation.CreatedBy);
                        cmd.Parameters.AddWithValue("@ModifiedBy", selfEvaluation.ModifiedBy);

                        object returnObj = cmd.ExecuteScalar();
                        if (returnObj != null)
                        {
                            selfEvaluation.SelfEvaluationId = int.Parse(returnObj.ToString());
                            if (selfEvaluation.SelfEvaluationDetails != null
                                && selfEvaluation.SelfEvaluationDetails.Count() > 0)
                            {
                                selfEvaluation.SelfEvaluationDetails?
                                    .Select(x =>
                                    {
                                        x.PMS_SelfEvaluations_SelfEvaluationId =
                                            selfEvaluation.SelfEvaluationId;

                                        return x;
                                    })
                                    .ToList();
                            }
                        }
                    }

                    // SCRIPT 2
                    if (selfEvaluation.SelfEvaluationDetails != null && selfEvaluation.SelfEvaluationDetails.Any() 
                        && selfEvaluation.SelfEvaluationId > 0)
                    {
                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(selfEvaluation.SelfEvaluationDetails);
                        using (SqlCommand cmd = new SqlCommand(PMSSqls.UpsertEmpSelfEvaluationDetails, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Json", jsonString);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Commit transaction
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    // Rollback transaction
                    transaction.Rollback();

                    Console.WriteLine("Error: " + ex.Message);

                    return false;
                }
            }
        }

        public static List<EmpSelfEvaluations> GetEmployeesSelfEvaluationsByStatus(Guid compId, int cycleId, string status)
        {
            List<EmpSelfEvaluations> empSelfEvaluations = new List<EmpSelfEvaluations>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PMSSqls.GetEmployeesSelfEvaluationsByStatus;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);
                    command.Parameters.AddWithValue("@cycleId", cycleId);
                    command.Parameters.AddWithValue("@status", status);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var empSelfEvaluation = new EmpSelfEvaluations
                            {
                                SelfEvaluationId = Convert.ToInt32(reader["SelfEvaluationId"].ToString()),
                                MS_PerformanceCycles_CycleId = Convert.ToInt32(reader["MS_PerformanceCycles_CycleId"].ToString()),
                                CycleName = reader["CycleName"].ToString(),
                                EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                FullName = reader["FullName"].ToString(),
                                SubmittedDate = Convert.ToDateTime(reader["SubmittedDate"].ToString()),
                                OverallComments = reader["OverallComments"].ToString(),
                                Status = reader["Status"].ToString()
                            };

                            empSelfEvaluations.Add(empSelfEvaluation);
                        }
                    }
                }
            }

            return empSelfEvaluations;
        }

        public static List<EmpSelfEvaluationDetails> GetEmpSelfEvaluationGoalDetailsByCycleAndEmp(Guid empId, int cycleId)
        {
            List<EmpSelfEvaluationDetails> empSelfEvaluations = new List<EmpSelfEvaluationDetails>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PMSSqls.GetEmpSelfEvaluationGoalDetailsByCycleAndEmp;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@empId", empId);
                    command.Parameters.AddWithValue("@cycleId", cycleId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var empSelfEvaluation = new EmpSelfEvaluationDetails
                            {
                                Id = reader["Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Id"].ToString()),
                                PMS_SelfEvaluations_SelfEvaluationId = reader["PMS_SelfEvaluations_SelfEvaluationId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PMS_SelfEvaluations_SelfEvaluationId"].ToString()),
                                PMS_EmployeeGoals_GoalId = reader["PMS_EmployeeGoals_GoalId"] == DBNull.Value ? 0: Convert.ToInt32(reader["PMS_EmployeeGoals_GoalId"].ToString()),
                                CycleName = reader["CycleName"].ToString(),
                                SelfRating = reader["SelfRating"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["SelfRating"].ToString()),
                                EmployeeName = reader["FullName"].ToString(),
                                Comments = reader["Comments"] == DBNull.Value ? "" : reader["Comments"].ToString(),
                                GoalDescription = reader["GoalDescription"] == DBNull.Value ? "" : reader["GoalDescription"].ToString(),
                                GoalTitle = reader["GoalTitle"].ToString(),
                                OverallComments = reader["OverallComments"].ToString()
                            };

                            empSelfEvaluations.Add(empSelfEvaluation);
                        }
                    }
                }
            }

            return empSelfEvaluations;
        }

        public static List<MngrEvaluationGoalDetail> GetMngrEvaluationGoalDetailsByCycleAndEmp(Guid empId, int cycleId)
        {
            List<MngrEvaluationGoalDetail> mngrEvaluations = new List<MngrEvaluationGoalDetail>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PMSSqls.GetMngrEvaluationGoalDetailsByCycleAndEmp;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@empId", empId);
                    command.Parameters.AddWithValue("@cycleId", cycleId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var mngrEval = new MngrEvaluationGoalDetail
                            {
                                PMS_EmployeeGoals_GoalId = reader["PMS_EmployeeGoals_GoalId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PMS_EmployeeGoals_GoalId"].ToString()),
                                CycleName = reader["CycleName"].ToString(),
                                SelfRating = reader["SelfRating"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["SelfRating"].ToString()),
                                EmployeeName = reader["FullName"].ToString(),
                                SelfEvalGoalComments = reader["Comments"] == DBNull.Value ? "" : reader["Comments"].ToString(),
                                GoalDescription = reader["GoalDescription"] == DBNull.Value ? "" : reader["GoalDescription"].ToString(),
                                GoalTitle = reader["GoalTitle"].ToString(),
                                OverallComments = reader["OverallComments"].ToString(),
                                PMS_ManagerEvaluations_Id = reader["PMS_ManagerEvaluations_Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PMS_ManagerEvaluations_Id"].ToString()),
                                ManagerId = reader["ManagerId"] == DBNull.Value ? Guid.Empty : Guid.Parse(reader["ManagerId"].ToString()),
                                MngrOverallRating = reader["MngrOverallRating"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["MngrOverallRating"].ToString()),
                                MngrOverallComments = reader["MngrOverallComments"] == DBNull.Value ? "" : reader["MngrOverallComments"].ToString(),
                                Id = reader["MngrEvalDetailId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MngrEvalDetailId"].ToString()),
                                MngrGoalId = reader["MngrGoalId"] == DBNull.Value ? 0: Convert.ToInt32(reader["MngrGoalId"]),
                                MngrGoalRating = reader["MngrGoalRating"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["MngrGoalRating"].ToString()),
                                MngrGoalComment = reader["MngrGoalComment"] == DBNull.Value ? "" : reader["MngrGoalComment"].ToString(),
                            };

                            mngrEvaluations.Add(mngrEval);
                        }
                    }
                }
            }

            return mngrEvaluations;
        }

        public static bool UpsertManagerEvaluations(MngrEvaluation mngrEvaluation)
        {
            string strConString = DbContext.ConnectionString;
            using (SqlConnection conn = new SqlConnection(strConString))
            {
                conn.Open();

                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // SCRIPT 1
                    using (SqlCommand cmd = new SqlCommand(PMSSqls.UpsertManagerEvaluation, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Id", mngrEvaluation.Id);
                        cmd.Parameters.AddWithValue("@MachineId", mngrEvaluation.MachineId);
                        cmd.Parameters.AddWithValue("@MachineIp", mngrEvaluation.MachineIp);
                        cmd.Parameters.AddWithValue("@CompId", mngrEvaluation.CompId);
                        cmd.Parameters.AddWithValue("@MS_PerformanceCycles_CycleId", mngrEvaluation.MS_PerformanceCycles_CycleId);
                        cmd.Parameters.AddWithValue("@EMP_Info_Id", mngrEvaluation.EMP_Info_Id);
                        cmd.Parameters.AddWithValue("@ManagerId", mngrEvaluation.ManagerId);
                        cmd.Parameters.AddWithValue("@OverallRating", mngrEvaluation.OverallRating);
                        cmd.Parameters.AddWithValue("@SubmittedDate", mngrEvaluation.SubmittedDate);
                        cmd.Parameters.AddWithValue("@OverallComments", mngrEvaluation.OverallComments);
                        cmd.Parameters.AddWithValue("@Status", mngrEvaluation.Status);
                        cmd.Parameters.AddWithValue("@Active", mngrEvaluation.Active);
                        cmd.Parameters.AddWithValue("@CreatedBy", mngrEvaluation.CreatedBy);
                        cmd.Parameters.AddWithValue("@ModifiedBy", mngrEvaluation.ModifiedBy);

                        object returnObj = cmd.ExecuteScalar();
                        if (returnObj != null)
                        {
                            mngrEvaluation.Id = int.Parse(returnObj.ToString());
                            
                            if (mngrEvaluation.Id != null && mngrEvaluation.Id > 0
                                && mngrEvaluation.ManagerEvaluationDetails.Count() > 0)
                            {
                                mngrEvaluation.ManagerEvaluationDetails?
                                    .Select(x =>
                                    {
                                        x.PMS_ManagerEvaluations_Id =
                                            mngrEvaluation.Id;

                                        return x;
                                    })
                                    .ToList();
                            }
                        }
                    }

                    // SCRIPT 2
                    if (mngrEvaluation.ManagerEvaluationDetails != null && mngrEvaluation.ManagerEvaluationDetails.Any()
                        && mngrEvaluation.Id > 0)
                    {
                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(mngrEvaluation.ManagerEvaluationDetails);
                        using (SqlCommand cmd = new SqlCommand(PMSSqls.UpsertManagerEvaluationDetails, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Json", jsonString);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Commit transaction
                    transaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    // Rollback transaction
                    transaction.Rollback();

                    Console.WriteLine("Error: " + ex.Message);

                    return false;
                }
            }
        }

        public static List<MngrEvaluation> GetMngrEvaluationsByManagerAndStatus(Guid compId, string status)
        {
            List<MngrEvaluation> mngrEvaluations = new List<MngrEvaluation>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PMSSqls.GetManagerEvaluationsByMngrId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);
                    command.Parameters.AddWithValue("@status", status);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var mngrEvaluation = new MngrEvaluation
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                EmpName = reader["EmpName"].ToString(),
                                ManagerId = Guid.Parse(reader["ManagerId"].ToString()),
                                ManagerName = reader["ManagerName"].ToString(),
                                MS_PerformanceCycles_CycleId = Convert.ToInt32(reader["MS_PerformanceCycles_CycleId"].ToString()),
                                CycleName = reader["CycleName"].ToString(),
                                OverallRating = Convert.ToInt32(reader["OverallRating"].ToString()),
                                OverallComments = reader["OverallComments"].ToString(),
                                SubmittedDate = Convert.ToDateTime(reader["SubmittedDate"].ToString()),
                                Status = reader["Status"].ToString()
                            };

                            mngrEvaluations.Add(mngrEvaluation);
                        }
                    }
                }
            }

            return mngrEvaluations;
        }

        public static List<MngrEvaluation> GetMngrEvaluationsByDates(Guid compId, DateTime frmdt, DateTime todt)
        {
            List<MngrEvaluation> mngrEvaluations = new List<MngrEvaluation>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PMSSqls.GetManagerEvaluationsByDates;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);
                    command.Parameters.AddWithValue("@FromDt", frmdt);
                    command.Parameters.AddWithValue("@ToDt", todt);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var mngrEvaluation = new MngrEvaluation
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                EmpName = reader["EmpName"].ToString(),
                                ManagerId = Guid.Parse(reader["ManagerId"].ToString()),
                                ManagerName = reader["ManagerName"].ToString(),
                                MS_PerformanceCycles_CycleId = Convert.ToInt32(reader["MS_PerformanceCycles_CycleId"].ToString()),
                                CycleName = reader["CycleName"].ToString(),
                                OverallRating = Convert.ToInt32(reader["OverallRating"].ToString()),
                                OverallComments = reader["OverallComments"].ToString(),
                                SubmittedDate = Convert.ToDateTime(reader["SubmittedDate"].ToString()),
                                Status = reader["Status"].ToString()
                            };

                            mngrEvaluations.Add(mngrEvaluation);
                        }
                    }
                }
            }

            return mngrEvaluations;
        }

        public static List<HRCalibrationReviewDetail> GetMngrEvaluatedEmpByCycleId(int cycleId, string status)
        {
            List<HRCalibrationReviewDetail> emps = new List<HRCalibrationReviewDetail>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PMSSqls.GetMngrEvaluatedEmpByCycleId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@cycleId", cycleId);
                    command.Parameters.AddWithValue("@status", status);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var emp = new HRCalibrationReviewDetail
                            {
                                EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                EmployeeName = reader["EmployeeName"].ToString()
                            };

                            emps.Add(emp);
                        }
                    }
                }
            }

            return emps;
        }
        
        public static bool UpsertHRCalibrationReview(List<HRCalibrationReview> calibrationReviews)
        {
            bool result = false;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(calibrationReviews);

            string sql = PMSSqls.UpsertHRCalibrationReviews;
            string strConString = DbContext.ConnectionString;

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = sql;

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@json", jsonString);

                object res = cmd.ExecuteScalar();

                if (res != null && res != DBNull.Value)
                {
                    result = Convert.ToBoolean(res);
                }
            }
            return result;
        }

        public static List<HRCalibrationReviewDetail> GetHRCalibrationReviewsByCycleId(Guid compId, int cycleId)
        {
            List<HRCalibrationReviewDetail> hrEvaluations = new List<HRCalibrationReviewDetail>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PMSSqls.GetHRCalibrationReviewsByCycleId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@compId", compId);
                    command.Parameters.AddWithValue("@cycleId", cycleId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var hrEvaluation = new HRCalibrationReviewDetail
                            {
                                Id = Convert.ToInt32(reader["Id"].ToString()),
                                FormDate = Convert.ToDateTime(reader["FormDate"].ToString()),
                                EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                EmployeeName = reader["EmployeeName"].ToString(),
                                MS_PerformanceCycles_CycleId = Convert.ToInt32(reader["MS_PerformanceCycles_CycleId"].ToString()),
                                CycleName = reader["CycleName"].ToString(),
                                PMS_ManagerEvaluations_Id = Convert.ToInt32(reader["PMS_ManagerEvaluations_Id"].ToString()),
                                ManagerName = reader["ManagerName"].ToString(),
                                ReviewerId = Guid.Parse(reader["ReviewerId"].ToString()),
                                ReviewerName = reader["ReviewerName"].ToString(),
                                FinalRating = Convert.ToDecimal(reader["FinalRating"].ToString()),
                                ReviewDate = Convert.ToDateTime(reader["ReviewDate"].ToString()),
                                Comments = reader["Comments"].ToString()
                            };

                            hrEvaluations.Add(hrEvaluation);
                        }
                    }
                }
            }

            return hrEvaluations;
        }

        public static bool UpsertPerformanceOutcomes(List<PMSPerformanceOutcomes> performanceOutcomes)
        {
            bool result = false;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(performanceOutcomes);

            string sql = PMSSqls.UpsertPerformanceOutcomes;
            string strConString = DbContext.ConnectionString;

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = sql;

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@json", jsonString);

                object res = cmd.ExecuteScalar();

                if (res != null && res != DBNull.Value)
                {
                    result = Convert.ToBoolean(res);
                }
            }
            return result;
        }
    
        public static List<PMSPerformanceOutcomesDetail> GetPerformanceOutcomesByCycleId(int cycleId)
        {
            List<PMSPerformanceOutcomesDetail> perfOutcomes = new List<PMSPerformanceOutcomesDetail>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PMSSqls.GetPerformanceOutcomesByCycleId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@cycleId", cycleId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var perfOutcome = new PMSPerformanceOutcomesDetail
                            {
                                PMS_CalibrationReviews_Id = Convert.ToInt32(reader["PMS_CalibrationReviews_Id"].ToString()),
                                MS_PerformanceCycles_CycleId = Convert.ToInt32(reader["MS_PerformanceCycles_CycleId"].ToString()),
                                FinalRating = Convert.ToDecimal(reader["FinalRating"].ToString()),
                                EMP_Info_Id = Guid.Parse(reader["EMP_Info_Id"].ToString()),
                                EmployeeName = reader["EmployeeName"].ToString(),
                                Id = reader["Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Id"].ToString()),
                                MS_PerformanceBand_Id = reader["MS_PerformanceBand_Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MS_PerformanceBand_Id"].ToString()),
                                BonusEligible = reader["BonusEligible"] == DBNull.Value ? false : Convert.ToBoolean(reader["BonusEligible"].ToString()),
                                PromotionRecommendation = reader["PromotionRecommendation"] == DBNull.Value ? false : Convert.ToBoolean(reader["PromotionRecommendation"].ToString()),
                                PromotionEligible = reader["PromotionEligible"] == DBNull.Value ? false : Convert.ToBoolean(reader["PromotionEligible"].ToString()),
                                BonusAmount = reader["BonusAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["BonusAmount"].ToString()),
                                IncrementPercentage = reader["IncrementPercentage"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["IncrementPercentage"].ToString()),
                                DevelopmentPlan = reader["DevelopmentPlan"] == DBNull.Value ? "" : reader["DevelopmentPlan"].ToString(),
                                PublishedDate = reader["PublishedDate"] == DBNull.Value ? DateTime.MinValue.Date : Convert.ToDateTime(reader["PublishedDate"].ToString()),
                                OutcomeStatus = reader["OutcomeStatus"] == DBNull.Value ? "" : reader["OutcomeStatus"].ToString(),
                                ApprovedBy = reader["ApprovedBy"] == DBNull.Value ? Guid.Empty : Guid.Parse(reader["ApprovedBy"].ToString()),
                                ApprovedDate = reader["ApprovedDate"] == DBNull.Value ? DateTime.MinValue.Date : Convert.ToDateTime(reader["ApprovedDate"].ToString()),
                                AppraisalLetterGenerated = reader["AppraisalLetterGenerated"] == DBNull.Value ? false : Convert.ToBoolean(reader["AppraisalLetterGenerated"].ToString()),
                                AppraisalLetterGeneratedDate = reader["AppraisalLetterGeneratedDate"] == DBNull.Value ? DateTime.MinValue.Date : Convert.ToDateTime(reader["AppraisalLetterGeneratedDate"].ToString()),
                                PromotionEffectiveDate = reader["PromotionEffectiveDate"] == DBNull.Value ? DateTime.MinValue.Date : Convert.ToDateTime(reader["PromotionEffectiveDate"].ToString()),
                                IncrementEffectiveDate = reader["IncrementEffectiveDate"] == DBNull.Value ? DateTime.MinValue.Date : Convert.ToDateTime(reader["IncrementEffectiveDate"].ToString()),
                                Remarks = reader["Remarks"] == DBNull.Value ? "" : reader["Remarks"].ToString(),
                                IsAcknowledgedByEmployee = reader["IsAcknowledgedByEmployee"] == DBNull.Value ? false : Convert.ToBoolean(reader["IsAcknowledgedByEmployee"].ToString()),
                                AcknowledgedDate = reader["AcknowledgedDate"] == DBNull.Value ? DateTime.MinValue.Date : Convert.ToDateTime(reader["AcknowledgedDate"].ToString())
                            };

                            perfOutcomes.Add(perfOutcome);
                        }
                    }
                }
            }

            return perfOutcomes;
        }

        public static int UpsertEmployeeFeedback(PMSFeedbacks feedback)
        {
           try
            {
                string sql = PMSSqls.UpsertEmpFeedback;
                string strConString = DbContext.ConnectionString;

                using (SqlConnection con = new SqlConnection(strConString))
                {
                    con.Open();
                    string query = sql;

                    SqlCommand cmd = new SqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@Id", feedback.Id);
                    cmd.Parameters.AddWithValue("@MachineId", feedback.MachineId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MachineIp", feedback.MachineIp ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CompId", feedback.CompId);
                    cmd.Parameters.AddWithValue("@FromEmployeeId", feedback.FromEmployeeId);
                    cmd.Parameters.AddWithValue("@ToEmployeeId", feedback.ToEmployeeId);
                    cmd.Parameters.AddWithValue("@MS_PerformanceCycles_CycleId", feedback.MS_PerformanceCycles_CycleId);
                    cmd.Parameters.AddWithValue("@FeedbackType", feedback.FeedbackType ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FeedbackText", feedback.FeedbackText ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Rating", feedback.Rating);
                    cmd.Parameters.AddWithValue("@CreatedBy", feedback.CreatedBy);
                    cmd.Parameters.AddWithValue("@ModifiedBy", feedback.ModifiedBy);

                    object returnObj = cmd.ExecuteScalar();

                    if (returnObj != null)
                    {
                        int id = int.Parse(returnObj.ToString());
                        return id;
                    }

                    return 0;
                }
            }
            catch (Exception ex)
            {
                var abc = ex;
                return 0;
            }
        }

        public static List<PMSFeedbackDetails> GetPMSFeedbackDetailsByCycleId(int cycleId)
        {
            List<PMSFeedbackDetails> empFeedbcks = new List<PMSFeedbackDetails>();
            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            {
                connection.Open();
                string sql = PMSSqls.GetPMSFeedbackDetailsByCycleId;
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@cycleId", cycleId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var empFeedbck = new PMSFeedbackDetails
                            {
                                Id = reader["Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Id"].ToString()),
                                FromEmployeeId = Guid.Parse(reader["FromEmployeeId"].ToString()),
                                FromEmployeeName = reader["FromEmployeeName"].ToString(),
                                ToEmployeeId = Guid.Parse(reader["ToEmployeeId"].ToString()),
                                ToEmployeeName = reader["ToEmployeeName"].ToString(),
                                MS_PerformanceCycles_CycleId = reader["MS_PerformanceCycles_CycleId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["MS_PerformanceCycles_CycleId"].ToString()),
                                CycleName = reader["CycleName"].ToString(),
                                FeedbackType = reader["FeedbackType"] == DBNull.Value ? "" : reader["FeedbackType"].ToString(),
                                FeedbackText = reader["FeedbackText"] == DBNull.Value ? "" : reader["FeedbackText"].ToString(),
                                Rating = reader["Rating"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Rating"].ToString()),
                                SubmittedDate = Convert.ToDateTime(reader["SubmittedDate"].ToString())
                            };

                            empFeedbcks.Add(empFeedbck);
                        }
                    }
                }
            }

            return empFeedbcks;
        }
    }
}
