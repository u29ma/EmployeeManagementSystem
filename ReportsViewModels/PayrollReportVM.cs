namespace EmployeeManagementSystem.ReportsViewModels
{
    public class PayrollReportVM
    {
        public int PayrollId { get; set; }
        public string EmployeeName { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Bonus { get; set; }
        public decimal Deduction { get; set; }
        public decimal NetSalary { get; set; }
        public string SalaryMonth { get; set; }
        public int SalaryYear { get; set; }
        public string Status { get; set; }
    }
}
