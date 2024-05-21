using Microsoft.EntityFrameworkCore;
using NETAPI_LevelGroupChallenge.Models;

namespace NETAPI_LevelGroupChallenge.Data
{
    public class CategoriaDb : DbContext
    {

        public DbSet<Categoria> Categorias { get; set; }

        public CategoriaDb(DbContextOptions<CategoriaDb> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>().ToTable("CATEGORIA");

            modelBuilder.Entity<Categoria>(entity =>
         {
             entity.Property(e => e.Id)
                 .HasColumnType("INTEGER")
                 .UseIdentityColumn()
                 .HasColumnName("CATEGORIA_ID");
             entity.Property(e => e.Name)
                 .IsRequired()
                 .HasColumnType("NVARCHAR2(255)")
                 .HasColumnName("CATEGORIA_NAME");
         });
        }
    }
}
