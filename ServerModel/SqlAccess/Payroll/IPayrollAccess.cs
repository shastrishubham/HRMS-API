using ServerModel.Model.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.SqlAccess.Payroll
{
    public interface IPayrollAccess
    {
        List<dynamic> GetEmpPayrollDetailsByBranchId(int year, int month, int branchId, Guid employeeId);

        List<PayrollInformation> GetCalculatedPayrollDetailsByBranchId(int year, int month, int branchId, Guid compId);

        void UpsertPayrollCreation(List<PayrollInformation> payrollInformation);

        bool UpsertSalaryAdjustment(List<SalaryAdjustment> salaryAdjustments);

        List<SalaryAdjustment> GetSalaryAdjustmentEmployeesByCompId(Guid compId);

        void UpsertPayrollReimbursements(List<PayrollReimbursement> payrollReimbursements);

        List<EmployeePayrollInformation> GetEmployeePayrollInformation(Guid employeeId, int month, int year);

        List<EmployeePayrollInformation> GetEmployeeSalaryHeadsDetails(Guid employeeId, int month, int year);

        List<PayrollReimbursement> GetEmployeeReimbursementsByBranchAndMonth(int year, int month, int branchId, Guid compId);

        List<SalaryAdjustment> GetCalcSalaryAdjustmentsEmployeesByAdjustmentId(List<Guid> employeeIds, int adjustmentId);

        List<SalaryAdjustment> GetSalaryAdjustmentsByPayrollMonthYear(DateTime payrollDate);
    }
}

