using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("FamilyPhotos")]
public class FamilyPhoto
{
    public int Id { get; set; }
    public string Url { get; set; }
    public bool IsMain { get; set; }
    public string PublicId { get; set; }

    public int FamilyId { get; set; }
    public Family Family { get; set; }
    
    public int AuthorId { get; set; }
    public AppUser Author { get; set; }
}
