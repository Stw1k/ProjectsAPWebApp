using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectsAPWebApp.Models;

namespace ProjectsAPWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ProjectAPIContext _context;

        public ProjectsController(ProjectAPIContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        
        public async Task<IActionResult> PutProject(int id, [FromBody] Project project)
        {
            if (id != project.Id)
            {
                return BadRequest("Проект з таким Id не знайден.");
            }

            var existingProject = await _context.Projects
                .Include(p => p.Admin)
                .Include(p => p.Worker)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existingProject == null)
            {
                return NotFound("Проект не знайден.");
            }

            
            existingProject.AdminId = project.AdminId;
            existingProject.WorkerId = project.WorkerId;

            
            var admin = await _context.Admins.FindAsync(project.AdminId);
            var worker = await _context.Workers.FindAsync(project.WorkerId);

            if (admin == null || worker == null)
            {
                return BadRequest("Немає працівника або адміністратора з таким Id.");
            }

            existingProject.Admin = admin;
            existingProject.Worker = worker;

            
            

            _context.Entry(existingProject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok("Дані успішно змінились.");
        }

        

        

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            if (project == null)
            {
                return BadRequest("");
            }

            
            var admin = await _context.Admins.FindAsync(project.AdminId);
            var worker = await _context.Workers.FindAsync(project.WorkerId);

            if (admin == null || worker == null)
            {
                return BadRequest("Немає працівника або адміністратора з таким Id.");
            }

            
            project.Admin = admin;
            project.Worker = worker;

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return Ok(project);
        }


        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound("Проект не знайден.");
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return Ok("Проект успішно видалився.");
        }
       

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
