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
    public class ProductsController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public ProductsController(NorthwindContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("sales")]
        public IEnumerable<Object> GetSalesFromProduct(String name, DateTime startDate, DateTime endDate)
        {
            return _context.Products

                .Where(p => p.ProductName == name)
                .Join(
                    _context.Movementdetails,
                    p => p.ProductId,
                    md => md.ProductId,
                    (p, md) => new
                    {
                        Producto = p.ProductId,
                        Movimiento = md.MovementId,
                        Nombre = p.ProductName,
                        Cantidad = md.Quantity
                    }
                )
                .Join(
                    _context.Movements,
                    md => md.Movimiento,
                    m => m.MovementId,
                    (md, m) => new
                    {
                        Poducto=md.Producto,
                        md.Nombre,
                        md.Cantidad,
                        m.Date,
                        m.OriginWarehouseId,
                        m.Type
                    }
                )
                .Where(m => m.Date >= startDate
                    && m.Date <= endDate
                    //&& m.Type == "VENTAS"
                );
                
                
        }
        [HttpGet]
        [Route("CampaniasDatos")]
        public IEnumerable<Object> CompaniaDatos(int year)
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
                .Where(m => m.Anio == year && m.idC == 1)
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
                        SubTotal = (pr.Precio * pr.Cantidad),
                        productid = pr.Idproducto,
                        Total = pr.Cantidad
                    }
                )
                .GroupBy(s => s.Nombre)
                .Select(e => new
                {
                    Nombre = e.Key,
                    ProductosVendidos = e.Sum(g => g.Total),
                    VentaXProveedor = "$" + decimal.Round((decimal)e.Sum(e => e.SubTotal),2)

                })
                .OrderByDescending(f => f.ProductosVendidos)
                .Take(8)
                .AsEnumerable();
        }

        [HttpGet]
        [Route("salesbyear")]
        public IEnumerable<Object> GetSalesByYar(int year)
        {
            System.Globalization.DateTimeFormatInfo mfi = new
            System.Globalization.DateTimeFormatInfo();
            //string strMonthName = mfi.GetMonthName(8).ToString();

            return _context.Products
                .Join(
                    _context.Movementdetails,
                    p => p.ProductId,
                    md => md.ProductId,
                    (p, md) => new
                    {
                        Producto = p.ProductId,
                        Movimiento = md.MovementId,
                        Cantidad = md.Quantity,
                        numero = p.UnitPrice
                    }
                )
                .Join(
                    _context.Movements,
                    md => md.Movimiento,
                    m => m.MovementId,
                    (md, m) => new
                    {
                        Poducto = md.Producto,
                        Dia = m.Date.Day,
                        Mes = m.Date.Month,
                        Anio = m.Date.Year,
                        Tipo = m.Type,
                        Compania = m.CompanyId,
                        Cantidad = md.Cantidad,
                        Precio=md.numero
                    }
                )
                .Where(m => m.Anio == year
                    && m.Tipo == "VENTA"
                    && m.Compania == 1
                )
                .GroupBy(o => new {o.Dia, o.Mes, o.Compania}, (k)=> 
                    new {Company=k.Compania, Dia=k.Dia, Mes=k.Mes, k.Cantidad, k.Precio})
                .Select(o => new
                {
                    Company = o.Key.Compania,
                    Fecha = mfi.GetMonthName(o.Key.Mes)+ "-"+ o.Key.Dia,
                    //Mes = o.Key.Mes,
                    Cantidad = o.Sum( o => o.Cantidad),
                    Ventas = decimal.Round((decimal) o.Sum(o=> o.Cantidad*o.Precio),2)

                })
                ;


        }

        [HttpGet]
        [Route("salesbytri/{year}/{tri}")]
        public IEnumerable<Object> GetSalesByTri(int year, int tri)
        {
            int limSup = tri*3;
            int limInf = limSup-2;
            return _context.Products
                .Join(
                    _context.Movementdetails,
                    p => p.ProductId,
                    md => md.ProductId,
                    (p, md) => new
                    {
                        Producto = p.ProductId,
                        Name = p.ProductName,
                        Movimiento = md.MovementId,
                        Cantidad = md.Quantity,
                    }
                )
                .Join(
                    _context.Movements,
                    md => md.Movimiento,
                    m => m.MovementId,
                    (md, m) => new
                    {
                        Poducto = md.Producto,
                        Namee=md.Name,
                        Anio = m.Date.Year,     
                        Mes = m.Date.Month,                   
                        Cantidad = md.Cantidad,
                    }
                )
                .Where(m => m.Anio == year && m.Mes <= limSup && m.Mes >= limInf
                )
                .GroupBy(e => new {e.Namee})
                .Select(e => new
                {
                    Producto = e.Key.Namee,
                    Cantidad =  e.Sum(l=>l.Cantidad)

                })
                .OrderBy(e => e.Cantidad)
                .Take(5)
                .AsEnumerable();
            ;
}

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
          if (_context.Products == null)
          {
              return Problem("Entity set 'NorthwindContext.Products'  is null.");
          }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
