using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendService.Data;

namespace BackendService.Contexts
{
    /// <summary>
    /// Context to the database for code first approach
    /// </summary>
    public class ApplicationContext : DbContext
    {
        public ApplicationContext () : base() { }
        public ApplicationContext (DbContextOptions options) : base(options) { }
        public DbSet<Request> Requests { get; set; }
    }
}
