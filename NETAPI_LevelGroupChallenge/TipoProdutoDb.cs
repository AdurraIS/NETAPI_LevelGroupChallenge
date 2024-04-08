using Microsoft.EntityFrameworkCore;

namespace NETAPI_LevelGroupChallenge
{
    public class TipoProdutoDb : DbContext
    {

        public DbSet<TipoProduto> TipoProdutos { get; set; }

        public TipoProdutoDb(DbContextOptions<TipoProdutoDb> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoProduto>().ToTable("TIPOPRODUTO");

            modelBuilder.Entity<TipoProduto>(entity =>
         {
             entity.Property(e => e.Id)
                 .HasColumnType("INTEGER")
                 .UseIdentityColumn()
                 .HasColumnName("PRODUCT_TYPE_ID");
             entity.Property(e => e.Name)
                 .IsRequired()
                 .HasColumnType("NVARCHAR2(255)")
                 .HasColumnName("PRODUCT_TYPE_NAME");
         });
        }
    }
}
