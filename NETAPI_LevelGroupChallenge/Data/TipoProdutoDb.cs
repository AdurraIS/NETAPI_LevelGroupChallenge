using Microsoft.EntityFrameworkCore;
using NETAPI_LevelGroupChallenge.Models;

namespace NETAPI_LevelGroupChallenge.Data
{
    public class TipoProdutoDb : DbContext
    {

        public DbSet<TipoProduto> TipoProdutos { get; set; }

        public TipoProdutoDb(DbContextOptions<TipoProdutoDb> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoProduto>().ToTable("TIPO_PRODUTO");

            modelBuilder.Entity<TipoProduto>(entity =>
         {
             entity.Property(e => e.Id)
                 .HasColumnType("INTEGER")
                 .UseIdentityColumn()
                 .HasColumnName("TIPO_PRODUTO_ID");
             entity.Property(e => e.Name)
                 .IsRequired()
                 .HasColumnType("NVARCHAR2(255)")
                 .HasColumnName("TIPO_PRODUTO_NAME");
         });
        }
    }
}
