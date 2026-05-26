using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LeaveApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // =====================================
        // GET ALL LEAVES
        // =====================================
        [HttpGet]
        public IActionResult GetLeaves()
        {
            var leaves = _context.Leaves.ToList();

            return Ok(leaves);
        }

        // =====================================
        // GET LEAVE BY ID
        // =====================================
        [HttpGet("{id}")]
        public IActionResult GetLeaveById(int id)
        {
            var leave = _context.Leaves
                .FirstOrDefault(x => x.LeaveId == id);

            if (leave == null)
            {
                return NotFound(new
                {
                    message = "Leave Not Found"
                });
            }

            return Ok(leave);
        }

        // =====================================
        // APPLY LEAVE
        // =====================================
        [HttpPost]
        public IActionResult ApplyLeave(LeaveManagementModel model)
        {
            model.Status = "Pending";

            _context.Leaves.Add(model);

            _context.SaveChanges();

            return Ok(new
            {
                message = "Leave Applied Successfully"
            });
        }

        // =====================================
        // APPROVE LEAVE
        // =====================================
        [HttpPut("Approve/{id}")]
        public IActionResult ApproveLeave(int id)
        {
            var leave = _context.Leaves
                .FirstOrDefault(x => x.LeaveId == id);

            if (leave == null)
            {
                return NotFound(new
                {
                    message = "Leave Not Found"
                });
            }

            leave.Status = "Approved";

            _context.SaveChanges();

            return Ok(new
            {
                message = "Leave Approved Successfully"
            });
        }

        // =====================================
        // REJECT LEAVE
        // =====================================
        [HttpPut("Reject/{id}")]
        public IActionResult RejectLeave(int id)
        {
            var leave = _context.Leaves
                .FirstOrDefault(x => x.LeaveId == id);

            if (leave == null)
            {
                return NotFound(new
                {
                    message = "Leave Not Found"
                });
            }

            leave.Status = "Rejected";

            _context.SaveChanges();

            return Ok(new
            {
                message = "Leave Rejected Successfully"
            });
        }

        // =====================================
        // DELETE LEAVE
        // =====================================
        [HttpDelete("{id}")]
        public IActionResult DeleteLeave(int id)
        {
            var leave = _context.Leaves
                .FirstOrDefault(x => x.LeaveId == id);

            if (leave == null)
            {
                return NotFound(new
                {
                    message = "Leave Not Found"
                });
            }

            _context.Leaves.Remove(leave);

            _context.SaveChanges();

            return Ok(new
            {
                message = "Leave Deleted Successfully"
            });
        }
    }
}