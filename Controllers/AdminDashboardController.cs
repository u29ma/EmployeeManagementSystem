using EmployeeManagementSystem.Da;
using EmployeeManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly DashboardDa _dashboardDa;

        public AdminDashboardController(DashboardDa dashboardDa)
        {
            _dashboardDa = dashboardDa;
        }

        public IActionResult Index()
        {
            var data = _dashboardDa.GetAdminDashboardData();
            return View(data);
        }

        [HttpGet]
        public JsonResult GetLeaveStatusChart()
        {
            var data = _dashboardDa.GetLeaveStatusChart();

            return Json(new
            {
                approved = data.approved,
                pending = data.pending
            });
        }

        [HttpGet]
        public JsonResult GetMonthlyChart()
        {
            var data = _dashboardDa.GetMonthlyChart();
            return Json(data);
        }


    }
}
