using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Ingredients")]
public class Ingredient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Amount { get; set; }
    
    public int RecipeId { get; set; }
    public Recipe Recipe { get; set; }
    
    public int MeasurementUnitId { get; set; }
    public MeasurementUnit MeasurementUnit { get; set; }
}
