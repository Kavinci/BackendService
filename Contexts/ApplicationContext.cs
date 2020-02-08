using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendService.Data;

namespace BackendService.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
        public DbSet<Request> Requests { get; set; }
    }
}
