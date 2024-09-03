using Microsoft.EntityFrameworkCore;
using PationsMedics_test.Models;

namespace PationsMedics_test.Data
{

    public class PationsMedicsContext : DbContext
    {
        public PationsMedicsContext(DbContextOptions<PationsMedicsContext> options): base (options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Cabinet>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Specialization>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Medic>()
                .HasKey(m => m.Id);
            modelBuilder.Entity<Medic>()
                .HasOne(m => m.Cabinet)
                .WithMany(c => c.Medics);
            modelBuilder.Entity<Medic>()
                .HasOne(m => m.Specialization)
                .WithMany(s => s.Medics);
            modelBuilder.Entity<Medic>()
                .HasOne(m => m.Area)
                .WithMany(a => a.Medics);

            modelBuilder.Entity<Patient>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Area)
                .WithMany(a => a.Patients);

            modelBuilder.Entity<Area>()
                .HasKey(a => a.Id);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Area> Areas { get; set; }
        public DbSet<Cabinet> Cabinets { get; set; }
        public DbSet<Medic> Medics { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
    }
}
