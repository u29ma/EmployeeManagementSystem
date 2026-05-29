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
            ViewBag.Designations = _employeeDa.GetDesignations();
            ViewBag.Roles = _employeeDa.GetRoles();
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
            ViewBag.Roles = _employeeDa.GetRoles();
            ViewBag.Designations = _employeeDa.GetDesignations();

            return View(emp);
          
        }
      
        // GET
        [HttpGet]
        public IActionResult EditEmployee(int id)
        {
            var emp = _employeeDa.GetEmployeeById(id);
            ViewBag.Departments = new SelectList(_employeeDa.GetDepartments(), "DepartmentId", "DepartmentName", emp.DepartmentId);
            ViewBag.Designations = new SelectList(_employeeDa.GetDesignations(), "DesignationId", "DesignationName", emp.DesignationId);

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
       
        public IActionResult Profile()
        {
            string employeeId = HttpContext.Session.GetString("EmployeeId");

            if (string.IsNullOrEmpty(employeeId))
            {
                return RedirectToAction("Login", "Account");
            }

            int id = Convert.ToInt32(employeeId);

            var model = _employeeDa.GetEmployeeProfile(id);

            return View(model);

        }
    }
}
