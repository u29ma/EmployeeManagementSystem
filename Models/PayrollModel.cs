using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models;


public class PayrollModel
{
    [Key]
    public int PayrollId { get; set; }

    public int EmployeeId { get; set; }

    [ForeignKey("EmployeeId")]
    public EmployeeModel? Employee { get; set; }

    public decimal BasicSalary { get; set; }
    public decimal Bonus { get; set; }
    public decimal Deductions { get; set; }

    public string Status { get; set; } = "Pending";

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public decimal NetSalary { get; set; }

    public string SalaryMonth { get; set; }

    [Required]
    public int? SalaryYear { get; set; }
    public DateTime PaymentDate { get; set; }
}

