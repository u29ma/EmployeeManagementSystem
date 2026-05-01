using EmployeeManagementSystem.Da;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    public class PayrollController : Controller
    {

            private readonly PayrollDa _payrollDa;

            public PayrollController(PayrollDa payrollDa)
            {
                _payrollDa = payrollDa;
            }
        public IActionResult PendingPayroll()
        {
            var data = _payrollDa.GetPendingPayroll();
            return View(data);
        }

        // ================= ADMIN =================

        public IActionResult Index()
            {
                var data = _payrollDa.GetAllPayroll();
                return View(data);
            }

            public IActionResult CreatePayroll()
            {
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

                return View(payroll);
            }

            public IActionResult Approve(int id)
            {
                _payrollDa.ApprovePayroll(id);
                return RedirectToAction("Index");
            }

            // ================= EMPLOYEE =================

            public IActionResult MySalary()
            {
                var empIdStr = HttpContext.Session.GetString("EmployeeId");

                if (string.IsNullOrEmpty(empIdStr))
                {
                    return RedirectToAction("Login", "Account");
                }

                int empId = int.Parse(empIdStr);

                var data = _payrollDa.GetPayrollByEmployee(empId);
                return View(data);
            }
        }
    }


   
       

