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

    }
}