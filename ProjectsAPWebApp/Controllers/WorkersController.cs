using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectsAPWebApp.Models;

namespace ProjectsAPWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly ProjectAPIContext _context;

        public WorkersController(ProjectAPIContext context)
        {
            _context = context;
        }

        // GET: api/Workers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Worker>>> GetWorkers()
        {
            return await _context.Workers.ToListAsync();
        }

        // GET: api/Workers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Worker>> GetWorker(int id)
        {
            var worker = await _context.Workers.FindAsync(id);

            if (worker == null)
            {
                return NotFound();
            }

            return worker;
        }

        // PUT: api/Workers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorker(int id, [FromBody] Worker updatedWorker)
        {
            if (id != updatedWorker.Id)
            {
                return BadRequest("Працівник з таким Id не знайден.");
            }

            var existingWorker = await _context.Workers.FindAsync(id);
            if (existingWorker == null)
            {
                return NotFound("Працівник не знайден.");
            }

            
            existingWorker.Name = updatedWorker.Name;
            existingWorker.Position = updatedWorker.Position;
            existingWorker.Exp = updatedWorker.Exp;
            existingWorker.CompanyId = updatedWorker.CompanyId;

            _context.Entry(existingWorker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

       

    // POST: api/Workers
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
        public async Task<IActionResult> CreateWorker([FromBody] Worker workerDto)
        {
            if (workerDto == null)
            {
                return BadRequest("Invalid worker data.");
            }

            
            if (workerDto.CompanyId <= 0)
            {
                return BadRequest("Invalid company ID.");
            }

            
            var company = await _context.Companys.FindAsync(workerDto.CompanyId);
            if (company == null)
            {
                return NotFound("Company not found."); 
            }

            var worker = new Worker
            {
                Name = workerDto.Name,
                Position = workerDto.Position,
                Exp = workerDto.Exp,
                CompanyId = workerDto.CompanyId,
                Company = company 
            };

            _context.Workers.Add(worker);
            await _context.SaveChangesAsync(); 

            return Ok(worker);
        }

        // DELETE: api/Workers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorker(int id)
        {
            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
            {
                return NotFound("Працівник не знайден.");
            }

            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();

            return Ok("Працівник успішно видален.");
        }

        private bool WorkerExists(int id)
        {
            return _context.Workers.Any(e => e.Id == id);
        }
    }
}
