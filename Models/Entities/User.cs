namespace Tour_Booking.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

public class User
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 6)]
    public string UserName { get; set; }


    [Required]
    [StringLength(50, MinimumLength = 6)]
    public string Password { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string FullName { get; set; }

    [Required]
    [StringLength(10, MinimumLength = 1)]
    public string PhoneNumber { get; set; }

    public bool IsBlocked { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreatedDate { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime UpdatedDate { get; set; }

    public List<Role> Roles { get; set; }
    [JsonIgnore]
    public List<Booking> Bookings { get; set; } = new List<Booking>();
}