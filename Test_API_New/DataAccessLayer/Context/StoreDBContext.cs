using Microsoft.EntityFrameworkCore;
using Test_API_New.DataAccessLayer.Entities;

namespace Test_API_New.DataAccessLayer.Context
{
    public partial class StoreDBContext : DbContext
    {
        public StoreDBContext() { }
        public StoreDBContext(DbContextOptions options) : base(options) { }
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);       // To set Id as a primary key
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();     // To set Id auto increment

                entity.Property(e => e.Name)
                    .HasColumnType("nvarchar")  // To set column data type
                    .HasMaxLength(50)           // To set column length
                    .IsRequired(true);          // To set column NOT NULL

                entity.Property(e => e.Age)
                    .HasColumnType("int")
                    .IsRequired(false);         // To set column NULL

                entity.Property(e => e.Email)
                    .IsUnicode(true)            // Mapped to nvarchar (It can store nvarchar data)
                    .IsRequired(true)
                    .HasMaxLength(450);
                entity.HasIndex(e => e.Email)   // To ensure that email property will accept
                    .IsUnique(true);            // To ensure all email property will unique

                entity.Property(e => e.Dob)
                    .HasColumnType("datetime")
                    .HasColumnName("DOB");      // To set column name as DOB

                entity.HasOne(d => d.Product)   // To set one to many relationship and set foregin key, We need to white where we define forrgin key
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.ProductId);  // This way we can set Foregin key
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50)
                    .IsRequired(true);

                entity.Property(e => e.Price)
                    .HasColumnType("float")
                    .IsRequired(true);
            });
        }
    }
}