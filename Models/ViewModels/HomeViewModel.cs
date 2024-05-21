namespace Tour_Booking.Models;

public class HomeViewModel
{
  public List<Tour>? mostBookingInteriorTours { get; set; }
  public List<Tour>? aboardTours { get; set; }
  public List<Destination>? interiorDestinations { get; set; }
  public List<Destination>? aboardDestinations { get; set; }
  public List<Tour>? OrderTours { get; set; }
  public string? Action {get; set;}
}