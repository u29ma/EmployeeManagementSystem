using EmployeeManagementSystem.Da;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly AttendanceDa _attendanceDa;

        public AttendanceController(AttendanceDa attendanceDa)
        {
            _attendanceDa = attendanceDa;
        }

        public IActionResult PresentToday()
        {
            var data = _attendanceDa.GetPresentToday();
            return View(data);
        }
        // ================= EMPLOYEE =================

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CheckIn()
        {
            int empId = int.Parse(HttpContext.Session.GetString("EmployeeId"));
            _attendanceDa.CheckIn(empId);

            return RedirectToAction("MyAttendance");
        }

        public IActionResult CheckOut()
        {
            int empId = int.Parse(HttpContext.Session.GetString("EmployeeId"));
            _attendanceDa.CheckOut(empId);

            return RedirectToAction("MyAttendance");
        }

        public IActionResult MyAttendance()
        {
            int empId = int.Parse(HttpContext.Session.GetString("EmployeeId"));
            var data = _attendanceDa.GetEmployeeAttendance(empId);

            return View(data);
        }
        public IActionResult GetMyAttendance()
        {
            int empId = int.Parse(HttpContext.Session.GetString("EmployeeId"));

            var data = _attendanceDa.GetEmployeeAttendance(empId);

            return PartialView("_MyAttendancePartial", data);
        }


        // ================= ADMIN =================

        public IActionResult AllAttendance()
        {
            var data = _attendanceDa.GetAllAttendance();
            return View(data);
        }
    }
}
