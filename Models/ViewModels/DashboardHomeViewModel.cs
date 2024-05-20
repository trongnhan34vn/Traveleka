using System.Security.Principal;

namespace Tour_Booking.Models;

public class DashboardHomeViewModel
{
    public int UserNumber { get; set; }
    public int TourNumber { get; set; }
    public int BookingNumber { get; set; }
    public double Earnings { get; set; }
}