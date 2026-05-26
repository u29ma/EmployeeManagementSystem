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
                        join des in _context.Designations
                        on e.DesignationId equals des.DesignationId

                        select new EmployeeReportVM
                        {
                            EmployeeId = e.EmployeeId,
                            FullName = e.FirstName + " " + e.LastName,
                            DepartmentName = d.DepartmentName,
                            Designation = des.DesignationName,
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
                query = query.Where(x => x.EmployeeName.Contains(search));
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
                        join des in _context.Designations
                        on e.DesignationId equals des.DesignationId
                        where p.PayrollId == payrollId

                        select new PayslipVM
                        {
                            EmployeeName = e.FirstName + " " + e.LastName,
                            Department = d.DepartmentName,
                            Designation =des.DesignationName,
                            BasicSalary = p.BasicSalary,
                            Bonus = p.Bonus,
                            Deduction = p.Deductions,
                            NetSalary = p.BasicSalary + p.Bonus - p.Deductions,
                            Month = p.SalaryMonth,
                            PaymentDate = p.PaymentDate
                        }).FirstOrDefault();

            return data;
        }
        public List<AttendanceReportVM> GetAttendanceReport(string search, string month)
        {
            var query = from a in _context.Attendance
                        join e in _context.Employees
                        on a.EmployeeId equals e.EmployeeId

                        select new AttendanceReportVM
                        {
                            AttendanceId = a.AttendanceId,
                            EmployeeName = e.FirstName + " " + e.LastName,
                            Date = a.Date,
                            CheckInTime = a.CheckIn,
                            CheckOutTime = a.CheckOut,
                            Status = a.Status
                        };

            // 🔍 Search
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.EmployeeName.Contains(search));
            }

            // 📅 Month Filter
            if (!string.IsNullOrEmpty(month))
            {
                query = query.Where(x => x.Date.ToString("MMMM") == month);
            }

            return query.OrderByDescending(x => x.Date).ToList();
        }
    }
}
