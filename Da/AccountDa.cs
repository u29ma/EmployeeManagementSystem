using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EmployeeManagementSystem.Da
{
    public class AccountDa
    {
        private readonly ApplicationDbContext _context;

        public AccountDa(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ LOGIN
        public UserModel ValidateUser(string email, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public string GetRoleName(int? roleId)
        {
            return _context.Roles.Where(r => r.RoleId == roleId).Select(r => r.RoleName).FirstOrDefault();
        }

        public EmployeeModel GetEmployeeByEmployeeId(int employeeId)
        {
            var data = (from e in _context.Employees
                        join u in _context.Users
                        on e.EmployeeId equals u.EmployeeId
                        join d in _context.Departments
                        on e.DepartmentId equals d.DepartmentId
                        join des in _context.Designations
                        on e.DesignationId equals des.DesignationId
                        join r in _context.Roles on e.RoleId equals r.RoleId

                        where e.EmployeeId == employeeId

                        select new EmployeeModel
                        {
                            EmployeeId = e.EmployeeId,
                            RoleId = r.RoleId,
                            RoleName = r.RoleName,
                            FirstName = e.FirstName,
                            LastName = e.LastName,
                            DepartmentId = e.DepartmentId,
                            DepartmentName = d.DepartmentName,
                            DesignationId = e.DesignationId,
                            DesignationName = des.DesignationName,
                            Salary = e.Salary,
                            Status = e.Status
                        }).FirstOrDefault();

            return data;
        }
        public string GetEmail(string email)
        {
            return _context.Users.Where(u => u.Email == email).Select(u => u.Email).FirstOrDefault();
        }
        public string GetEmailByEmployeeId(int employeeId)
        {
            return _context.Users
                .Where(u => u.EmployeeId == employeeId)
                .Select(u => u.Email)
                .FirstOrDefault();
        }
        public bool ChangePassword(int userId, string currentPassword, string newPassword)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserId == userId);

            if (user == null)
                return false;

            if (user.Password != currentPassword)
                return false;

            user.Password = newPassword;
            _context.SaveChanges();

            return true;
        }
        public bool ResetPassword(string email, string newPassword)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.Email == email);

            if (user == null)
                return false;

            user.Password = newPassword;
            _context.SaveChanges();

            return true;
        }
    }
}
