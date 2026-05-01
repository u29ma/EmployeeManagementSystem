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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DepartmentModel dept)
        {
            _departmentDa.AddDepartment(dept);
            return RedirectToAction("Index");
        }
    }
}
