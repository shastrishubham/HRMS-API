using ServerModel.Data;
using ServerModel.Model;
using ServerModel.Model.Payroll;
using ServerModel.ServerModel.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HRMS_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PayrollController : ApiController
    {
        PayrollInfoServer payrollInfoServer;

        public PayrollController()
        {
            DbContext.ConnectionString = WebApiConfig.ConnectionString;
            payrollInfoServer = new PayrollInfoServer();
        }

        [Route("api/Payroll/GetEmployeePayrollDetailsByBranchId")]
        [HttpGet]
        public List<dynamic> GetEmpPayrollDetailsByBranchId(int year, int month, int branchId, Guid employeeId)
        {
            return payrollInfoServer.GetEmpPayrollDetailsByBranchId(year, month, branchId, employeeId);
        }

        [Route("api/Payroll/GetCalculatedPayrollDetailsByBranchId")]
        [HttpGet]
        public List<PayrollInformation> GetCalculatedPayrollDetailsByBranchId(Guid compId, int year, int month, int branchId)
        {
            return payrollInfoServer.GetCalculatedPayrollDetailsByBranchId(year, month, branchId, compId);
        }

        [Route("api/Payroll/UpsertPayrollCreation")]
        [HttpPost]
        public DataResult UpsertPayrollCreation(List<PayrollInformation> payrolls)
        {
            return payrollInfoServer.UpsertPayrollCreation(payrolls);
        }

        [Route("api/Payroll/GetEmpPayrollDetailsByBranchId")]
        [HttpGet]
        public List<EmployeePayrollInformation> GetEmpPayrollDetailsByBranchId(Guid employeeId, int month, int year)
        {
            return payrollInfoServer.GetEmployeePayrollInformation(employeeId, month, year);
        }

        [Route("api/Payroll/GetEmployeeSalaryHeadsDetails")]
        [HttpGet]
        public List<EmployeePayrollInformation> GetEmployeeSalaryHeadsDetails(Guid employeeId, int month, int year)
        {
            return payrollInfoServer.GetEmployeeSalaryHeadsDetails(employeeId, month, year);
        }

        [Route("api/Payroll/GetEmployeeReimbursementsByBranchAndMonth")]
        [HttpGet]
        public List<PayrollReimbursement> GetEmployeeReimbursementsByBranchAndMonth(Guid compId, int year, int month, int branchId)
        {
            return payrollInfoServer.GetEmployeeReimbursementsByBranchAndMonth(year, month, branchId, compId);
        }

        [Route("api/Payroll/UpsertPayrollReimbursements")]
        [HttpPost]
        public void UpsertPayrollReimbursements(List<PayrollReimbursement> payrollReimbursements)
        {
            payrollInfoServer.UpsertPayrollReimbursements(payrollReimbursements);
        }


        [Route("api/Payroll/GetSalaryAdjustmentEmployeesByCompId")]
        [HttpGet]
        [AllowAnonymous]
        public List<SalaryAdjustment> GetSalaryAdjustmentEmployeesByCompId(Guid compId)
        {
            return payrollInfoServer.GetSalaryAdjustmentEmployeesByCompId(compId);
        }

        [Route("api/Payroll/GetSalaryAdjustmentsByPayrollMonthYear")]
        [HttpGet]
        [AllowAnonymous]
        public List<SalaryAdjustment> GetSalaryAdjustmentsByPayrollMonthYear(DateTime payrollDate)
        {
            return payrollInfoServer.GetSalaryAdjustmentsByPayrollMonthYear(payrollDate);
        }


        [Route("api/Payroll/UpsertSalaryAdjustment")]
        [HttpPost]
        public bool UpsertSalaryAdjustment(List<SalaryAdjustment> salaryAdjustments)
        {
            return payrollInfoServer.UpsertSalaryAdjustment(salaryAdjustments);
        }

    }
}