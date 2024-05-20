namespace Tour_Booking.Models;
using System.ComponentModel.DataAnnotations;

public class UserViewModel {

    public string UserName { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }

    public bool IsBlocked { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreatedDate { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime UpdatedDate { get; set; }

    public List<Role> Roles { get; set; }
    public List<Booking> Bookings{ get; set; }
    
}