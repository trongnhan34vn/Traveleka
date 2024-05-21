using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Tour_Booking.Models;

public class Asset {

    [ForeignKey("Tour")]
    public Guid TourId { get; set; }

    public Guid Id { get; set; }
    public string Url { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreatedDate { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime UpdatedDate { get; set; }
    
    [JsonIgnore]
    public Tour Tour{ get; set; } = null!;


}