namespace ef_update;

using System.ComponentModel.DataAnnotations.Schema;

public class Data {
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }

  public string Name { get; set; } = null!;
  public ICollection<InnerData> InnerDatas { get; set; } = new HashSet<InnerData>();
}