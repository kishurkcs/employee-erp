namespace EmployeeApi.Models
{
    public class EmployeeProfile
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public string Address { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public Employee? Employee { get; set; }
    }
}
