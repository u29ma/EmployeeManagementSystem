using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PayrollApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =====================================
        // GET ALL PAYROLL
        // =====================================
        [HttpGet]
        public IActionResult GetPayrolls()
        {
            var payrolls = from p in _context.Payrolls
                           join e in _context.Employees
                           on p.EmployeeId equals e.EmployeeId

                           select new
                           {
                               p.PayrollId,

                               EmployeeName =
                                   e.FirstName + " " + e.LastName,

                               p.SalaryMonth,
                               p.SalaryYear,
                               p.BasicSalary,
                               p.Bonus,
                               p.Deductions,
                               p.NetSalary,
                               p.Status
                           };

            return Ok(payrolls);
        }

        // =====================================
        // GET PAYROLL BY ID
        // =====================================
        [HttpGet("{id}")]
        public IActionResult GetPayrollById(int id)
        {
            var payroll = _context.Payrolls
                .FirstOrDefault(x => x.PayrollId == id);

            if (payroll == null)
            {
                return NotFound(new
                {
                    message = "Payroll Not Found"
                });
            }

            return Ok(payroll);
        }

        // =====================================
        // CREATE PAYROLL
        // =====================================
        [HttpPost]
        public IActionResult CreatePayroll(PayrollModel model)
        {
            model.Status = "Pending";

            _context.Payrolls.Add(model);

            _context.SaveChanges();

            return Ok(new
            {
                message = "Payroll Created Successfully"
            });
        }

        //-----------------------------------------------------------------------------------
        [HttpPost("CalculateSalary")]
        public IActionResult CalculateSalary(PayrollModel model)
        {
            model.NetSalary =
                model.BasicSalary +
                model.Bonus -
                model.Deductions;

            return Ok(new
            {
                BasicSalary = model.BasicSalary,
                Bonus = model.Bonus,
                Deduction = model.Deductions,
                NetSalary = model.NetSalary
            });
        }
        // =====================================
        // APPROVE PAYROLL
        // =====================================
        [HttpPut("Approve/{id}")]
        public IActionResult ApprovePayroll(int id)
        {
            var payroll = _context.Payrolls
                .FirstOrDefault(x => x.PayrollId == id);

            if (payroll == null)
            {
                return NotFound(new
                {
                    message = "Payroll Not Found"
                });
            }

            payroll.Status = "Paid";

            _context.SaveChanges();

            return Ok(new
            {
                message = "Payroll Approved Successfully"
            });
        }

        // =====================================
        // HOLD PAYROLL
        // =====================================
        [HttpPut("Hold/{id}")]
        public IActionResult HoldPayroll(int id)
        {
            var payroll = _context.Payrolls
                .FirstOrDefault(x => x.PayrollId == id);

            if (payroll == null)
            {
                return NotFound(new
                {
                    message = "Payroll Not Found"
                });
            }

            payroll.Status = "Hold";

            _context.SaveChanges();

            return Ok(new
            {
                message = "Payroll Put On Hold"
            });
        }

        // =====================================
        // DELETE PAYROLL
        // =====================================
        [HttpDelete("{id}")]
        public IActionResult DeletePayroll(int id)
        {
            var payroll = _context.Payrolls
                .FirstOrDefault(x => x.PayrollId == id);

            if (payroll == null)
            {
                return NotFound(new
                {
                    message = "Payroll Not Found"
                });
            }

            _context.Payrolls.Remove(payroll);

            _context.SaveChanges();

            return Ok(new
            {
                message = "Payroll Deleted Successfully"
            });
        }
    }
}