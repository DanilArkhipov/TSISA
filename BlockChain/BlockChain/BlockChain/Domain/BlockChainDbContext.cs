using BlockChain.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlockChain.Domain;

public class BlockChainDbContext : DbContext
{
    public DbSet<Block> Blocks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=DESKTOP-520CUVL;Database=BlockChainDB;Trusted_Connection=True;");
    }
}