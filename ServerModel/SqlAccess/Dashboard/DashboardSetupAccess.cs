using ServerModel.Data;
using ServerModel.Model.Dashboard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.Dashboard
{
    public class DashboardSetupAccess
    {
        public static DashboardResponseDto GetDashboard(Guid compId)
        {
            var dashboard = new DashboardResponseDto
            {
                BranchWiseEmployees = new List<BranchWiseEmployeeDto>(),
                DesignationWiseEmployees = new List<DesignationWiseEmployeeDto>(),
                DepartmentWiseEmployees = new List<DepartmentWiseEmployeeDto>(),
                ShiftWiseEmployees = new List<ShiftWiseEmployeeDto>(),
                PresentTodayBranchWise = new List<PresentTodayBranchWiseDto>(),
                HelpDeskTickets = new List<HelpDeskTicketStatusDto>(),
                InterviewVsOnboarded = new List<InterviewVsOnboardedDto>()
            };

            string strConString = DbContext.ConnectionString;

            using (var connection = new SqlConnection(strConString))
            using (var cmd = new SqlCommand("usp_HRMS_Dashboard", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompId", compId);
                connection.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    // 1. Summary
                    if (reader.Read())
                    {
                        dashboard.Summary = new DashboardSummaryDto
                        {
                            TotalEmpCount = reader.GetInt32(0),
                            CurrentMonthOnboardedEmpCount = reader.GetInt32(1),
                            TotalBranchCount = reader.GetInt32(2),
                            CurrentMonthOpenTickets = reader.GetInt32(3),
                            TodaysPresentEmpCount = reader.GetInt32(4)
                        };
                    }

                    // 2. Branch-wise
                    reader.NextResult();
                    while (reader.Read())
                    {
                        dashboard.BranchWiseEmployees.Add(new BranchWiseEmployeeDto
                        {
                            BranchName = reader["BranchName"].ToString(),
                            EmployeeCount = Convert.ToInt32(reader["EmployeeCount"])
                        });
                    }

                    // 3. Designation-wise
                    reader.NextResult();
                    while (reader.Read())
                    {
                        dashboard.DesignationWiseEmployees.Add(new DesignationWiseEmployeeDto
                        {
                            DesignationName = reader["DesignationName"].ToString(),
                            EmployeeCount = Convert.ToInt32(reader["EmployeeCount"])
                        });
                    }

                    // 4. Department-wise
                    reader.NextResult();
                    while (reader.Read())
                    {
                        dashboard.DepartmentWiseEmployees.Add(new DepartmentWiseEmployeeDto
                        {
                            DepartmentName = reader["DepartmentName"].ToString(),
                            EmployeeCount = Convert.ToInt32(reader["EmployeeCount"])
                        });
                    }

                    // 5. Shift-wise
                    reader.NextResult();
                    while (reader.Read())
                    {
                        dashboard.ShiftWiseEmployees.Add(new ShiftWiseEmployeeDto
                        {
                            BranchName = reader["BranchName"].ToString(),
                            ShiftName = reader["ShiftName"].ToString(),
                            EmployeeCount = Convert.ToInt32(reader["EmployeeCount"])
                        });
                    }

                    // 6. Present Today Branch Wise
                    reader.NextResult();
                    while (reader.Read())
                    {
                        dashboard.PresentTodayBranchWise.Add(new PresentTodayBranchWiseDto
                        {
                            BranchName = reader["BranchName"].ToString(),
                            PresentEmployeeCount = Convert.ToInt32(reader["PresentEmployeeCount"])
                        });
                    }

                    // 7. Help Desk Tickets
                    reader.NextResult();
                    while (reader.Read())
                    {
                        dashboard.HelpDeskTickets.Add(new HelpDeskTicketStatusDto
                        {
                            TicketStatus = reader["TicketStatus"].ToString(),
                            TicketCount = Convert.ToInt32(reader["TicketCount"])
                        });
                    }

                    // 8. Interview vs Onboarded
                    reader.NextResult();
                    while (reader.Read())
                    {
                        dashboard.InterviewVsOnboarded.Add(new InterviewVsOnboardedDto
                        {
                            MonthName = reader["MonthName"].ToString(),
                            InterviewScheduled = Convert.ToInt32(reader["InterviewScheduled"]),
                            Onboarded = Convert.ToInt32(reader["Onboarded"])
                        });
                    }
                }
            }

            return dashboard;
        }
    }
}
