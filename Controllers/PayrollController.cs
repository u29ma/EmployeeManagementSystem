using EmployeeManagementSystem.Da;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagementSystem.Controllers
{
    public class PayrollController : Controller
    {

        private readonly PayrollDa _payrollDa;
        private readonly EmployeeDa _employeeDa;

        public PayrollController(PayrollDa payrollDa, EmployeeDa employeeDa)
        {
            _payrollDa = payrollDa;
            _employeeDa = employeeDa;
        }
        public IActionResult PendingPayroll()
        {
            var data = _payrollDa.GetPendingPayroll();
            return View(data);
        }

        // ================= ADMIN =================


        public IActionResult Index(string search, string status, string? month, int page = 1)
        {
            int pageSize = 5;

            var query = _payrollDa.GetAllPayrollQueryable();

            // 🔍 Search (by employee name)
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(p =>
                    (p.Employee.FirstName + " " + p.Employee.LastName)
                        .Contains(search)
                );
            }

            // 🔄 Status Filter
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(p => p.Status == status);
            }

            // 📅 Month Filter
            if (!string.IsNullOrWhiteSpace(month))
            {
                month = month.Trim().ToLower();

                query = query.Where(p =>
                    (p.SalaryMonth ?? "").Trim().ToLower() == month
                );
            }

            // 📄 Pagination
            int totalRecords = query.Count();

            var data = query
                .OrderByDescending(p => p.PayrollId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.Month = month;

            return View(data);
        }
        public IActionResult CreatePayroll()
        {
            ViewBag.EmployeeList = new SelectList(_employeeDa.GetAllEmployees()
            .Select(e => new 
            {
                e.EmployeeId,
                FullName = e.FirstName + " " + e.LastName + " (" + e.DepartmentName + ")"
            }),
            "EmployeeId",
            "FullName"
            );
            return View();
        }

        [HttpPost]
        public IActionResult CreatePayroll(PayrollModel payroll)
        {
            if (ModelState.IsValid)
            {
                _payrollDa.AddPayroll(payroll);
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeList = new SelectList(_employeeDa.GetAllEmployees()
            .Select(e => new
            {
                e.EmployeeId,
                FullName = e.FirstName + " " + e.LastName + " (" + e.DepartmentName + ")"
            }),
            "EmployeeId",
            "FullName"
            );

            return View(payroll);
        }

        public IActionResult Approve(int id)
        {
            _payrollDa.ApprovePayroll(id);
            return RedirectToAction("Index");
        }

        // ================= EMPLOYEE =================

        public IActionResult MySalary(string month, string status)
        {
            var empIdStr = HttpContext.Session.GetString("EmployeeId");

            if (string.IsNullOrEmpty(empIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            int empId = int.Parse(empIdStr);

            var data = _payrollDa.GetPayrollByEmployee(empId, month, status);

            // 🔁 keep selected values in UI
            ViewBag.Month = month;
            ViewBag.Status = status;

            return View(data);
        }
       
    }
}





