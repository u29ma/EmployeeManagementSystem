using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagementSystem.Da
{
    public class DepartmentDa
    {
        private readonly ApplicationDbContext _context;

        public DepartmentDa(ApplicationDbContext context)
        {
            _context = context;
        }
        // Get All
        public List<DepartmentModel> GetAllDepartments()
        {
            return _context.Departments.ToList();
        }

        // Insert
        public void AddDepartment(DepartmentModel emp)
        {
            _context.Departments.Add(emp);
            _context.SaveChanges();
        }

        // Get by Id
        public DepartmentModel GetDepartmentById(int id)
        {
            return _context.Departments.Find(id);
        }
        // Update
        public void UpdateDepartment(DepartmentModel dep)
        {
            _context.Departments.Update(dep);
            _context.SaveChanges();
        }

        // Delete
        public void DeleteDepartment(int id)
        {
            var dept = _context.Departments.Find(id);
            if (dept != null)
            {
                _context.Departments.Remove(dept);
                _context.SaveChanges();
            }
        }

    }
}
