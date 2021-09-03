using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace PlayersApi.Models
{
    public class TransfermarketContext : DbContext
    {
        public TransfermarketContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
          
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TransferMarket;Trusted_Connection=True;");
             
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Team>Teams { get; set; }
    }
}
