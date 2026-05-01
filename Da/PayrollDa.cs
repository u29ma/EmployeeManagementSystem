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
            public List<PayrollModel> GetPayrollByEmployee(int empId)
            {
                return _context.Payroll
                    .Where(p => p.EmployeeId == empId)
                    .ToList();
            }
        }
    }


