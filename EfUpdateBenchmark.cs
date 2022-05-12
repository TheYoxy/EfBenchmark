namespace EfBenchmark;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Microsoft.EntityFrameworkCore;

[MemoryDiagnoser]
[RankColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
[Config(typeof(Config))]

[ShortRunJob]
[MediumRunJob]
[LongRunJob]
[VeryLongRunJob]
[MarkdownExporterAttribute.Default]
[MarkdownExporterAttribute.GitHub]
[RPlotExporter]
[AsciiDocExporter]
public class EfUpdateBenchmark {
  private AppContext? _context;

  [GlobalSetup]
  public async Task GlobalSetup() {
    var context = new AppContext();
    await context.Database.EnsureCreatedAsync();
  }

  [GlobalCleanup]
  public async Task GlobalCleanup() {
    var context = new AppContext();
    await context.Database.EnsureDeletedAsync();
    await context.DisposeAsync();
  }

  [IterationSetup]
  public void IterationSetup() {
    _context = new AppContext();
  }

  [IterationCleanup]
  public void IterationCleanup() {
    _context?.Dispose();
    _context = null;
  }


  [Benchmark(Baseline = true)]
  [BenchmarkCategory("EF", "Update")]
  public async Task<Data> UpdateWithTrackingAsync() {
    var data = await _context!.Set<Data>().AsTracking().FirstAsync(x => x.Id == 2);
    data.Name = "Updated";
    await _context.SaveChangesAsync();
    return data;
  }

  [Benchmark]
  [BenchmarkCategory("EF", "Update")]
  public async Task<Data> UpdateWithTrackingAndUpdateAsync() {
    var dbSet = _context!.Set<Data>();
    var data = await dbSet.AsTracking().FirstAsync(x => x.Id == 2);
    data.Name = "Updated";
    dbSet.Update(data);
    await _context.SaveChangesAsync();
    return data;
  }

  [Benchmark]
  [BenchmarkCategory("EF", "Update")]
  public async Task<Data> UpdateWithNoTrackingAndUpdateAsync() {
    var dbSet = _context!.Set<Data>();
    var data = await dbSet.AsNoTracking().FirstAsync(x => x.Id == 2);
    data.Name = "Updated";
    dbSet.Update(data);
    await _context.SaveChangesAsync();
    return data;
  }

  [Benchmark]
  [BenchmarkCategory("EF", "Update")]
  public async Task<Data> UpdateWithNoTrackingAndIdentityResolutionAndUpdateAsync() {
    var dbSet = _context!.Set<Data>();
    var data = await dbSet.AsNoTracking().FirstAsync(x => x.Id == 2);
    data.Name = "Updated";
    dbSet.Update(data);
    await _context.SaveChangesAsync();
    return data;
  }
}