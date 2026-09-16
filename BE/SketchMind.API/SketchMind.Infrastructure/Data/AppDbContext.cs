using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SketchMind.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Infrastructure.Data
{
    // class will connect our entity to database and manage the database operations
    // extend DBContext but as we use identiy and identityDbcontext by default extend Dbcontext
    // we will extend IdentityDbContext directly to get all the identity related tables and functionality
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        // public constructor to pass options to base class
        // option is passed from the startup class when we configure the dbcontext in the dependency injection container
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Domain entity configurations will go here later
        }
    }
}
