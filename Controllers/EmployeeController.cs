using EmployeeManagementSystem.Da;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeDa _employeeDa;

        public EmployeeController(EmployeeDa emplDa)
        {
            _employeeDa = emplDa;
        }

        public IActionResult Index(string search, int? departmentId, bool? status)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("AccessDenied", "Home");

            var employees = _employeeDa.GetAllEmployees(search, departmentId, status);

            ViewBag.Departments = new SelectList(
                _employeeDa.GetDepartments(),
                "DepartmentId",
                "DepartmentName"
            );

            return View(employees);
        }
        // 🔹 GET: Add Employee Page
        public IActionResult AddEmployee()
        {
            ViewBag.Departments = _employeeDa.GetDepartments();
            return View();
        }
        // 🔹 POST: Save Employee
        [HttpPost]
        public IActionResult AddEmployee(EmployeeModel emp, string email, string password)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _employeeDa.AddEmployee(emp, email, password);

                    TempData["Success"] = "Employee Added Successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // ✅ Show error on UI
                    ModelState.AddModelError("", ex.Message);
                }
            }
            // 🔥 Reload dropdown if error occurs
            ViewBag.Departments = _employeeDa.GetDepartments();

            return View(emp);
          
        }
        //public IActionResult EmployeeList()
        //{
        //    var employees = _employeeDa.GetAllEmployees();  // ✅ Call Da
        //    return View(employees);
        //}

        // GET
        [HttpGet]
        public IActionResult EditEmployee(int id)
        {
            var emp = _employeeDa.GetEmployeeById(id);
            ViewBag.Departments = new SelectList(_employeeDa.GetDepartments(), "DepartmentId", "DepartmentName", emp.DepartmentId);

            if (emp == null)
            {
                return NotFound(); // prevents crash
            }
            return View(emp);
        }

        [HttpPost]
        public IActionResult EditEmployee(EmployeeModel emp)
        {
            Console.WriteLine("POST HIT"); // 👈 check if hitting
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                _employeeDa.UpdateEmployee(emp); // 👈 call Da method
                return RedirectToAction("Index");
            }
            // 👇 MUST reload dropdown
            ViewBag.Departments = new SelectList(_employeeDa.GetDepartments(), "DepartmentId", "DepartmentName", emp.DepartmentId);
            return View(emp);
        }
        
        [HttpPost]
        public IActionResult DeleteEmployee(int id)
        {
            _employeeDa.DeleteEmployee(id);
            return RedirectToAction("Index");
        }

        public IActionResult CompleteProfile()
        {
            var empIdStr = HttpContext.Session.GetString("EmployeeId");

            if (string.IsNullOrEmpty(empIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            int empId = Convert.ToInt32(empIdStr);

            var emp = _employeeDa.GetEmployeeByID(empId);

            return View(emp);
        }

        [HttpPost]
        public IActionResult CompleteProfile(EmployeeModel model)
        {
            var empIdStr = HttpContext.Session.GetString("EmployeeId");

            if (string.IsNullOrEmpty(empIdStr))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 🔐 Secure ID from session
            model.EmployeeId = Convert.ToInt32(empIdStr);

            _employeeDa.UpdateProfile(model);

            return RedirectToAction("Index", "Dashboard");
        }


    }
}
