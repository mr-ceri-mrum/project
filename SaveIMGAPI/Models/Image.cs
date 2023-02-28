namespace SaveIMGAPI.Models;

public class Image
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public IFormFile File { get; set; }
    
}