using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tour_Booking.Models;

public class Booking
{

    [ForeignKey("User")]
    public Guid UserId { get; set; }


    [ForeignKey("Tour")]
    public Guid TourId { get; set; }
    public Guid Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string CustomerName { get; set; }

    [Required]
    [StringLength(10, MinimumLength = 1)]
    public string PhoneNumber { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime TravelDate { get; set; }
    public double Payment { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreatedDate { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime UpdatedDate { get; set; }
    public Tour Tour { get; set; } = null!;
    public User User { get; set; } = null!;


}