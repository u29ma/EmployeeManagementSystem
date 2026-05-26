using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
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

        public string GetRoleName(int roleId)
        {
            return _context.Roles.Where(r => r.RoleId == roleId).Select(r => r.RoleName).FirstOrDefault();
        }

        public EmployeeModel GetEmployeeByUserId(int userId)
        {
            var data = (from e in _context.Employees
                        join d in _context.Departments
                        on e.DepartmentId equals d.DepartmentId
                        where e.UserId == userId
                        select new EmployeeModel
                        {
                            EmployeeId = e.EmployeeId,
                            UserId = e.UserId,
                            FirstName = e.FirstName,
                            LastName = e.LastName,
                            DepartmentId = e.DepartmentId,
                            DepartmentName = d.DepartmentName, 
                            Salary = e.Salary,
                            IsProfileComplete = e.IsProfileComplete,
                            Status = e.Status
                        }).FirstOrDefault();

            return data;
        }


        // ✅ REGISTER (User + Employee)
        public bool Register(RegisterModel model)
        {
            // 🔍 Check duplicate email
            if (_context.Users.Any(u => u.Email == model.Email))
            {
                return false;
            }

            // ✅ Save User
            var user = new UserModel
            {
                Username = model.Name,
                Email = model.Email,
                Password = model.Password,
                Role = "Employee"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // ✅ Save Employee
            var employee = new EmployeeModel
            {
                UserId = user.UserId,
                Email = user.Email,
                Status = true, // ✅ Active
                IsProfileComplete = false
            };

            _context.Employees.Add(employee);
            _context.SaveChanges();

            return true;
        }

        public EmployeeModel GetByEmail(string email)
        {
            return _context.Employees
                .FirstOrDefault(e => e.Email == email);
        }
        public bool ChangePassword(int employeeId, string currentPassword, string newPassword)
        {
            var user = _context.Employees
                .FirstOrDefault(x => x.EmployeeId == employeeId);

            if (user == null)
            {
                return false;
            }

            // Check current password
            if (user.Password != currentPassword)
            {
                return false;
            }

            // Update password
            user.Password = newPassword;

            _context.SaveChanges();

            return true;
        }
        public bool ResetPassword(string email, string newPassword)
        {
            var user = _context.Employees
                .FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                return false;
            }

            user.Password = newPassword;

            _context.SaveChanges();

            return true;
        }
    }
}
