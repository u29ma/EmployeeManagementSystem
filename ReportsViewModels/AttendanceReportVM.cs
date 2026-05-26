namespace EmployeeManagementSystem.ReportsViewModels
{
    public class AttendanceReportVM
    {
        public int AttendanceId { get; set; }

        public string EmployeeName { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan? CheckInTime { get; set; }

        public TimeSpan? CheckOutTime { get; set; }

        public string Status { get; set; }
    }
}
