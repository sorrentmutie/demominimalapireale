using DemoMinimalAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace DemoMinimalAPI.DTO;

public class CategoryDTO
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = "";

    public string? Description { get; set; }

    public int NumberOfProducts { get; set; }
}

public class CategoryCreateDTO
{
    [Required(), MaxLength(15)]
    public string Name { get; set; } = "";
    public string? Description { get; set; }

}
