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

            var record = _context.Attendances
                .FirstOrDefault(a => a.EmployeeId == empId && a.Date == today);

            if (record == null)
            {
                _context.Attendances.Add(new AttendanceModel
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

            var record = _context.Attendances
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
            return _context.Attendances
                .Where(a => a.EmployeeId == empId)
                .OrderByDescending(a => a.Date)
                .ToList();
        }

        // 👉 Admin view all
        public List<AttendanceModel> GetAllAttendance(string search, DateTime? date, string status)
        {
            var query = _context.Attendances
                .Include(a => a.Employee)
                .AsQueryable();

            // 🔍 Search (Employee Name)
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(a =>
                    (a.Employee.FirstName + " " + a.Employee.LastName).Contains(search) ||
                    a.Employee.FirstName.Contains(search) ||
                    a.Employee.LastName.Contains(search)
                );
            }

            // 📅 Date filter
            if (date.HasValue)
            {
                query = query.Where(a => a.Date.Date == date.Value.Date);
            }

            // 🔄 Status filter
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(a => a.Status == status);
            }

            return query
                .OrderByDescending(a => a.Date)
                .ToList();
        }

        // Admin Dashboard
        public List<AttendanceModel> GetPresentToday()
        {
            var today = DateTime.Today;

            return _context.Attendances
                .Include(a => a.Employee)
                .Where(a => a.Date == today && a.Status == "Present")
                .ToList();
        }

    }
}
