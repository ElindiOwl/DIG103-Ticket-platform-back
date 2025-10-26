namespace DIG103_Ticket_platform_back.Model;

public class Image
{
    public int Id { get; set; }
    public string ImagePath { get; set; }
    public string ContentType { get; set; }
    public DateTime UploadedAt { get; set; }
}