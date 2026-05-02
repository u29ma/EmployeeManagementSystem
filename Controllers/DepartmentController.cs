using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.Da;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly DepartmentDa _departmentDa;

        public DepartmentController(DepartmentDa departDa)
        {
            _departmentDa = departDa;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("AccessDenied", "Home");

            var data = _departmentDa.GetAllDepartments();
            return View(data);
        }

        // ➕ Add (GET)
        public IActionResult AddDepartment()
        {
            return View();
        }

        // ➕ Add (POST)
        [HttpPost]
        public IActionResult AddDepartment(DepartmentModel dept)
        {
            if (ModelState.IsValid)
            {
                _departmentDa.AddDepartment(dept);
                return RedirectToAction("Index");
            }
            return View(dept);
        }
        // ✏️ Edit (GET)
        public IActionResult EditDepartment(int id)
        {
            var dept = _departmentDa.GetDepartmentById(id);
            if (dept == null) return NotFound();

            return View(dept);
        }
        // ✏️ Edit (POST)
        [HttpPost]
        public IActionResult EditDepartment(DepartmentModel dept)
        {
            if (ModelState.IsValid)
            {
                _departmentDa.UpdateDepartment(dept);
                return RedirectToAction("DepartmentList");
            }
            return View(dept);
        }
        // ❌ Delete
        [HttpPost]
        public IActionResult DeleteDepartment(int id)
        {
            try
            {
                _departmentDa.DeleteDepartment(id);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("DepartmentList");
        }
    }
}
