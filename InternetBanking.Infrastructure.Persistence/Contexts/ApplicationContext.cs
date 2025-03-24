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

        public DbSet<User> users { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Beneficiary> beneficiaries { get; set; }
        public DbSet<Transaction> transactions { get; set; }
        public DbSet<UserType> userTypes    { get; set; }
        public DbSet<ProductType> productTypes { get; set; }
        public DbSet<TransactionType> transactionTypes  { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region tables
            modelBuilder.Entity<User>().ToTable("Usuarios");
            modelBuilder.Entity<Product>().ToTable("Productos");
            modelBuilder.Entity<Beneficiary>().ToTable("Beneficiarios");
            modelBuilder.Entity<Transaction>().ToTable("Transacciones");
            modelBuilder.Entity<UserType>().ToTable("TipoUsuarios");
            modelBuilder.Entity<ProductType>().ToTable("TipoProductos");
            modelBuilder.Entity<TransactionType>().ToTable("TipoTransacciones");
            #endregion

            #region primary keys
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Product>().HasKey(p => p.Id);
            modelBuilder.Entity<Beneficiary>().HasKey(b => b.Id);
            modelBuilder.Entity<Transaction>().HasKey(t => t.Id);
            modelBuilder.Entity<UserType>().HasKey(ut => ut.Id);
            modelBuilder.Entity<ProductType>().HasKey(pt => pt.Id);
            modelBuilder.Entity<TransactionType>().HasKey(tt => tt.Id);
            #endregion

            #region relationships
            #region users
            modelBuilder.Entity<User>()
               .HasMany<Product>(u => u.products)
               .WithOne(p => p.User)
               .HasForeignKey(p => p.UserID)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasMany<Transaction>(u => u.transactions)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
               .HasMany<Beneficiary>(u => u.beneficiaries)
               .WithOne(b => b.user)
               .HasForeignKey(b => b.UserID)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne<UserType>(u => u.userType)
                .WithMany(ut => ut.users)
                .HasForeignKey(u => u.UserTypeID)
                .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region products

            modelBuilder.Entity<Product>()
                .HasMany<Transaction>(p => p.transactions)
                .WithOne(t => t.Product)
                .HasForeignKey(t => t.ProductID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>()
               .HasOne<ProductType>(p => p.ProductType)
               .WithMany(pt => pt.Products)
               .HasForeignKey(p => p.ProductTypeID)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>()
               .HasOne<User>(p => p.User)
               .WithMany(u => u.products)
               .HasForeignKey(p => p.UserID)
               .OnDelete(DeleteBehavior.NoAction);


            #endregion

            #region Beneficiaries
            modelBuilder.Entity<Beneficiary>()
            .HasOne<User>(b => b.user)
            .WithMany(u => u.beneficiaries)
            .HasForeignKey(b => b.UserID)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Beneficiary>()
           .HasOne<Product>(b => b.account)
           .WithMany()
           .HasForeignKey(b => b.ProductID)
           .OnDelete(DeleteBehavior.NoAction);
            #endregion

            #region transactions

            modelBuilder.Entity<Transaction>()
                .HasOne<User>(t => t.User)
                .WithMany(u => u.transactions)
                .HasForeignKey(t => t.UserID)
                .OnDelete(DeleteBehavior.NoAction);

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

            #region usertypes
            modelBuilder.Entity<UserType>()
                .HasMany<User>(ut => ut.users)
                .WithOne(u => u.userType)
                .HasForeignKey(u => u.UserTypeID)
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
              .WithOne(p => p.ProductTypeID)
              .HasForeignKey(p => p.ProductType)
              .OnDelete(DeleteBehavior.NoAction);
            #endregion
            #endregion

            #region properties configurations

            #region user
            modelBuilder.Entity<User>().Property(u => u.Name).HasMaxLength(30);
            modelBuilder.Entity<User>().Property(u => u.LastName).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Mail).HasMaxLength(320);
            modelBuilder.Entity<User>().Property(u => u.UserName).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(u => u.Password).HasMaxLength(255);
            modelBuilder.Entity<User>().Property(u => u.Status).HasDefaultValue(false);
            modelBuilder.Entity<User>().Property(u => u.Identification).HasMaxLength(11);
            #endregion

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
