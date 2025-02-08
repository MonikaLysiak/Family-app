using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("MeasurementUnits")]
public class MeasurementUnit
{
    public int Id { get; set;}
    public string Name { get; set;}

    public ICollection<Ingredient> Ingredients { get; set; }
}
