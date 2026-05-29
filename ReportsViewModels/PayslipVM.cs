namespace EmployeeManagementSystem.ReportsViewModels
{
    public class PayslipVM
    {
        public int PayrollId { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Bonus { get; set; }
        public decimal Deduction { get; set; }
        public decimal NetSalary { get; set; }
        public string Month { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}