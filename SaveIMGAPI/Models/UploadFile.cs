namespace SaveIMGAPI.Models
{
    public class UploadFile
    {
        public int Id { get; set; }
        public IFormFile File { get; set; }
        public string Name { get; set; }
    }    
}
