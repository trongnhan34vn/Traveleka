namespace Tour_Booking.Models;

public class AddEditFormModel
{
    public Guid DestinationId { get; set; }
    public Guid? TourId { get; set; }
    public string? Name { get; set; }
    public string? Action { get; set; }
    public string? Description { get; set; }
    public string? Schedule { get; set; }
    public double Duration { get; set; }
    public double Price { get; set; }
    public string? Iframe { get; set; }
    public Destination? Destination { get; set; }
    public List<Destination>? Destinations { get; set; }
    public List<IFormFile>? Files { get; set; }
}