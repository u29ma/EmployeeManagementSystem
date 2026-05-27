using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AttendanceApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ====================================
        // GET ALL ATTENDANCE
        // ====================================
        [HttpGet]
        public IActionResult GetAttendance()
        {
            var data = _context.Attendances.ToList();

            return Ok(data);
        }
        // ====================================
        // GET ATTENDANCE BY ID
        // ====================================
        [HttpGet("{id}")]
        public IActionResult GetAttendanceById(int id)
        {
            var attendance = _context.Attendances
                .FirstOrDefault(x => x.AttendanceId == id);

            if (attendance == null)
            {
                return NotFound(new
                {
                    message = "Attendance Not Found"
                });
            }

            return Ok(attendance);
        }

        // ====================================
        // CHECK IN
        // ====================================
        [HttpPost("CheckIn")]
        public IActionResult CheckIn(AttendanceModel model)
        {
            model.Date = DateTime.Today;

            model.CheckIn = DateTime.Now.TimeOfDay;

            model.Status = "Present";

            _context.Attendances.Add(model);

            _context.SaveChanges();

            return Ok(new
            {
                message = "Check-In Successful"
            });
        }


        // ====================================
        // CHECK OUT
        // ====================================
        [HttpPut("CheckOut/{id}")]
        public IActionResult CheckOut(int id)
        {
            var attendance = _context.Attendances
                .FirstOrDefault(x => x.AttendanceId == id);

            if (attendance == null)
            {
                return NotFound(new
                {
                    message = "Attendance Not Found"
                });
            }

            attendance.CheckOut = DateTime.Now.TimeOfDay;

            _context.SaveChanges();

            return Ok(new
            {
                message = "Check-Out Successful"
            });
        }

        // ====================================
        // DELETE ATTENDANCE
        // ====================================
        [HttpDelete("{id}")]
        public IActionResult DeleteAttendance(int id)
        {
            var attendance = _context.Attendances
                .FirstOrDefault(x => x.AttendanceId == id);

            if (attendance == null)
            {
                return NotFound(new
                {
                    message = "Attendance Not Found"
                });
            }

            _context.Attendances.Remove(attendance);

            _context.SaveChanges();

            return Ok(new
            {
                message = "Attendance Deleted Successfully"
            });
        }
    }
}
