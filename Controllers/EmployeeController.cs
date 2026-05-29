using EmployeeApi.Data;
using EmployeeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Controllers
{
    [Authorize]
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

        // GET: api/Employee/count
        [HttpGet("count")]
        public async Task<IActionResult> GetEmployeeCount()
        {
            var count = await _context.Employees.CountAsync();
            return Ok(new { count });
        }

        // GET: api/Employee/department/{department}
        [HttpGet("department/{department}")]
        public async Task<IActionResult> GetEmployeesByDepartment(string department)
        {
            var employees = await _context.Employees
                .Where(e => e.Department == department)
                .ToListAsync();

            return Ok(employees);
        }

        // GET: api/Employee/search?name=
        [HttpGet("search")]
        public async Task<IActionResult> SearchEmployeesByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Query parameter 'name' is required");
            }

            var employees = await _context.Employees
                .Where(e => e.Name.Contains(name))
                .ToListAsync();

            return Ok(employees);
        }

        // GET: api/Employee/{id}/exists
        [HttpGet("{id}/exists")]
        public async Task<IActionResult> EmployeeExists(int id)
        {
            var exists = await _context.Employees.AnyAsync(e => e.Id == id);
            return Ok(new { id, exists });
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
            employee.Id = 0;
            
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            
            return Ok(employee);
        }

        // ============================
        // Salary related endpoints
        // ============================

        // GET: api/Employee/{id}/salaries
        [HttpGet("{id}/salaries")]
        public async Task<IActionResult> GetEmployeeSalaries(int id)
        {
            var exists = await _context.Employees.AnyAsync(e => e.Id == id);
            if (!exists)
            {
                return NotFound($"Employee with ID {id} not found");
            }

            var salaries = await _context.EmployeeSalaries
                .Where(s => s.EmployeeId == id)
                .OrderByDescending(s => s.Year)
                .ThenByDescending(s => s.Month)
                .ToListAsync();

            return Ok(salaries);
        }

        // GET: api/Employee/{id}/salary/latest
        [HttpGet("{id}/salary/latest")]
        public async Task<IActionResult> GetLatestSalary(int id)
        {
            var exists = await _context.Employees.AnyAsync(e => e.Id == id);
            if (!exists)
            {
                return NotFound($"Employee with ID {id} not found");
            }

            var latestSalary = await _context.EmployeeSalaries
                .Where(s => s.EmployeeId == id)
                .OrderByDescending(s => s.Year)
                .ThenByDescending(s => s.Month)
                .FirstOrDefaultAsync();

            if (latestSalary == null)
            {
                return NotFound($"No salary records found for employee {id}");
            }

            return Ok(latestSalary);
        }

        // POST: api/Employee/{id}/salary
        [HttpPost("{id}/salary")]
        public async Task<IActionResult> AddSalaryRecord(int id, [FromBody] EmployeeSalary salary)
        {
            var exists = await _context.Employees.AnyAsync(e => e.Id == id);
            if (!exists)
            {
                return NotFound($"Employee with ID {id} not found");
            }

            salary.Id = 0;
            salary.EmployeeId = id;

            _context.EmployeeSalaries.Add(salary);
            await _context.SaveChangesAsync();

            return Ok(salary);
        }

        // ============================
        // Leave management endpoints
        // ============================

        // GET: api/Employee/{id}/leaves
        [HttpGet("{id}/leaves")]
        public async Task<IActionResult> GetLeaves(int id)
        {
            var exists = await _context.Employees.AnyAsync(e => e.Id == id);
            if (!exists)
            {
                return NotFound($"Employee with ID {id} not found");
            }

            var leaves = await _context.EmployeeLeaves
                .Where(l => l.EmployeeId == id)
                .OrderByDescending(l => l.StartDate)
                .ToListAsync();

            return Ok(leaves);
        }

        // POST: api/Employee/{id}/leaves
        [HttpPost("{id}/leaves")]
        public async Task<IActionResult> ApplyLeave(int id, [FromBody] EmployeeLeave leave)
        {
            var exists = await _context.Employees.AnyAsync(e => e.Id == id);
            if (!exists)
            {
                return NotFound($"Employee with ID {id} not found");
            }

            leave.Id = 0;
            leave.EmployeeId = id;

            if (leave.EndDate < leave.StartDate)
            {
                return BadRequest("EndDate cannot be earlier than StartDate");
            }

            _context.EmployeeLeaves.Add(leave);
            await _context.SaveChangesAsync();

            return Ok(leave);
        }

        // ============================
        // Profile management endpoints
        // ============================

        // GET: api/Employee/{id}/profile
        [HttpGet("{id}/profile")]
        public async Task<IActionResult> GetProfile(int id)
        {
            var exists = await _context.Employees.AnyAsync(e => e.Id == id);
            if (!exists)
            {
                return NotFound($"Employee with ID {id} not found");
            }

            var profile = await _context.EmployeeProfiles
                .FirstOrDefaultAsync(p => p.EmployeeId == id);

            if (profile == null)
            {
                return NotFound($"Profile not found for employee {id}");
            }

            return Ok(profile);
        }

        // PUT: api/Employee/{id}/profile
        [HttpPut("{id}/profile")]
        public async Task<IActionResult> UpsertProfile(int id, [FromBody] EmployeeProfile profile)
        {
            var exists = await _context.Employees.AnyAsync(e => e.Id == id);
            if (!exists)
            {
                return NotFound($"Employee with ID {id} not found");
            }

            var existingProfile = await _context.EmployeeProfiles
                .FirstOrDefaultAsync(p => p.EmployeeId == id);

            if (existingProfile == null)
            {
                profile.Id = 0;
                profile.EmployeeId = id;

                _context.EmployeeProfiles.Add(profile);
                await _context.SaveChangesAsync();

                return Ok(profile);
            }

            existingProfile.Address = profile.Address;
            existingProfile.Phone = profile.Phone;
            existingProfile.Position = profile.Position;

            _context.Entry(existingProfile).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(existingProfile);
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