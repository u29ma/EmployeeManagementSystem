using EmployeeManagementSystem.Da;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Controllers
{
    public class LeaveManagementController : Controller
    {
        private readonly LeaveManagementDa _leaveDa;

        public LeaveManagementController(LeaveManagementDa leaveDa)
        {
            _leaveDa = leaveDa;
        }

        // ================= Admin Dashboard =================
        public IActionResult OnLeaveToday()
        {
            var leaves = _leaveDa.GetOnLeaveToday();
            return View(leaves);
        }

        public IActionResult PendingApprovals()
        {
            var leaves = _leaveDa.GetPendingApprovals();
            return View(leaves);
        }

        public IActionResult ApprovedLeaves()
        {
            var data = _leaveDa.GetApprovedLeaves();
            return View(data);
        }

        // ================= HOME =================
        public IActionResult Index()
        {
            return View();
        }

        // ================= EMPLOYEE SIDE =================

        // 👉 Apply Leave (GET)
        public IActionResult ApplyLeave()
        {
            ViewBag.LeaveTypes = _leaveDa.GetLeaveTypes(); // dropdown
            return View();
        }

        // 👉 Apply Leave (POST)
        [HttpPost]
        public IActionResult ApplyLeave(LeaveManagementModel leave)
        {
            // get logged-in employee id from session
            leave.EmployeeId = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));

            foreach (var item in ModelState)
            {
                foreach (var error in item.Value.Errors)
                {
                    Console.WriteLine(item.Key + " : " + error.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                _leaveDa.ApplyLeave(leave);

                return RedirectToAction("MyLeaves");
            }

            // reload dropdown if validation fails
            ViewBag.LeaveTypes = _leaveDa.GetLeaveTypes();
            return View(leave);
        }

        // 👉 Employee can see their leaves
        public IActionResult MyLeaves()
        {
            int empId = Convert.ToInt32(HttpContext.Session.GetString("EmployeeId"));
            var leaves = _leaveDa.GetLeavesEmployeeId(empId);
            return View(leaves);
        }

        // ================= ADMIN SIDE =================

        // 👉 View all leave requests
        public IActionResult AllLeaves()
        {
            var leaves = _leaveDa.GetAllLeaves();
            return View(leaves);
        }

        // 👉 Approve Leave
        public IActionResult Approve(int id)
        {
            _leaveDa.ApproveLeave(id);
            return RedirectToAction("AllLeaves");
        }

        // 👉 Reject Leave
        public IActionResult Reject(int id)
        {
            _leaveDa.RejectLeave(id);
            return RedirectToAction("AllLeaves");
        }

        // 👉 View single leave details (optional)
        public IActionResult Details(int id)
        {
            var leave = _leaveDa.GetLeaveById(id);

            if (leave == null)
            {
                return NotFound();
            }

            return View(leave);
        }
    }
}
