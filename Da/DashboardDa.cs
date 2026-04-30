using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;

namespace EmployeeManagementSystem.Da
{
    public class DashboardDa
    {
        private readonly ApplicationDbContext _context;

        public DashboardDa(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Admin Dashboard Data
        public DashboardModel GetAdminDashboardData()
        {
            return new DashboardModel
            {
                TotalEmployees = _context.Employees.Count(),

                OnLeaveToday = _context.Leaves
                .Count(l => l.Status == "Approved" && l.StartDate <= DateTime.Today  && l.EndDate >= DateTime.Today),

                //OnLeaveToday = _context.Leaves
                //    .Count(l => l.Date == DateTime.Today && l.Status == "Approved"),

                TotalDepartments = _context.Departments.Count(),

                PendingApprovals = _context.Leaves
                    .Count(l => l.Status == "Pending"),

                PresentToday = _context.Attendance
                    .Count(a => a.Date == DateTime.Today && a.Status == "Present"),

                TotalAnnouncements = _context.Announcements.Count(),

                ApprovedLeaves = _context.Leaves
                    .Count(l => l.Status == "Approved"),

                PendingPayroll = _context.Payroll
                    .Count(p => p.Status == "Pending")
            };
        }
        public (int approved, int pending) GetLeaveStatusChart()
        {
            var approved = _context.Leaves.Count(l => l.Status == "Approved");
            var pending = _context.Leaves.Count(l => l.Status == "Pending");

            return (approved, pending);
        }
        public List<object> GetMonthlyChart()
        {
            var data = _context.Leaves
                .Where(l => l.Status == "Approved")
                .AsEnumerable()
                .GroupBy(l => l.StartDate.Month)
                .Select(g => new
                {
                    month = System.Globalization.CultureInfo
                                .CurrentCulture
                                .DateTimeFormat
                                .GetMonthName(g.Key),

                    count = g.Count()
                })
                .OrderBy(x => DateTime.ParseExact(x.month, "MMMM", null).Month)
                .ToList<object>();

            return data;
        }

        // ✅ Employee Dashboard Data
        public EmployeeDashboardModel GetEmployeeDashboard(int empId)
        {
            return new EmployeeDashboardModel
            {
                TotalLeaves = _context.Leaves.Count(l => l.EmployeeId == empId),
                ApprovedLeaves = _context.Leaves.Count(l => l.EmployeeId == empId && l.Status == "Approved"),
                PendingLeaves = _context.Leaves.Count(l => l.EmployeeId == empId && l.Status == "Pending"),
                PresentDays = _context.Attendance.Count(a => a.EmployeeId == empId && a.Status == "Present")
            };
        }
        // 🔹 Leave Chart
        public object GetEmployeeLeaveChart(int empId)
        {
            var approved = _context.Leaves.Count(l => l.EmployeeId == empId && l.Status == "Approved");
            var pending = _context.Leaves.Count(l => l.EmployeeId == empId && l.Status == "Pending");

            return new { approved, pending };
        }
        // 🔹 Attendance Chart
        public object GetAttendanceChart(int empId)
        {
            var present = _context.Attendance
                .Count(a => a.EmployeeId == empId && a.Status == "Present");

            var total = _context.Attendance
                .Count(a => a.EmployeeId == empId);

            var absent = total - present;

            return new { present, absent };
        }
        // 🔹 Salary Chart
        public object GetSalaryChart(int empId)
        {
            var paid = _context.Payroll
                .Count(p => p.EmployeeId == empId && p.Status == "Paid");

            var pending = _context.Payroll
                .Count(p => p.EmployeeId == empId && p.Status == "Pending");

            return new { paid, pending };
        }

    }
}
    

