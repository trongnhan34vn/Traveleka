namespace Tour_Booking.Models;

public class BookingFormModel
{
    public Guid UserId { get; set; }
    public Guid TourId { get; set; }
    public string? CustomerName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime TravelDate { get; set; }
    public double Payment { get; set; }
}