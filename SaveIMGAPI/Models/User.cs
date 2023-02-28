namespace SaveIMGAPI.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public List<string> FotoPath { get; set; }
}