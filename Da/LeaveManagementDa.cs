using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;


namespace EmployeeManagementSystem.Da
{
    public class LeaveManagementDa
    {
        private readonly ApplicationDbContext _context;

        public LeaveManagementDa(ApplicationDbContext context)
        {
            _context = context;
        }
        // ================= Admin Dashboard Details=================

        // ✅ On Leave Today
        public List<LeaveManagementModel> GetOnLeaveToday()
        {
            var today = DateTime.Today;

            return _context.Leaves
                .Include(l => l.Employee)   // 🔥 for Full Name
                .Include(l => l.LeaveType)
                .Where(l => l.Status == "Approved"
                    && l.StartDate <= today
                    && l.EndDate >= today)
                .ToList();
        }

        // ✅ Pending Approvals
        public List<LeaveManagementModel> GetPendingApprovals()
        {
            return _context.Leaves
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Where(l => l.Status == "Pending")
                .ToList();
        }

        public List<LeaveManagementModel> GetApprovedLeaves()
        {
            return _context.Leaves
                .Include(l => l.Employee)
                .Where(l => l.Status == "Approved")
                .ToList();
        }

        //-------------------------------------------------------------------

        public List<LeaveTypeModel> GetLeaveTypes()
        {
            return _context.LeaveTypes
                .OrderBy(l => l.LeaveName)
                .ToList();
        }

        // ================= APPLY LEAVE =================
        public void ApplyLeave(LeaveManagementModel leave)
        {
            leave.Status = "Pending";
            leave.AppliedDate = DateTime.Now;

            _context.Leaves.Add(leave);
            _context.SaveChanges();
        }

        // ================= GET ALL LEAVES (ADMIN) =================
        
        public List<LeaveManagementModel> GetAllLeaves(string search, string status)
        {
            var query = _context.Leaves
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .AsQueryable();

            // 🔍 Search by employee name
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(e =>
                    EF.Functions.Like(e.Employee.FirstName, "%" + search + "%") ||
                    EF.Functions.Like(e.Employee.LastName, "%" + search + "%") ||
                    (
                        EF.Functions.Like(e.Employee.FirstName, "%" + search.Split(' ')[0] + "%") &&
                        search.Contains(" ") &&
                        EF.Functions.Like(e.Employee.LastName, "%" + search.Split(' ').Last() + "%")
                    )
                );
            }

            // 📌 Filter by status
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(l => l.Status == status);
            }

            return query
                .OrderByDescending(l => l.LeaveId)
                .ToList();
        }

        // ================= GET EMPLOYEE LEAVES =================
        public List<LeaveManagementModel> GetLeavesByEmployee(int empId)
        {
            return _context.Leaves
                .Where(l => l.EmployeeId == empId)
                .Include(l => l.LeaveType)   // ✅ Important
                .ToList();
        }
        // 👉 Get leaves by EmployeeId
        public List<LeaveManagementModel> GetLeavesEmployeeId(int empId)
        {
            return _context.Leaves
                .Include(l => l.LeaveType) // for LeaveType name
                .Where(l => l.EmployeeId == empId)
                .ToList();
        }

        // ================= APPROVE LEAVE =================
        public void ApproveLeave(int id)
        {
            var leave = _context.Leaves.Find(id);

            if (leave != null)
            {
                leave.Status = "Approved";
                _context.SaveChanges();
            }
        }

        // ================= REJECT LEAVE =================
        public void RejectLeave(int id)
        {
            var leave = _context.Leaves.Find(id);

            if (leave != null)
            {
                leave.Status = "Rejected";
                _context.SaveChanges();
            }
        }

        // ================= GET LEAVE BY ID =================
        public LeaveManagementModel GetLeaveById(int id)
        {
            return _context.Leaves
                .Include(l => l.LeaveType)
                .FirstOrDefault(l => l.LeaveId == id);
        }
        public int GetUsedLeave(int empId)
        {
            return _context.Leaves
                .Where(x =>
                    x.EmployeeId == empId &&
                    x.Status == "Approved")
                .Count();
        }

        //public (int totalLeave, int usedLeave, int remainingLeave) GetLeaveBalance(int empId)
        //{
        //    int totalLeave = 20;
        //    int usedLeave = _context.Leaves.Where(x => x.EmployeeId == empId && x.Status == "Approved")
        //        .Count();
        //    int remainingLeave = totalLeave - usedLeave;

        //    return (totalLeave, usedLeave,remainingLeave);
        //}
    }
}

