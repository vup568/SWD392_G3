using Microsoft.EntityFrameworkCore;

namespace SWD392_MVC.Models;

public partial class OnlineShopContext : DbContext
{
    public OnlineShopContext(DbContextOptions<OnlineShopContext> options) : base(options) { }

    public virtual DbSet<Feedback>        Feedbacks         { get; set; }
    public virtual DbSet<Order>           Orders            { get; set; }
    public virtual DbSet<OrderItem>       OrderItems        { get; set; }
    public virtual DbSet<Post>            Posts             { get; set; }
    public virtual DbSet<Product>         Products          { get; set; }
    public virtual DbSet<ProductCategory> ProductCategories { get; set; }
    public virtual DbSet<Role>            Roles             { get; set; }
    public virtual DbSet<Setting>         Settings          { get; set; }
    public virtual DbSet<Slider>          Sliders           { get; set; }
    public virtual DbSet<User>            Users             { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.HasOne(d => d.Product).WithMany(p => p.Feedbacks).OnDelete(DeleteBehavior.ClientSetNull);
            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks).OnDelete(DeleteBehavior.ClientSetNull);
            // OrderId: optional FK – a feedback may be tied to a completed order
            entity.HasOne(d => d.Order).WithMany(p => p.Feedbacks)
                  .HasForeignKey(d => d.OrderId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Submitted");
            entity.HasOne(d => d.Sale).WithMany(p => p.OrderSales).OnDelete(DeleteBehavior.ClientSetNull);
            entity.HasOne(d => d.User).WithMany(p => p.OrderUsers).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems);
            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems);
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.HasOne(d => d.Author).WithMany(p => p.Posts);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.HasOne(d => d.Category).WithMany(p => p.Products);
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<Setting>(entity =>
        {
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<Slider>(entity =>
        {
            entity.Property(e => e.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.HasOne(d => d.Role).WithMany(p => p.Users).OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
