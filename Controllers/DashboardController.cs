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

            var data = _dashboardDa.GetEmployeeDashboardData(empId);

            return View(data);
        }
    }
}
  

