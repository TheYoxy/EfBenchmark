namespace EfBenchmark;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Microsoft.EntityFrameworkCore;

[MemoryDiagnoser]
[RankColumn]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[Config(typeof(Config))]
public class Benchmark {
  private AppContext? _context;
  public int Id { get; set; } = 1;

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
  [BenchmarkCategory("Update")]
  public async Task<Data> UpdateWithTrackingAsync() {
    var data = await _context!.Set<Data>().AsTracking().FirstAsync(x => x.Id == Id);
    data.Name = "Updated";
    await _context.SaveChangesAsync();
    return data;
  }

  [Benchmark]
  [BenchmarkCategory("Update")]
  public async Task<Data> UpdateWithTrackingAndUpdateAsync() {
    var dbSet = _context!.Set<Data>();
    var data = await dbSet.AsTracking().FirstAsync(x => x.Id == Id);
    data.Name = "Updated";
    dbSet.Update(data);
    await _context.SaveChangesAsync();
    return data;
  }

  [Benchmark]
  [BenchmarkCategory("Update")]
  public async Task<Data> UpdateWithNoTrackingAndUpdateAsync() {
    var dbSet = _context!.Set<Data>();
    var data = await dbSet.AsNoTracking().FirstAsync(x => x.Id == Id);
    data.Name = "Updated";
    dbSet.Update(data);
    await _context.SaveChangesAsync();
    return data;
  }

  [Benchmark]
  [BenchmarkCategory("Update")]
  public async Task<Data> UpdateWithNoTrackingAndIdentityResolutionAndUpdateAsync() {
    var dbSet = _context!.Set<Data>();
    var data = await dbSet.AsNoTracking().FirstAsync(x => x.Id == Id);
    data.Name = "Updated";
    dbSet.Update(data);
    await _context.SaveChangesAsync();
    return data;
  }

  [Benchmark(Baseline = true)]
  [BenchmarkCategory("Select", "Single")]
  public async Task<Data> SelectWithTrackingAsync() {
    return await _context!.Set<Data>().AsTracking().FirstAsync(x => x.Id == Id);
  }


  [Benchmark]
  [BenchmarkCategory("Select", "Single")]
  public async Task<Data> SelectWithNoTrackingAndSelectAsync() {
    return await _context!.Set<Data>().AsNoTracking().FirstAsync(x => x.Id == Id);
  }

  [Benchmark]
  [BenchmarkCategory("Select", "Single")]
  public async Task<Data> SelectWithNoTrackingAndIdentityResolutionAndUpdateAsync() {
    return await _context!.Set<Data>().AsNoTrackingWithIdentityResolution().FirstAsync(x => x.Id == Id);
  }


  [Benchmark(Baseline = true)]
  [BenchmarkCategory("Select", "Multiple")]
  public async Task<(Data data, Data data2)> SelectMultiplesWithTrackingAsync() {
    var data = await _context!.Set<Data>().AsTracking().FirstAsync(x => x.Id == Id);
    var data2 = await _context!.Set<Data>().AsTracking().FirstAsync(x => x.Id == Id);
    return (data, data2);
  }


  [Benchmark]
  [BenchmarkCategory("Select", "Multiple")]
  public async Task<(Data data, Data data2)> SelectMultiplesWithNoTrackingAndSelectAsync() {
    var data = await _context!.Set<Data>().AsNoTracking().FirstAsync(x => x.Id == Id);
    var data2 = await _context!.Set<Data>().AsNoTracking().FirstAsync(x => x.Id == Id);
    return (data, data2);
  }

  [Benchmark]
  [BenchmarkCategory("Select", "Multiple")]
  public async Task<(Data data, Data data2)> SelectMultiplesWithNoTrackingAndIdentityResolutionAndUpdateAsync() {
    var data = await _context!.Set<Data>().AsNoTrackingWithIdentityResolution().FirstAsync(x => x.Id == Id);
    var data2 = await _context!.Set<Data>().AsNoTrackingWithIdentityResolution().FirstAsync(x => x.Id == Id);
    return (data, data2);
  }
}