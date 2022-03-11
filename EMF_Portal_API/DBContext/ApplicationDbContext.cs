using EMF_Portal_API.IdentityAuth;
using EMF_Portal_API.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMF_Portal_API.IdentityAuth
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }
        
        public DbSet<UserDetails> UserDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserDetails>().ToTable("UserDetails");
            builder.Entity<DocumentUpload>().ToTable("DocumentUpload");
            base.OnModelCreating(builder);
        }
        public DbSet<EMF_Portal_API.Model.Disability> Disability { get; set; }
        public DbSet<EMF_Portal_API.Model.Gender> Gender { get; set; }
        public DbSet<EMF_Portal_API.Model.Race> Race { get; set; }
        public DbSet<EMF_Portal_API.Model.Title> Title { get; set; }
        public DbSet<EMF_Portal_API.Model.Qualification> Qualification { get; set; }
        public DbSet<EMF_Portal_API.Model.DocumentUpload> DocumentUploads { get; set; }

    }
}
