using EmployeeApi.Data;
using EmployeeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return Ok(employees);
        }

        // GET: api/Employee/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found");
            }
            
            return Ok(employee);
        }

        // POST: api/Employee
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            // Don't send ID - database auto-generates
            employee.Id = 0;
            
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            
            return Ok(employee);
        }

        // PUT: api/Employee/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            // Check if ID in URL matches ID in body
            if (id != employee.Id)
            {
                return BadRequest("ID in URL does not match ID in body");
            }
            
            // Check if employee exists
            var existingEmployee = await _context.Employees.FindAsync(id);
            if (existingEmployee == null)
            {
                return NotFound($"Employee with ID {id} not found");
            }
            
            // Update the properties of the existing entity
            existingEmployee.Name = employee.Name;
            existingEmployee.Department = employee.Department;
            
            // Mark as modified and save
            _context.Entry(existingEmployee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return Ok(existingEmployee);
        }
        // DELETE: api/Employee/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found");
            }
            
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            
            return Ok(new { message = $"Employee with ID {id} deleted successfully" });
        }
    }
}