using Microsoft.EntityFrameworkCore;

namespace ProjetBrasserie.Models
{
    public class BrasserieDbContext : DbContext
    {
        public BrasserieDbContext(DbContextOptions<BrasserieDbContext> options)
           : base(options)
        {
        }

        public DbSet<Brasserie> Brasseries { get; set; }
        public DbSet<Biere> Bieres { get; set; }
        public DbSet<Grossiste> Grossistes { get; set; }
        public DbSet<GrossisteStock> GrossisteStocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brasserie>().HasData(
                new Brasserie { Id = 1, Nom = "Abbaye de Maredsous" },
                new Brasserie { Id = 2, Nom = "Brasserie d'Achouffe" },
                new Brasserie { Id = 3, Nom = "Trappist Westvleteren" });

            modelBuilder.Entity<Brasserie>().Property(br => br.Nom).IsRequired();

            modelBuilder.Entity<Biere>().HasData(
                new Biere { Id = 1, BrasserieId = 1, Nom = "Maredsous Blonde", Degre = 6m, Prix = 2.6m },
                new Biere { Id = 2, BrasserieId = 1, Nom = "Maredsous Brune", Degre = 8m, Prix = 3m },
                new Biere { Id = 3, BrasserieId = 1, Nom = "Maredsous Triple", Degre = 10m, Prix = 3m },
                new Biere { Id = 4, BrasserieId = 2, Nom = "La Chouffe", Degre = 8m, Prix = 2.5m },
                new Biere { Id = 5, BrasserieId = 2, Nom = "Cherry Chouffe", Degre = 8m, Prix = 2.5m },
                new Biere { Id = 6, BrasserieId = 3, Nom = "Westvleteren Blond", Degre = 5.8m, Prix = 3.2m },
                new Biere { Id = 7, BrasserieId = 3, Nom = "Westvleteren 8", Degre = 8m, Prix = 3.5m },
                new Biere { Id = 8, BrasserieId = 3, Nom = "Westvleteren 12", Degre = 10.2m, Prix = 3.5m });

            modelBuilder.Entity<Biere>().Property(bi => bi.Nom).IsRequired();

            modelBuilder.Entity<Grossiste>().HasData(
                new Grossiste { Id = 1, Nom = "GeneDrinks" },
                new Grossiste { Id = 2, Nom = "Top Beer" });

            modelBuilder.Entity<Grossiste>().Property(gr => gr.Nom).IsRequired();

            modelBuilder.Entity<GrossisteStock>().HasData(
                new GrossisteStock { BiereId = 7, GrossisteId = 2, Quantite = 20 },
                new GrossisteStock { BiereId = 4, GrossisteId = 2, Quantite = 15 },
                new GrossisteStock { BiereId = 1, GrossisteId = 1, Quantite = 30 },
                new GrossisteStock { BiereId = 6, GrossisteId = 1, Quantite = 15 },
                new GrossisteStock { BiereId = 2, GrossisteId = 1, Quantite = 25 });

            modelBuilder.Entity<GrossisteStock>().HasKey(st => new { st.GrossisteId, st.BiereId });

            modelBuilder.Entity<Biere>().HasOne(bi => bi.Brasserie).WithMany(br => br.Bieres).HasForeignKey(bi => bi.BrasserieId);
            modelBuilder.Entity<GrossisteStock>().HasOne(st => st.Grossiste).WithMany(gr => gr.Stocks).HasForeignKey(st => st.GrossisteId);
            modelBuilder.Entity<GrossisteStock>().HasOne(st => st.Biere).WithMany(bi => bi.Stocks).HasForeignKey(st => st.BiereId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
