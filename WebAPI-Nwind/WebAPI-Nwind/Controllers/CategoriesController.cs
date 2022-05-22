using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_Nwind.Data;
using WebAPI_Nwind.Models;

namespace WebAPI_Nwind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public CategoriesController(NorthwindContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("movimientos")]
        public IEnumerable<Object> GetCategoriesMovements(int IdEmpleado)
        {
            return _context.Categories
                
                .Select(c => new
                {
                    Id = c.CategoryId,
                    Category = c.CategoryName
                })
                
                .Join(_context.Products,
                    c => c.Id,
                    p => p.CategoryId,
                    (c, p) => new
                    {
                        c.Id,
                        c.Category,
                        p.ProductId
                    }
                )
                .Join(_context.Movementdetails,
                    p => p.ProductId,
                    md => md.ProductId,
                    (p, md)=> new
                    {
                        p.Id,
                        p.Category,
                        md.MovementId
                    }
                )
                .Join(_context.Movements,
                    md => md.MovementId,
                    m => m.MovementId,
                    (md, m) => new
                    {
                        md.Id,
                        md.Category,
                        m.EmployeeId,
                        m.Type
                    }
                )
                .Where(o => 
                    o.EmployeeId == IdEmpleado &&
                    o.Type == "VENTA"
                )
                .GroupBy(o => new { o.Id, o.Category } )
                .Select( o => new
                {
                    Id = o.Key.Id,
                    Category = o.Key.Category,
                    Movimientos=o.Count()
                })
                .OrderBy(o=> o.Id)
                
                
                //.GroupBy(c => c.Id)
                .AsEnumerable();
        }


        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
          if (_context.Categories == null)
          {
              return NotFound();
          }
            return await _context.Categories.ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
          if (_context.Categories == null)
          {
              return NotFound();
          }
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
          if (_context.Categories == null)
          {
              return Problem("Entity set 'NorthwindContext.Categories'  is null.");
          }
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
