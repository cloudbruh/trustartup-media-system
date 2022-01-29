using System.ComponentModel.DataAnnotations.Schema;

namespace CloudBruh.Trustartup.MediaSystem.Models;

public class Media
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public bool IsPublic { get; set; }
    public string? Link { get; set; }
    public MediaType Type { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }
}