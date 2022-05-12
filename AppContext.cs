namespace EfBenchmark;

using Microsoft.EntityFrameworkCore;

public class AppContext : DbContext {
  /// <inheritdoc />
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    base.OnConfiguring(optionsBuilder);
    optionsBuilder.UseSqlite("Data Source=ef_update.db");
  }

  /// <inheritdoc />
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<Data>(builder => {
      builder.HasMany(x => x.InnerDatas).WithOne(x => x.Data).HasForeignKey(data => data.DataFk);
      builder.HasData(new() {
          Id = 1, Name = "Data1"
        },
        new() { Id = 2, Name = "Data2" },
        new() { Id = 3, Name = "Data3" },
        new() { Id = 4, Name = "Data4" },
        new() { Id = 5, Name = "Data5" });
    });
    modelBuilder.Entity<InnerData>()
      .HasData(new() { Id = 1, Name = "InnerData1", DataFk = 1 },
        new() { Id = 2, Name = "InnerData2", DataFk = 1 }
      );
  }

  public DbSet<Data> Datas { get; set; } = null!;
  public DbSet<InnerData> InnerDatas { get; set; } = null!;
}