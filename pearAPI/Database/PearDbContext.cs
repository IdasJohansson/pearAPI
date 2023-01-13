using System;
using Microsoft.EntityFrameworkCore;
using pearAPI.Models;

namespace pearAPI.Database
{
	public class PearDbContext : DbContext
	{
        public PearDbContext(DbContextOptions<PearDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set;}
        public DbSet<Delivery> Delivery { get; set; }
        public DbSet<Warehouse> Warehouse { get; set;}
    }
}

