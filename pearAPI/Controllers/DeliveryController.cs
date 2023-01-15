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
    public class DeliveryController : ControllerBase
    {
        private readonly PearDbContext _context;

        public DeliveryController(PearDbContext context)
        {
            _context = context;
        }

        // GET: api/Delivery
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetDelivery()
        {
          if (_context.Delivery == null)
          {
              return NotFound();
          }
            return await _context.Delivery.ToListAsync();
        }

        // GET: api/Delivery/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Delivery>> GetDelivery(Guid id)
        {
          if (_context.Delivery == null)
          {
              return NotFound();
          }
            var delivery = await _context.Delivery.FindAsync(id);

            if (delivery == null)
            {
                return NotFound();
            }

            return delivery;
        }

        // PUT: api/Delivery/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDelivery(Guid id, Delivery delivery)
        {
            if (id != delivery.Id)
            {
                return BadRequest();
            }

            _context.Entry(delivery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryExists(id))
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

        // POST: api/Delivery
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Delivery>> PostDelivery(Delivery delivery)
        {
          if (_context.Delivery == null)
          {
              return Problem("Entity set 'PearDbContext.Delivery'  is null.");
          }
            _context.Delivery.Add(delivery);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDelivery", new { id = delivery.Id }, delivery);
        }

        // DELETE: api/Delivery/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDelivery(Guid id)
        {
            if (_context.Delivery == null)
            {
                return NotFound();
            }
            var delivery = await _context.Delivery.FindAsync(id);
            if (delivery == null)
            {
                return NotFound();
            }

            _context.Delivery.Remove(delivery);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeliveryExists(Guid id)
        {
            return (_context.Delivery?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
