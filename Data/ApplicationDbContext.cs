//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Data
    {
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            } 
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<DesignationModel> Designations { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
        public DbSet<LeaveManagementModel> Leaves { get; set; }
        public DbSet<LeaveTypeModel> LeaveTypes { get; set; }
        public DbSet<PayrollModel> Payroll { get; set; }
        public DbSet<AttendanceModel> Attendance { get; set; }
        public DbSet<AnnouncementModel> Announcements { get; set; }

    }
}

