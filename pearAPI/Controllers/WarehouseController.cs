using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pearAPI.Database;
using pearAPI.Models;

namespace pearAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly PearDbContext _context;

        public WarehouseController(PearDbContext context)
        {
            _context = context;
        }

        // GET: api/Warehouse
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Warehouse>>> GetWarehouse()
        {
          if (_context.Warehouse == null)
          {
              return NotFound();
          }
            return await _context.Warehouse.ToListAsync();
        }

        // GET: api/Warehouse/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Warehouse>> GetWarehouse(int id)
        {
          if (_context.Warehouse == null)
          {
              return NotFound();
          }
            var warehouse = await _context.Warehouse.FindAsync(id);

            if (warehouse == null)
            {
                return NotFound();
            }

            return warehouse;
        }


        // POST: api/Warehouse
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Warehouse>> PostWarehouse(Warehouse warehouse)
        {
          if (_context.Warehouse == null)
          {
              return Problem("Entity set 'PearDbContext.Warehouse'  is null.");
          }
            _context.Warehouse.Add(warehouse);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWarehouse", new { id = warehouse.Id }, warehouse);
        }

        private bool WarehouseExists(int id)
        {
            return (_context.Warehouse?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
