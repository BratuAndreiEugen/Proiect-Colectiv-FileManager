using fileAPI.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileAPI.Infrastructure
{
    public class FileContext : DbContext
    {
        public DbSet<FileEntry> Files { get; set; }

        public FileContext(DbContextOptions<FileContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FileEntry>().ToTable("photos");
            modelBuilder.Entity<FileEntry>().Property(t => t.Uri).HasColumnName("link");
            modelBuilder.Entity<FileEntry>().Property(t => t.RecipeId).HasColumnName("recipe_id");


            modelBuilder.Entity<FileEntry>().HasKey(t => t.Uri);
        }

    }
}
