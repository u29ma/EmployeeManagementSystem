using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Da
{
    public class AttendanceDa
    {
        private readonly ApplicationDbContext _context;

        public AttendanceDa(ApplicationDbContext context)
        {
            _context = context;
        }

        // 👉 Check In
        public void CheckIn(int empId)
        {
            var today = DateTime.Today;

            var record = _context.Attendance
                .FirstOrDefault(a => a.EmployeeId == empId && a.Date == today);

            if (record == null)
            {
                _context.Attendance.Add(new AttendanceModel
                {
                    EmployeeId = empId,
                    Date = today,
                    CheckIn = DateTime.Now.TimeOfDay,
                    Status = "Present"
                });
            }

            _context.SaveChanges();
        }

        // 👉 Check Out
        public void CheckOut(int empId)
        {
            var today = DateTime.Today;

            var record = _context.Attendance
                .FirstOrDefault(a => a.EmployeeId == empId && a.Date == today);

            if (record != null)
            {
                record.CheckOut = DateTime.Now.TimeOfDay;
                _context.SaveChanges();
            }
        }

        // 👉 Employee attendance
        public List<AttendanceModel> GetEmployeeAttendance(int empId)
        {
            return _context.Attendance
                .Where(a => a.EmployeeId == empId)
                .OrderByDescending(a => a.Date)
                .ToList();
        }

        // 👉 Admin view all
        public List<AttendanceModel> GetAllAttendance()
        {
            return _context.Attendance
                .Include(a => a.Employee)
                .ToList();
        }

        // Admin Dashboard
        public List<AttendanceModel> GetPresentToday()
        {
            var today = DateTime.Today;

            return _context.Attendance
                .Include(a => a.Employee)
                .Where(a => a.Date == today && a.Status == "Present")
                .ToList();
        }

    }
}
