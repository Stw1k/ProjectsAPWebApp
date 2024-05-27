using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using ProjectsAPWebApp.Models;

namespace ProjectsAPWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ProjectAPIContext _context;

        public CompaniesController(ProjectAPIContext context)
        {
            _context = context;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanys()
        {
            return await _context.Companys.ToListAsync();
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var company = await _context.Companys.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] Company updatedCompany)
        {
            if (id != updatedCompany.Id)
            {
                return BadRequest("Компанія з таким Id не знайдена.");
            }

            var existingCompany = await _context.Companys.FindAsync(id);
            if (existingCompany == null)
            {
                return NotFound("Компанія не знайдена.");
            }

            
            existingCompany.Name = updatedCompany.Name;
            existingCompany.Description = updatedCompany.Description;

            _context.Entry(existingCompany).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] Company company)
        {
            if (company == null)
            {
                return BadRequest("");
            }

            _context.Companys.Add(company);
            await _context.SaveChangesAsync();

            return Ok(company);
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Companys.FindAsync(id);
            if (company == null)
            {
                return NotFound("Компанія не знайдена.");
            }

            _context.Companys.Remove(company);
            await _context.SaveChangesAsync();

            return Ok("Компанія успішно видалена.");
        }

        private bool CompanyExists(int id)
        {
            return _context.Companys.Any(e => e.Id == id);
        }
    }
}
