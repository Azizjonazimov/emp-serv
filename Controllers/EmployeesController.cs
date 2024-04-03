using FullStackApi.Data;
using FullStackApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace FullStackApi.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : Controller
    {
        private readonly FullStackDbContext _fullStackDbContext;

        public EmployeesController(FullStackDbContext fullStackDbContext)
        {
            _fullStackDbContext = fullStackDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _fullStackDbContext.Employees.Find(_ => true).ToListAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employeeRequest)
        {
            employeeRequest.Id = Guid.NewGuid();
            await _fullStackDbContext.Employees.InsertOneAsync(employeeRequest);

            return Ok(employeeRequest);
        }


        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetEmployee([FromRoute] Guid id)
        {
            var employee = await _fullStackDbContext.Employees.Find(x => x.Id == id).FirstOrDefaultAsync();

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }


        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, Employee updateEmployeeRequest)
        {
            var filter = Builders<Employee>.Filter.Eq(x => x.Id, id);
            var update = Builders<Employee>.Update
                .Set(x => x.Name, updateEmployeeRequest.Name)
                .Set(x => x.Email, updateEmployeeRequest.Email)
                .Set(x => x.Phone, updateEmployeeRequest.Phone)
                .Set(x => x.Salary, updateEmployeeRequest.Salary)
                .Set(x => x.Department, updateEmployeeRequest.Department);

            var result = await _fullStackDbContext.Employees.UpdateOneAsync(filter, update);

            if (result.ModifiedCount == 0)
            {
                return NotFound();
            }

            return Ok(updateEmployeeRequest);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            var filter = Builders<Employee>.Filter.Eq(x => x.Id, id);
            var result = await _fullStackDbContext.Employees.DeleteOneAsync(filter);

            if (result.DeletedCount == 0)
            {
                return NotFound();
            }

            return Ok();
        }

    }
}
