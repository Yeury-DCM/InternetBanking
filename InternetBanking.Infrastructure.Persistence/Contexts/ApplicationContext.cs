using InternetBanking.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Persistence.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

      
        public DbSet<Product> products { get; set; }
        public DbSet<Beneficiary> beneficiaries { get; set; }
        public DbSet<Transaction> transactions { get; set; }
        public DbSet<ProductType> productTypes { get; set; }
        public DbSet<TransactionType> transactionTypes  { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region tables
            
            modelBuilder.Entity<Product>().ToTable("Productos");
            modelBuilder.Entity<Beneficiary>().ToTable("Beneficiarios");
            modelBuilder.Entity<Transaction>().ToTable("Transacciones");
            modelBuilder.Entity<ProductType>().ToTable("TipoProductos");
            modelBuilder.Entity<TransactionType>().ToTable("TipoTransacciones");
            #endregion

            #region Keys
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Beneficiary>().HasKey(b => b.Id);
            modelBuilder.Entity<Transaction>().HasKey(t => t.Id);
            modelBuilder.Entity<ProductType>().HasKey(pt => pt.Id);
            modelBuilder.Entity<TransactionType>().HasKey(tt => tt.Id);
            #endregion

            #region relationships
        
            #region products

            modelBuilder.Entity<Product>()
                .HasMany<Transaction>(p => p.transactions)
                .WithOne(t => t.Product)
                .HasForeignKey(t => t.ProductID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>()
               .HasOne<ProductType>(p => p.productType)
               .WithMany(pt => pt.Products)
               .HasForeignKey(p => p.ProductTypeID)
               .OnDelete(DeleteBehavior.NoAction);


            #endregion

            #region Beneficiaries

            modelBuilder.Entity<Beneficiary>()
           .HasOne<Product>(b => b.account)
           .WithMany()
           .HasForeignKey(b => b.ProductID)
           .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region transactions

            modelBuilder.Entity<Transaction>()
              .HasOne<Product>(t => t.Product)
              .WithMany(p => p.transactions)
              .HasForeignKey(t => t.ProductID)
              .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Transaction>()
               .HasOne<TransactionType>(t => t.transactionType)
               .WithMany(tt => tt.transactions)
               .HasForeignKey(t => t.TransactionTypeID)
               .OnDelete(DeleteBehavior.NoAction);

            #endregion


            #region transactiontypes
            modelBuilder.Entity<TransactionType>()
              .HasMany<Transaction>(tt => tt.transactions)
              .WithOne(t => t.transactionType)
              .HasForeignKey(t => t.TransactionTypeID)
              .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region producttypes
            modelBuilder.Entity<ProductType>()
              .HasMany<Product>(pt => pt.Products)
              .WithOne(p => p.productType)
              .HasForeignKey(p => p.ProductTypeID)
              .OnDelete(DeleteBehavior.NoAction);
            #endregion
            #endregion

            #region properties configurations


            #region products
            modelBuilder.Entity<Product>().Property(p => p.ProductNumber).HasMaxLength(9);
            modelBuilder.Entity<Product>().Property(p => p.Balance).HasDefaultValue(0.00m).HasPrecision(18,2);
            modelBuilder.Entity<Product>().Property(p => p.Limit).HasPrecision(18,2);
            modelBuilder.Entity<Product>().Property(p => p.IsPrincipal).HasDefaultValue(false);

            #endregion

            #region transaction
            modelBuilder.Entity<Transaction>().Property(t => t.Amount).HasPrecision(18,2);
            #endregion

            #endregion
        }
    }
}
