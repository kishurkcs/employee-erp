namespace EmployeeApi.Models
{
    public class EmployeeLeave
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Type { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public Employee? Employee { get; set; }
    }
}
