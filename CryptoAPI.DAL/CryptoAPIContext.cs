using CryptoAPI.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAPI.DAL
{
    public class CryptoAPIContext:DbContext
    {
        public CryptoAPIContext(DbContextOptions<CryptoAPIContext> options):base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Portfolio> Portfolio { get; set; }
        public DbSet<PortfolioItem> PortfolioItem { get; set; }
        public DbSet<PortfolioNote> PortfolioNotes { get; set; }

    }
}
