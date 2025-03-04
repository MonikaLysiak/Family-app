using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Devices")]
public class Device
{
    public int Id { get; set; }
    public string? Name { get; set; }
    
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
}
