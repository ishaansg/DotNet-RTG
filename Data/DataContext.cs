using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DOTNET_RPG.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
                new Skill { Id = 1, Name = "Fire Bolt", Damage = 100 },
                new Skill { Id = 2, Name = "Freeze", Damage = 80 },
                new Skill { Id = 3, Name = "Air Kick", Damage = 120 }
            );
        }

        // Creates a table with name Characters
        public DbSet<Character> Characters => Set<Character>();
        public DbSet<User> Users => Set<User>();

        public DbSet<Weapon> Weapons => Set<Weapon>();

        public DbSet<Skill> Skills => Set<Skill>();
    }
}