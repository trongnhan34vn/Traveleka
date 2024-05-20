namespace Tour_Booking.Models;
using System.ComponentModel.DataAnnotations;

public class Role {
    public Guid Id { get; set; }
    public string RoleName { get; set; }
    public List<User> Users { get; set; }
    
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreatedDate { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime UpdatedDate { get; set; }
}