using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Principal;

namespace Entities
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User>? Users { get; set; }
        public DbSet<Vendor>? Vendors { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<Request>? Requests { get; set; }
        public DbSet<RequestLine>? RequestLines { get; set; }
    }
}