namespace EfBenchmark;

using System.ComponentModel.DataAnnotations.Schema;

public class InnerData {
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  public string Name { get; set; } = null!;
  public Data Data { get; set; } = null!;
  public int DataFk { get; set; }
}