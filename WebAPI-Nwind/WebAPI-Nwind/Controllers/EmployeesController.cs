#nullable disable
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
    public class EmployeesController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public EmployeesController(NorthwindContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("CampaniasDatos")]
        public IEnumerable<Object> CompaniaDatos()
        {
            return _context.Movements
                .Join(
                    _context.Movementdetails,
                    m => m.MovementId,
                    md => md.MovementId,
                    (m, md) => new {
                        Anio = m.Date.Year,
                        Cantidad = md.Quantity,
                        productoid = md.ProductId,
                        idC = m.CompanyId
                    })
                .Where(m => m.Anio == 1996 && m.idC == 1)
                .Join(_context.Products,
                    mod => mod.productoid,
                    p => p.ProductId,
                    (mod, p) => new
                    {
                        Precio = p.UnitPrice,
                        Companiaid = p.SupplierId,
                        Cantidad = mod.Cantidad,
                        Idproducto = p.ProductId
                    }
                )
                .Join(_context.Suppliers,
                    pr => pr.Companiaid,
                    s => s.SupplierId,
                    (pr, s) => new
                    {
                        Nombre = s.CompanyName,
                        SubTotal = pr.Precio * pr.Cantidad,
                        productid = pr.Idproducto

                    }
                )
                .GroupBy(s => s.Nombre)
                .Select(e => new
                {
                    Nombre = e.Key,
                    ProductosVendidos = e.Count(),
                    VentaXProveedor = e.Sum(g => g.SubTotal)

                })
                .OrderBy(f => f.Nombre)
                .Take(10)
                .AsEnumerable();
        }

        [HttpGet]
        [Route("ByCompany")]
        public IEnumerable<Object> GetEmployeesByCompany()
        {
            return _context.Employees.
                GroupBy(e => e.CompanyId).
                Select(e => new {
                    Company = e.Key,
                    Empleados = e.Count()
                })
                .AsEnumerable();
        }

        [HttpGet]
        [Route("Top5")]
        public IEnumerable<Object> GetTop5Employees()
        {
            return _context.Employees
                .Where(e => e.CompanyId == 1)
                .Join(
                    _context.Movements, 
                    e => e.EmployeeId,
                    m => m.EmployeeId,
                    (e,m) => new {
                        Empleado = e.FirstName +" "+ e.LastName,
                        IdMovimiento = m.MovementId,
                        Anio = m.Date.Year
                    })
                .Where(em => em.Anio==1996)
                .Join(_context.Movementdetails, 
                    em => em.IdMovimiento,
                    md => md.MovementId,
                    (em, md) => new
                    {
                        Empleado = em.Empleado,
                        Cantidad = md.Quantity
                    }
                )
                .GroupBy(e => e.Empleado)
                .Select(e => new
                {
                    Empleado = e.Key,
                    Ventas = e.Sum(g => g.Cantidad)
                })
                .OrderByDescending(e => e.Ventas)
                .Take(5)
                .AsEnumerable();
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.EmployeeId }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
