using System.ComponentModel.DataAnnotations;

public class ConcilliationReqDTO()
{
  [Required]
  public required List<BaseJson?> DatabaseToFile { get; set; }
  
  [Required]
  public required List<BaseJson?> FileToDatabase { get; set; }

  [Required]
  public required List<BaseJson?> DifferentStatus { get; set; }
}

public class BaseJson(int id, string status)
{
    public required int Id { get; set; } = id;
    public required string Status { get; set; } = status;
}