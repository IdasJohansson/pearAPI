using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
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

        [HttpGet("warehouse-product-quantity")]
        public async Task<ActionResult<IEnumerable<DeliveryDTO>>> GetWarehouseProductQuantity()
        {
            //var query = from delivery in _context.Delivery
            //            group delivery by new { delivery.WarehouseId, delivery.ProductId } into g
            //            select new Delivery
            //            {
            //                WarehouseId = g.Key.WarehouseId,
            //                ProductId = g.Key.ProductId,
            //                Quantity = g.Sum(x => x.Quantity)
            //            };

            // Created this querie joins the tables and returns the query with WarehouseName and ProductName instead. 
             var query = from delivery in _context.Delivery
                         join product in _context.Products on delivery.ProductId equals product.Id
                         join warehouse in _context.Warehouse on delivery.WarehouseId equals warehouse.Id
                         group delivery by new { warehouse.City, product.Name } into g
                         select new DeliveryDTO
                         {
                             WarehouseName = g.Key.City,
                             ProductName = g.Key.Name,
                             Quantity = g.Sum(x => x.Quantity)
                         };

            return await query.ToListAsync();
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

        private bool DeliveryExists(Guid id)
        {
            return (_context.Delivery?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
