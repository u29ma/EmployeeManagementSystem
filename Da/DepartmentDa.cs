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

        //// Get by Id
        //public DepartmentModel GetDepartmentById(int id)
        //{
        //    return _context.Departments.Find(id);
        //}

        // Get by ID
        public DepartmentModel GetDepartmentById(int id)
        {
            return _context.Departments.FirstOrDefault(d => d.DepartmentId == id);
        }

        // Add
        public void AddDepartment(DepartmentModel dept)
        {
            _context.Departments.Add(dept);
            _context.SaveChanges();
        }
        // Update
        public void UpdateDepartment(DepartmentModel dept)
        {
            _context.Departments.Update(dept);
            _context.SaveChanges();
        }
        // Delete (SAFE)
        public void DeleteDepartment(int id)
        {
            bool isUsed = _context.Employees.Any(e => e.DepartmentId == id);

            if (isUsed)
                throw new Exception("Department is assigned to employees!");

            var dept = _context.Departments.Find(id);

            if (dept != null)
            {
                _context.Departments.Remove(dept);
                _context.SaveChanges();
            }
        }

    }
}
