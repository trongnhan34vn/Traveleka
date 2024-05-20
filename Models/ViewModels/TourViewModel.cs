using System.ComponentModel.DataAnnotations;

namespace Tour_Booking.Models;

public class TourViewModel{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Schedule {get; set; }
    public double Duration { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime LastestDate { get; set; }
    public double Price { get; set; }
    public int BookingNumber { get; set; }
    public string Iframe { get; set; }
    public bool IsBusiness { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreatedDate { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime UpdatedDate { get; set; }

    public Destination Destination { get; set; }
    public List<Asset> Assets { get; set; } = new List<Asset>();
}