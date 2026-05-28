using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL EMPLOYEES
        // =========================
        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employees = _context.Employees.ToList();

            return Ok(employees);
        }

        // =========================
        // GET EMPLOYEE BY ID
        // =========================
        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            var employee = _context.Employees
                .FirstOrDefault(x => x.EmployeeId == id);

            if (employee == null)
            {
                return NotFound(new
                {
                    message = "Employee Not Found"
                });
            }

            return Ok(employee);
        }

        // =========================
        // ADD EMPLOYEE
        // =========================
        [HttpPost]
        public IActionResult AddEmployee(EmployeeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Employees.Add(model);

            _context.SaveChanges();

            return Ok(new
            {
                message = "Employee Added Successfully"
            });
        }

        // =========================
        // UPDATE EMPLOYEE
        // =========================
        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, EmployeeModel model)
        {
            var employee = _context.Employees
                .FirstOrDefault(x => x.EmployeeId == id);

            if (employee == null)
            {
                return NotFound(new
                {
                    message = "Employee Not Found"
                });
            }

            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.Phone = model.Phone;
            employee.DesignationId = model.DesignationId;

            _context.SaveChanges();

            return Ok(new
            {
                message = "Employee Updated Successfully"
            });
        }

        // =========================
        // DELETE EMPLOYEE
        // =========================
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var employee = _context.Employees
                .FirstOrDefault(x => x.EmployeeId == id);

            if (employee == null)
            {
                return NotFound(new
                {
                    message = "Employee Not Found"
                });
            }

            _context.Employees.Remove(employee);

            _context.SaveChanges();

            return Ok(new
            {
                message = "Employee Deleted Successfully"
            });
        }
    }
}