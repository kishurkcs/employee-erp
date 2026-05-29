namespace EmployeeApi.Models
{
    public class EmployeeSalary
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public decimal Amount { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public Employee? Employee { get; set; }
    }
}
