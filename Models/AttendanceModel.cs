using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class AttendanceModel
    {
            [Key]
            public int AttendanceId { get; set; }

            public int EmployeeId { get; set; }

            [DataType(DataType.Date)]
            public DateTime Date { get; set; } = DateTime.Today;

            public TimeSpan? CheckIn { get; set; }
            public TimeSpan? CheckOut { get; set; }

            public string Status { get; set; } = "Present"; // Present / Absent

            [ForeignKey("EmployeeId")]
            [ValidateNever]
            public EmployeeModel Employee { get; set; }
        }
    }
