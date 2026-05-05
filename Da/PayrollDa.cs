using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Da;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Da
{
    public class PayrollDa
    {
        private readonly ApplicationDbContext _context;
        public PayrollDa(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<PayrollModel> GetPendingPayroll()
        {
            return _context.Payroll
                .Include(p => p.Employee)
                .Where(p => p.Status == "Pending")
                .ToList();
        }

        public IQueryable<PayrollModel> GetAllPayrollQueryable()
        {
            return _context.Payroll
                .Include(p => p.Employee);
        }
        // 👉 Get all payroll (Admin)
        public List<PayrollModel> GetAllPayroll()
        {
            return _context.Payroll
                .Include(p => p.Employee)
                .ToList();
        }
        // 👉 Insert payroll
        public void AddPayroll(PayrollModel payroll)
        {
            var exists = _context.Payroll.Any(p =>
            p.EmployeeId == payroll.EmployeeId &&
            p.SalaryMonth == payroll.SalaryMonth &&
            p.SalaryYear == payroll.SalaryYear);

            payroll.PaymentDate = DateTime.Now;

            _context.Payroll.Add(payroll);
            _context.SaveChanges();
        }

        // 👉 Approve payroll
        public void ApprovePayroll(int id)
        {
            var data = _context.Payroll.Find(id);
            if (data != null)
            {
                data.Status = "Approved";
                _context.SaveChanges();
            }
        }

        // 👉 Get employee payroll
        public List<PayrollModel> GetPayrollByEmployee(int empId, string month, string status)
        {
            var query = _context.Payroll
                .Where(p => p.EmployeeId == empId);

            // 📅 Month filter (ONLY if selected)
            if (!string.IsNullOrWhiteSpace(month))
            {
                query = query.Where(p => p.SalaryMonth == month);
            }

            // 🔄 Status filter (ONLY if selected)
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(p => p.Status == status);
            }

            return query
                .OrderByDescending(p => p.PayrollId)
                .ToList();
        }
    }
}


