using LocalCellars.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LocalCellars.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options){}

        public DbSet<Value> Values { get; set; }
    }
}