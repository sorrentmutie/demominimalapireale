using System.ComponentModel.DataAnnotations;

namespace DemoMinimalAPI.Configurations;

public class MyApp
{
    [Required(ErrorMessage = "Message is required"), MaxLength(50)]
    public string Message { get; set; } = "Ciao";
    public int PageSize { get; set; } = 10;
    public bool EnableFeatureX { get; set; } = false;
}
