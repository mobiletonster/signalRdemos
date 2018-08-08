using BrainBuffet.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainBuffet.Data
{
    public class BrainBuffetContext: DbContext
    {
        public BrainBuffetContext()
        {

        }

        public virtual DbSet<Question> Questions { get; set; }

        public BrainBuffetContext(DbContextOptions<BrainBuffetContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
