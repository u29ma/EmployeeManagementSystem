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
            return _context.Payrolls
                .Include(p => p.Employee)
                .Where(p => p.Status == "Pending")
                .ToList();
        }

        public IQueryable<PayrollModel> GetAllPayrollQueryable()
        {
            return _context.Payrolls
                .Include(p => p.Employee);
        }
        // 👉 Get all payroll (Admin)
        public List<PayrollModel> GetAllPayroll()
        {
            return _context.Payrolls
                .Include(p => p.Employee)
                .ToList();
        }
        // 👉 Insert payroll
        public bool AddPayroll(PayrollModel payroll)
        {
            var exists = _context.Payrolls.Any(p =>
                p.EmployeeId == payroll.EmployeeId &&
                p.SalaryMonth == payroll.SalaryMonth &&
                p.SalaryYear == payroll.SalaryYear);

            // ❌ Already exists
            if (exists)
            {
                return false;
            }

            payroll.PaymentDate = DateTime.Now;
            payroll.Status = "Pending";

            _context.Payrolls.Add(payroll);
            _context.SaveChanges();

            return true;
        }

        // 👉 Approve payroll
        public void ApprovePayroll(int id)
        {
            var data = _context.Payrolls.Find(id);
            if (data != null)
            {
                data.Status = "Paid";
                _context.SaveChanges();
            }
        }
        public void HoldPayroll(int id)
        {
            var data = _context.Payrolls.Find(id);

            if (data != null)
            {
                data.Status = "Hold";
                _context.SaveChanges();
            }
        }

        // 👉 Get employee payroll
        public List<PayrollModel> GetPayrollByEmployee(int empId, string month, string status)
        {
            var query = _context.Payrolls
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


