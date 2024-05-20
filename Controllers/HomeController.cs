using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tour_Booking.Data;
using Tour_Booking.Models;

namespace Tour_Booking.Controllers;
public class HomeController : Controller
{
    private ApplicationDbContext dbContext;
    private readonly ILogger<HomeController> _logger;
    public HomeController(ApplicationDbContext dbContext, ILogger<HomeController> logger)
    {
        this.dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IActionResult> Index(Guid interiorDestinationId)
    {
        try
        {
            _logger.LogInformation("Start Render Home Page");
            List<Tour> mostBookingInteriorTours = await dbContext.Tours
                            .Include(x => x.Assets)
                            .Where(x => x.Destination.IsInterior)
                            .OrderByDescending(x => x.BookingNumber)
                            .Take(5)
                            .ToListAsync();

            var destinationIds = mostBookingInteriorTours.Select(d => d.DestinationId).Distinct().ToList();
            List<Destination> interiorDestinations = await dbContext.Destinations
                                        .Where(d => destinationIds.Contains(d.Id))
                                        .Take(3)
                                        .ToListAsync();

            List<Tour> aboardTours = await dbContext.Tours
                                        .Include(x => x.Assets)
                                        .Where(x => x.Destination.IsInterior == false)
                                        .Take(5)
                                        .ToListAsync();

            List<Destination> aboardDestinations = await dbContext.Destinations
                                                        .Where(d => d.IsInterior == false)
                                                        .Take(3)
                                                        .ToListAsync();

            if (interiorDestinationId == Guid.Empty || interiorDestinationId == null)
            {
                var interiorDestinationFirstEntry = interiorDestinations.Take(1).FirstOrDefault();
                if (interiorDestinationFirstEntry != null)
                {
                    interiorDestinationId = interiorDestinationFirstEntry.Id;
                }
            }

            List<Tour> mostBookingInteriorTourByDestination =  await dbContext.Tours
                            .Include(x => x.Assets)
                            .Where(x => x.Destination.IsInterior && x.Destination.Id == interiorDestinationId)
                            .OrderByDescending(x => x.BookingNumber)
                            .Take(5)
                            .ToListAsync();;

            _logger.LogInformation("End Render Home Page");


            return View(new HomeViewModel
            {
                mostBookingInteriorTours = mostBookingInteriorTourByDestination,
                aboardTours = aboardTours,
                interiorDestinations = interiorDestinations,
                aboardDestinations = aboardDestinations
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return View();
        }
    }



    [HttpGet]
    public async Task<IActionResult> Detail(Guid id)
    {
        try
        {
            _logger.LogInformation("Start Index Detail Tour: " + id);
            var tour = await dbContext.Tours.Include(x => x.Destination).Include(x => x.Assets).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (tour == null)
            {
                _logger.LogInformation("Tour not found!");
                return View(new DetailTourViewModel
                {
                    tour = null
                });
            }
            _logger.LogInformation("End Index Detail Tour");
            return View(new DetailTourViewModel
            {
                tour = tour
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return View();
        }
    }

    [HttpGet]
    public IActionResult Booking()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Booking(BookingFormModel bookingFormModel)
    {
        var user = await dbContext.Users.FindAsync(bookingFormModel.UserId);
        var tour = await dbContext.Tours.FindAsync(bookingFormModel.TourId);
        if (user == null || tour == null)
        {
            return RedirectToAction("Index");
        }

        Booking booking = new Booking()
        {
            User = user,
            Tour = tour,
            CustomerName = bookingFormModel.CustomerName,
            PhoneNumber = bookingFormModel.PhoneNumber,
            TravelDate = bookingFormModel.TravelDate,
            Payment = bookingFormModel.Payment,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now,
        };
        tour.BookingNumber = tour.BookingNumber += 1;
        tour.UpdatedDate = DateTime.Now;
        await dbContext.Bookings.AddAsync(booking);
        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Booking Successfully!";

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
