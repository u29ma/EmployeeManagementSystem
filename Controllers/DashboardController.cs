using EmployeeManagementSystem.Da;
using EmployeeManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace EmployeeManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardDa _dashboardDa;

        public DashboardController(DashboardDa dashboardDa)
        {
            _dashboardDa = dashboardDa;
        }
        public IActionResult Index()
        {
            int empId = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));

            var data = _dashboardDa.GetEmployeeDashboard(empId);
            return View(data);
        }

        // Chart API
        public JsonResult GetLeaveChart()
        {
            int empId = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));

            var data = _dashboardDa.GetEmployeeLeaveChart(empId);
            return Json(data);
        }
        public JsonResult GetAttendanceChart()
        {
            int empId = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));

            var data = _dashboardDa.GetAttendanceChart(empId);

            return Json(data);
        }
        public JsonResult GetSalaryChart()
        {
            int empId = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));

            var data = _dashboardDa.GetSalaryChart(empId);

            return Json(data);
        }


    }
}
  

