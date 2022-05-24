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
    public class SuppliersController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public SuppliersController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/Suppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
          if (_context.Suppliers == null)
          {
              return NotFound();
          }
            return await _context.Suppliers.ToListAsync();
        }

        // GET: api/Suppliers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
          if (_context.Suppliers == null)
          {
              return NotFound();
          }
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            return supplier;
        }

        // PUT: api/Suppliers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(int id, Supplier supplier)
        {
            if (id != supplier.SupplierId)
            {
                return BadRequest();
            }

            _context.Entry(supplier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
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

[HttpGet]
        [Route("salesbot")]
        public IEnumerable<Object> GetSalesBot5PerDate(DateTime startDate, DateTime endDate)
        {
            string prueba = "";
          return _context.Suppliers
              //  .Where(e => e.SupplierId == 1)
                .Join(
                    _context.Movements, 
                    s => s.SupplierId,
                    m => m.SupplierId,
                    (s,m) => new {
                        Proveedor = s.CompanyName,
                        m.Date,
                        IdMovimiento = m.MovementId,
                    })
                .Where(m => m.Date >= startDate
                    && m.Date <= endDate
                )
                .Join(_context.Movementdetails, 
                    sm => sm.IdMovimiento,
                    md => md.MovementId,
                    (sm, md) => new
                    {
                        Proveedor = sm.Proveedor,
                        Cantidad = md.Quantity,
                        ProductoID = md.ProductId
                    }
                ).Join(_context.Products, 
                    smmd => smmd.ProductoID,
                    p => p.ProductId,
                    (smmd, p) => new
                    {
                        NombreProducto = p.ProductName,
                        Proveedor = smmd.Proveedor,
                        Cantidad = smmd.Cantidad,
                        
                    }
                )
                .GroupBy(s => new { s.Proveedor })
                .Select(s => new
                {
                    Proveedor = s.Key.Proveedor,
                    Ventas = s.Select(l=>l.Cantidad).Single()
                })
                .OrderBy(s => s.Ventas)
                .Take(5)
                .AsEnumerable();
                
                
        }
        // POST: api/Suppliers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Supplier>> PostSupplier(Supplier supplier)
        {
          if (_context.Suppliers == null)
          {
              return Problem("Entity set 'NorthwindContext.Suppliers'  is null.");
          }
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSupplier", new { id = supplier.SupplierId }, supplier);
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            if (_context.Suppliers == null)
            {
                return NotFound();
            }
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SupplierExists(int id)
        {
            return (_context.Suppliers?.Any(e => e.SupplierId == id)).GetValueOrDefault();
        }
    }
}
