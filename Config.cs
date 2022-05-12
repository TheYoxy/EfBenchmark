namespace ef_update;

using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;

public class Config : ManualConfig {
  public Config() {
    AddColumn(StatisticColumn.P0, StatisticColumn.P25, StatisticColumn.P50, StatisticColumn.P67, StatisticColumn.P80,
      StatisticColumn.P85, StatisticColumn.P90, StatisticColumn.P95, StatisticColumn.P100,
      StatisticColumn.OperationsPerSecond);
  }
}