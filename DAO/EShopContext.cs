using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace _21880263.DAO;

public partial class EShopContext : DbContext
{
    public EShopContext()
    {
    }

    public EShopContext(DbContextOptions<EShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<ImportOrder> ImportOrders { get; set; }

    public virtual DbSet<ImportOrderDetail> ImportOrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<SaleOrder> SaleOrders { get; set; }

    public virtual DbSet<SaleOrderDetail> SaleOrderDetails { get; set; }

    public string? ConnectionString { get; }

    public EShopContext(string ConnectionString)
    {
        this.ConnectionString = ConnectionString;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.BrandId).HasName("PK__Brands__5E5A8E276A75494D");

            entity.Property(e => e.BrandId)
                .ValueGeneratedNever()
                .HasColumnName("brand_id");
            entity.Property(e => e.BrandDescription)
                .HasColumnType("text")
                .HasColumnName("brand_description");
            entity.Property(e => e.BrandIsdeleted).HasColumnName("brand_isdeleted");
            entity.Property(e => e.BrandName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("brand_name");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__D54EE9B40B2CFD9D");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("category_id");
            entity.Property(e => e.CategoryDescription)
                .HasColumnType("text")
                .HasColumnName("category_description");
            entity.Property(e => e.CategoryIsdeleted).HasColumnName("category_isdeleted");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__IMAGES__47027DF5C7C34252");

            entity.ToTable("IMAGES");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("product_id");
            entity.Property(e => e.ProductImage).HasColumnName("product_image");

            entity.HasOne(d => d.Product).WithOne(p => p.Image)
                .HasForeignKey<Image>(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Image_Products");
        });

        modelBuilder.Entity<ImportOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__ImportOr__46596229346F34E3");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("order_id");
            entity.Property(e => e.OrderAddress)
                .HasMaxLength(255)
                .HasColumnName("order_address");
            entity.Property(e => e.OrderDate)
                .HasColumnType("date")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderIsdeleted).HasColumnName("order_isdeleted");
            entity.Property(e => e.OrderName)
                .HasMaxLength(50)
                .HasColumnName("order_name");
            entity.Property(e => e.OrderPhone)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("order_phone");
            entity.Property(e => e.OrderTotal)
                .HasColumnType("decimal(20, 2)")
                .HasColumnName("order_total");
        });

        modelBuilder.Entity<ImportOrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderdetailId).HasName("PK__ImportOr__59AE7411AA496B83");

            entity.ToTable(tb =>
            {
                tb.HasTrigger("UPDATE_ORDER_TOTAL");
                tb.HasTrigger("update_product_quantity");
            });

            entity.Property(e => e.OrderdetailId).HasColumnName("orderdetail_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.OrderdetailIsdeleted).HasColumnName("orderdetail_isdeleted");
            entity.Property(e => e.OrderdetailPrice)
                .HasColumnType("decimal(20, 2)")
                .HasColumnName("orderdetail_price");
            entity.Property(e => e.OrderdetailQuantity).HasColumnName("orderdetail_quantity");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Order).WithMany(p => p.ImportOrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_ImportOrders_ImportOrderDetails");

            entity.HasOne(d => d.Product).WithMany(p => p.ImportOrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Products_ImportOrderDetails");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__47027DF553CF4FE3");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("product_id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.ProductDescription).HasColumnName("product_description");
            entity.Property(e => e.ProductImportprice)
                .HasColumnType("decimal(20, 2)")
                .HasColumnName("product_importprice");
            entity.Property(e => e.ProductIsdeleted).HasColumnName("product_isdeleted");
            entity.Property(e => e.ProductName).HasColumnName("product_name");
            entity.Property(e => e.ProductQuantity).HasColumnName("product_quantity");
            entity.Property(e => e.ProductSellprice)
                .HasColumnType("decimal(20, 2)")
                .HasColumnName("product_sellprice");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_Brands");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Categories");
        });

        modelBuilder.Entity<SaleOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__SaleOrde__46596229CAAF8EE9");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("order_id");
            entity.Property(e => e.OrderAddress)
                .HasMaxLength(255)
                .HasColumnName("order_address");
            entity.Property(e => e.OrderDate)
                .HasColumnType("date")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderIsdeleted).HasColumnName("order_isdeleted");
            entity.Property(e => e.OrderName)
                .HasMaxLength(50)
                .HasColumnName("order_name");
            entity.Property(e => e.OrderPhone)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("order_phone");
            entity.Property(e => e.OrderTotal)
                .HasColumnType("decimal(20, 2)")
                .HasColumnName("order_total");
        });

        modelBuilder.Entity<SaleOrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderdetailId).HasName("PK__SaleOrde__59AE74118F150E21");

            entity.ToTable(tb =>
            {
                tb.HasTrigger("UPDATE_ORDER_TOTAL_SaleOrderDetails");
                tb.HasTrigger("update_product_quantity_SaleOrderDetails");
            });

            entity.Property(e => e.OrderdetailId).HasColumnName("orderdetail_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.OrderdetailIsdeleted).HasColumnName("orderdetail_isdeleted");
            entity.Property(e => e.OrderdetailPrice)
                .HasColumnType("decimal(20, 2)")
                .HasColumnName("orderdetail_price");
            entity.Property(e => e.OrderdetailQuantity).HasColumnName("orderdetail_quantity");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Order).WithMany(p => p.SaleOrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleOrders_SaleOrderDetails");

            entity.HasOne(d => d.Product).WithMany(p => p.SaleOrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Products_SaleOrderDetails");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
