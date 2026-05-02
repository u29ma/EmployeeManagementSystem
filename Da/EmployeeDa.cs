
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagementSystem.Da
{
    public class EmployeeDa
    {
        private readonly ApplicationDbContext _context;

        public EmployeeDa(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get All
        public List<EmployeeModel> GetAllEmployees()
        {
            var data = (from e in _context.Employees
                        join d in _context.Departments
                        on e.DepartmentId equals d.DepartmentId
                        select new EmployeeModel
                        {
                            EmployeeId = e.EmployeeId,
                            FirstName = e.FirstName,
                            LastName = e.LastName,
                            Salary = e.Salary,
                            Status = e.Status,
                            DepartmentId = e.DepartmentId,
                            DepartmentName = d.DepartmentName 
                        }).ToList();

            return data;
            //return _context.Employees.Where(e => e.Status).ToList(); //return _context.Employees.ToList();

        }

        // Insert
        public void AddEmployee(EmployeeModel emp, string email, string password)
        {
            // ✅ Step 1: Check if email already exists
            var exists = _context.Users.Any(u => u.Email == email);
            if (exists)
            {
                throw new Exception("Email already exists");
            }
            // Step 1: Create User
            var user = new UserModel
            {
                Email = email,
                Password = password,
                Username = emp.FirstName + " " + emp.LastName,
                Role = "Employee"
            };
            _context.Users.Add(user);
            _context.SaveChanges(); // 🔥 Important to get UserId

            // Step 2: Link Employee with User
            emp.UserId = user.UserId;
            emp.Status = true;   // ✅ Active

            _context.Employees.Add(emp);
            _context.SaveChanges(); // 🔥 gives EmployeeId

            // ✅ Step 4: UPDATE User with EmployeeId 
            user.EmployeeId = emp.EmployeeId;

            _context.Users.Update(user);
            _context.SaveChanges();

        }

        // Get by Id
        public EmployeeModel GetEmployeeById(int id)
        {
            return _context.Employees.Find(id);
        }

        // Update     
        public void UpdateEmployee(EmployeeModel emp)
        {
            var existingEmp = _context.Employees.FirstOrDefault(e => e.EmployeeId == emp.EmployeeId);

            if (existingEmp != null)
            {
                existingEmp.FirstName = emp.FirstName;
                existingEmp.LastName = emp.LastName;
                existingEmp.Phone = emp.Phone;
                existingEmp.Address = emp.Address;
                existingEmp.DepartmentId = emp.DepartmentId;
                existingEmp.Designation = emp.Designation;
                existingEmp.Salary = emp.Salary;
                existingEmp.Status = emp.Status;

                var user = _context.Users.Find(emp.UserId);


                if (user != null)
                {
                    user.Username = emp.FirstName + " " + emp.LastName;
                }

                _context.SaveChanges();
            }
        }
        public void UpdateProfile(EmployeeModel model)
        {
            var emp = _context.Employees.Find(model.EmployeeId);

            if (emp != null)
            {
                emp.FirstName = model.FirstName;
                emp.LastName = model.LastName;
                emp.Phone = model.Phone;
                emp.DepartmentId = model.DepartmentId;
                emp.Designation = model.Designation;
                emp.Salary = model.Salary;
                emp.IsProfileComplete = true;

                _context.SaveChanges();
            }
        }

        // Delete      
        public void DeleteEmployee(int id)
        {
            var emp = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            if (emp != null)
            {
                emp.Status = false;  // 👈 mark as deleted
                _context.Employees.Update(emp);
                _context.SaveChanges();
            }
        }

        // ✅ GET EMPLOYEE BY ID
        public EmployeeModel GetEmployeeByID(int empId)
        {
            var data = (from e in _context.Employees
                        join d in _context.Departments
                        on e.DepartmentId equals d.DepartmentId
                        where e.EmployeeId == empId
                        select new EmployeeModel
                        {
                            EmployeeId = e.EmployeeId,
                            FirstName = e.FirstName,
                            LastName = e.LastName,
                            DepartmentId = e.DepartmentId,
                            DepartmentName = d.DepartmentName,
                            IsProfileComplete = e.IsProfileComplete,
                            Salary = e.Salary,
                            Status = e.Status
                        }).FirstOrDefault();

            return data;
            //return _context.Employees.FirstOrDefault(e => e.EmployeeId == empId);
        }

        public List<DepartmentModel> GetDepartments()
        {
            return _context.Departments.ToList();
        }

    }
}

