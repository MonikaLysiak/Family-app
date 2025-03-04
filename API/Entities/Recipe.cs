using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Recipes")]
public class Recipe
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public TimeSpan? PreparationTime { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    
    public int FamilyId { get; set; }
    public Family Family { get; set; }
    
    public int AuthorId { get; set; }
    public AppUser Author { get; set; }

    public List<Device> Devices { get; set; } = [];
    public List<Ingredient> Ingredients { get; set; } = [];
}
