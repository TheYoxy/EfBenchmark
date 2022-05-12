namespace EfBenchmark;

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;

public class Config : ManualConfig {
  public Config() {
    AddColumn(StatisticColumn.P25, StatisticColumn.P50, StatisticColumn.P95, StatisticColumn.P100,
      StatisticColumn.OperationsPerSecond);
  }
}