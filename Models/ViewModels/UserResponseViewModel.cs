namespace Tour_Booking.Models;

public class UserResponseViewModel {
    public Guid Id { get; set;}
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public List<Role> Roles { get; set; }
}