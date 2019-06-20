using System;
using Microsoft.EntityFrameworkCore;

namespace MazeRetreat.Api.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Maze> Mazes { get; set; }
        public DbSet<Image> Images { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRING");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Maze>().HasKey(x => x.Id).ForSqlServerIsClustered(clustered: false);
            modelBuilder.Entity<Maze>().HasIndex(x => x.SysId).IsUnique().ForSqlServerIsClustered();
            modelBuilder.Entity<Maze>().Property(x => x.SysId).ValueGeneratedOnAdd();

            modelBuilder.Entity<Image>().HasKey(x => x.Id).ForSqlServerIsClustered(clustered: false);
            modelBuilder.Entity<Image>().HasIndex(x => x.SysId).IsUnique().ForSqlServerIsClustered();
            modelBuilder.Entity<Image>().Property(x => x.SysId).ValueGeneratedOnAdd();
            modelBuilder.Entity<Image>().HasIndex(x => x.Checksum).IsUnique();
        }
    }
}