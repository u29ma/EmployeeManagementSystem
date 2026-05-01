using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystem.Da;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Controllers
{
    public class AnnouncementController : Controller
    {
        private readonly AnnouncementDa _da;

        public AnnouncementController(AnnouncementDa da)
        {
            _da = da;
        }

        public IActionResult AnnouncementList()
        {
            var data = _da.GetAllAnnouncements();
            return View(data);
        }

        // 🔹 Admin - List
        public IActionResult Index()
        {
            var data = _da.GetAll();
            return View(data);
        }

        // 🔹 Admin - Create (GET)
        public IActionResult Create()
        {
            return View();
        }

        // 🔹 Admin - Create (POST)
        [HttpPost]
        public IActionResult Create(AnnouncementModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedAt = DateTime.Now;
                model.IsActive = true;
                model.CreatedBy = Convert.ToInt32(HttpContext.Session.GetString("UserId"));


                _da.Add(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // 🔹 Employee - View Announcements
        public IActionResult EmployeeAnnouncements()
        {
            string role = HttpContext.Session.GetString("Role");

            var data = _da.GetForEmployee(role);
            return View(data);
        }
    }
}

