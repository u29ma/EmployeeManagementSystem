using EmployeeManagementSystem.Da;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class LeaveManagementModel
    {
            [Key]
            public int LeaveId { get; set; }

            [Required]
            public int EmployeeId { get; set; }

        //public int LeaveTypeId { get; set; }
        [Required(ErrorMessage = "Please select leave type")]
        public int? LeaveTypeId { get; set; }

        [Required]
            [DataType(DataType.Date)]
            public DateTime StartDate { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime EndDate { get; set; }

            [Required]
            [StringLength(250)]
            public string Reason { get; set; }

            public string Status { get; set; } = "Pending";

            public DateTime AppliedDate { get; set; } = DateTime.Now;

            // 🔗 Navigation Properties (optional but useful)
            [ForeignKey("EmployeeId")]
        [ValidateNever]
        public EmployeeModel Employee { get; set; }

            [ForeignKey("LeaveTypeId")]
        [ValidateNever]
        public LeaveTypeModel LeaveType { get; set; }
        }
    public class LeaveTypeModel
    {
        [Key]
        //public int LeaveTypeId { get; set; }
        [Required(ErrorMessage = "Please select leave type")]
        public int? LeaveTypeId { get; set; }
        [Required]
        public string LeaveName { get; set; }
        public int MaxDays { get; set; }
    }
}


