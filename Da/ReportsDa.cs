using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.ReportsViewModels;

namespace EmployeeManagementSystem.Da
{
    public class ReportsDa
    {
        private readonly ApplicationDbContext _context;
        public ReportsDa(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<EmployeeReportVM> GetEmployeeReport(string search)
        {
            var query = from e in _context.Employees
                        join d in _context.Departments
                        on e.DepartmentId equals d.DepartmentId
                        select new EmployeeReportVM
                        {
                            EmployeeId = e.EmployeeId,
                            FullName = e.FirstName + " " + e.LastName,
                            DepartmentName = d.DepartmentName,
                            Designation = e.Designation,
                            JoinDate = e.JoiningDate
                        };

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.FullName.Contains(search));
            }

            return query.ToList();
        }
        public List<PayrollReportVM> GetPayrollReport(string search)
        {
            var query = from p in _context.Payroll
                        join e in _context.Employees
                        on p.EmployeeId equals e.EmployeeId
                        select new PayrollReportVM
                        {
                            PayrollId = p.PayrollId,

                            EmployeeName = e.FirstName + " " + e.LastName,

                            BasicSalary = p.BasicSalary,

                            Bonus = p.Bonus,

                            Deduction = p.Deductions,

                            NetSalary = p.BasicSalary + p.Bonus - p.Deductions,

                            Month = p.SalaryMonth
                        };

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x =>
                    x.EmployeeName.Contains(search));
            }

            return query.ToList();
        }
        public PayslipVM GetPayslip(int payrollId)
        {
            var data = (from p in _context.Payroll
                        join e in _context.Employees
                        on p.EmployeeId equals e.EmployeeId

                        join d in _context.Departments
                        on e.DepartmentId equals d.DepartmentId

                        where p.PayrollId == payrollId

                        select new PayslipVM
                        {
                            EmployeeName = e.FirstName + " " + e.LastName,

                            Department = d.DepartmentName,

                            Designation = e.Designation,

                            BasicSalary = p.BasicSalary,

                            Bonus = p.Bonus,

                            Deduction = p.Deductions,

                            NetSalary = p.BasicSalary + p.Bonus - p.Deductions,

                            Month = p.SalaryMonth,

                            PaymentDate = p.PaymentDate
                        }).FirstOrDefault();

            return data;
        }
    }
}
