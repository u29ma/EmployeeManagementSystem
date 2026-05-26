using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class DesignationModel
    {
        [Key]
        public int DesignationId { get; set; }

        [Required]
        public string DesignationName { get; set; }

        // Foreign Key
        public int DepartmentId { get; set; }

        // Navigation Property
        [ForeignKey("DepartmentId")]
        public DepartmentModel Department { get; set; }
    }
}