using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace JewelryStore_Prokofeva.ModelsDb;

public partial class DiplomDb2Context : DbContext
{
    public DiplomDb2Context()
    {
    }

    public DiplomDb2Context(DbContextOptions<DiplomDb2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ForWho> ForWhos { get; set; }

    public virtual DbSet<Insertion> Insertions { get; set; }

    public virtual DbSet<InsertionsCharacteristic> InsertionsCharacteristics { get; set; }

    public virtual DbSet<InsertionsDetail> InsertionsDetails { get; set; }

    public virtual DbSet<JewelryItem> JewelryItems { get; set; }

    public virtual DbSet<JewelrySize> JewelrySizes { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-94UBSDB\\SQLEXPRESS;Database=DiplomDB_2;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ForWho>(entity =>
        {
            entity.ToTable("ForWho");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Insertion>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<InsertionsCharacteristic>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Clarity).HasColumnName("clarity");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .HasColumnName("color");
            entity.Property(e => e.ColorGrade).HasColumnName("color_grade");
            entity.Property(e => e.CutOgranka).HasColumnName("cut_ogranka");
            entity.Property(e => e.Dimensions)
                .HasMaxLength(50)
                .HasColumnName("dimensions");
            entity.Property(e => e.InsertionsDetailsId).HasColumnName("insertions_details_id");
            entity.Property(e => e.ShapeForm)
                .HasMaxLength(50)
                .HasColumnName("shape_form");
            entity.Property(e => e.WeightCarat).HasColumnName("weight_carat");

            entity.HasOne(d => d.InsertionsDetails).WithMany(p => p.InsertionsCharacteristics)
                .HasForeignKey(d => d.InsertionsDetailsId)
                .HasConstraintName("FK_InsertionsCharacteristics_InsertionsDetails");
        });

        modelBuilder.Entity<InsertionsDetail>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.InsertionId).HasColumnName("insertion_id");
            entity.Property(e => e.JewelryItemId).HasColumnName("jewelry_item_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Insertion).WithMany(p => p.InsertionsDetails)
                .HasForeignKey(d => d.InsertionId)
                .HasConstraintName("FK_InsertionsDetails_Insertions");

            entity.HasOne(d => d.JewelryItem).WithMany(p => p.InsertionsDetails)
                .HasForeignKey(d => d.JewelryItemId)
                .HasConstraintName("FK_InsertionsDetails_JewelryItems");
        });

        modelBuilder.Entity<JewelryItem>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ApproximateWeight).HasColumnName("approximate_weight");
            entity.Property(e => e.Articul)
                .HasMaxLength(50)
                .HasColumnName("articul");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.ForWhoId).HasColumnName("forWho_id");
            entity.Property(e => e.MaterialId).HasColumnName("material_id");
            entity.Property(e => e.PhotoUrl).HasColumnName("photo_url");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("price");
            entity.Property(e => e.PriceDiscounr)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("price_discounr");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.StockQuantity).HasColumnName("stock_quantity");
            entity.Property(e => e.Title).HasColumnName("title");

            entity.HasOne(d => d.Category).WithMany(p => p.JewelryItems)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_JewelryItems_Categories");

            entity.HasOne(d => d.ForWho).WithMany(p => p.JewelryItems)
                .HasForeignKey(d => d.ForWhoId)
                .HasConstraintName("FK_JewelryItems_ForWho");

            entity.HasOne(d => d.Material).WithMany(p => p.JewelryItems)
                .HasForeignKey(d => d.MaterialId)
                .HasConstraintName("FK_JewelryItems_Materials");
        });

        modelBuilder.Entity<JewelrySize>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JewelryS__3213E83F3DC5B18B");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.JewelryItemId).HasColumnName("jewelry_item_id");
            entity.Property(e => e.Size).HasColumnName("size");
            entity.Property(e => e.StockQuantity).HasColumnName("stock_quantity");

            entity.HasOne(d => d.JewelryItem).WithMany(p => p.JewelrySizes)
                .HasForeignKey(d => d.JewelryItemId)
                .HasConstraintName("FK__JewelrySi__jewel__5CD6CB2B");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.ToTable("Purchase");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.JewelryItemsId).HasColumnName("jewelryItems_id");
            entity.Property(e => e.PurchaseDate).HasColumnName("purchase_date");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("total_price");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.JewelryItems).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.JewelryItemsId)
                .HasConstraintName("FK_Purchase_JewelryItems");

            entity.HasOne(d => d.User).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Purchase_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");
            entity.Property(e => e.Money)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("money");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
