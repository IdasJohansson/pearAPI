using System;
namespace pearAPI.Models
{
	public class Delivery
	{
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string? ProductId { get; set; }
        public int Quantity { get; set; }
        public int WarehouseId { get; set; }
        public Guid UserId { get; set; }
    }
}

