using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BussinessObject;

public partial class MilkTeaShopContext : DbContext
{
    public MilkTeaShopContext()
    {
    }

    public MilkTeaShopContext(DbContextOptions<MilkTeaShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Combo> Combos { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Topping> Toppings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Uid=sa;Pwd=12345;Database=MilkTeaShop;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2B2FECA369");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Combo>(entity =>
        {
            entity.HasKey(e => e.ComboId).HasName("PK__Combo__DD42580E79112B35");

            entity.ToTable("Combo");

            entity.Property(e => e.ComboId).HasColumnName("ComboID");
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Product1Id).HasColumnName("Product1ID");
            entity.Property(e => e.Product2Id).HasColumnName("Product2ID");
            entity.Property(e => e.Topping1Id).HasColumnName("Topping1ID");
            entity.Property(e => e.Topping2Id).HasColumnName("Topping2ID");

            entity.HasOne(d => d.Product1).WithMany(p => p.ComboProduct1s)
                .HasForeignKey(d => d.Product1Id)
                .HasConstraintName("FK__Combo__Product1I__440B1D61");

            entity.HasOne(d => d.Product2).WithMany(p => p.ComboProduct2s)
                .HasForeignKey(d => d.Product2Id)
                .HasConstraintName("FK__Combo__Product2I__44FF419A");

            entity.HasOne(d => d.Topping1).WithMany(p => p.ComboTopping1s)
                .HasForeignKey(d => d.Topping1Id)
                .HasConstraintName("FK__Combo__Topping1I__45F365D3");

            entity.HasOne(d => d.Topping2).WithMany(p => p.ComboTopping2s)
                .HasForeignKey(d => d.Topping2Id)
                .HasConstraintName("FK__Combo__Topping2I__46E78A0C");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__C3905BAF4CB241E3");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Orders__UserID__49C3F6B7");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D30CFD757C73");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.ComboId).HasColumnName("ComboID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Combo).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ComboId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__OrderDeta__Combo__5165187F");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__OrderDeta__Order__4F7CD00D");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__OrderDeta__Produ__5070F446");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6EDAC49514A");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Products__Catego__3D5E1FD2");
        });

        modelBuilder.Entity<Topping>(entity =>
        {
            entity.HasKey(e => e.ToppingId).HasName("PK__Toppings__EE02CCE54B615F38");

            entity.Property(e => e.ToppingId).HasColumnName("ToppingID");
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACBA15F6CD");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E47CBF3484").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(10);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
