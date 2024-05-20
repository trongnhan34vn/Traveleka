using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tour_Booking.Data;
using Tour_Booking.Models;

namespace Tour_Booking.Controllers;

public class DashboardController : Controller
{
    private ApplicationDbContext dbContext;
    private readonly IWebHostEnvironment webHostEnvironment;
    public DashboardController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
    {
        this.dbContext = dbContext;
        this.webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public async Task<IActionResult> Home()
    {
        int userNumber = dbContext.Users.Count();
        int tourNumber = dbContext.Tours.Count();
        int bookingNumber = dbContext.Bookings.Count();
        int currentYear = DateTime.Now.Year;
        int currentMonth = DateTime.Now.Month;
        DateTime endYear = new DateTime(currentYear + 1, 1, 1, 0, 0, 0);
        DateTime inMonth = new DateTime(currentYear, currentMonth, 1, 0, 0, 0);
        double earnings = dbContext.Bookings.Where(x => DateTime.Compare(x.CreatedDate, inMonth) >= 0 && DateTime.Compare(x.CreatedDate, endYear) < 0).Sum(x => x.Payment);
        DashboardHomeViewModel dashboardHomeViewModel = new DashboardHomeViewModel()
        {
            UserNumber = userNumber,
            TourNumber = tourNumber,
            BookingNumber = bookingNumber,
            Earnings = earnings,
        };
        return View(dashboardHomeViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> UserMng()
    {
        List<User> userList;
        string currentUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}{Request.QueryString}";
        Uri uri = new Uri(currentUrl);
        String param = HttpUtility.ParseQueryString(uri.Query).Get("search");
        if (param == null)
        {
            userList = await dbContext.Users.Include(x => x.Roles).ToListAsync();
        }
        else
        {
            userList = await dbContext.Users
            .Include(x => x.Roles)
            .Where(x => x.FullName.ToLower().Contains(param.ToLower())).ToListAsync();
        }


        List<User> responseListUser = new List<User>();
        foreach (var user in userList)
        {
            if (user.Roles.Count < 2)
            {
                responseListUser.Add(user);
            }
        }
        return View(responseListUser);
    }

    [HttpGet]
    public async Task<IActionResult> TourMng()
    {
        string currentUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}{Request.QueryString}";
        Uri uri = new Uri(currentUrl);
        String param = HttpUtility.ParseQueryString(uri.Query).Get("search");
        if (param == null)
        {
            List<Tour> tours = await dbContext.Tours.Include(x => x.Assets).Include(x => x.Destination).ToListAsync();
            return View(tours);
        }
        else
        {
            List<Tour> tours = await dbContext.Tours.Include(x => x.Assets).Include(x => x.Destination).Where(x => x.Name.ToLower().Contains(param.ToLower())).ToListAsync();
            return View(tours);
        }
    }

    [HttpGet]
    public async Task<IActionResult> AddEditTour()
    {
        AddEditFormModel addEditFormModel = new AddEditFormModel();
        string action = "add";
        string currentUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}{Request.QueryString}";

        Uri uri = new Uri(currentUrl);
        String param = HttpUtility.ParseQueryString(uri.Query).Get("Edit");
        if (param != null)
        {
            action = "edit";
            Guid editId = new Guid(param);
            List<Tour> tour = await dbContext.Tours.Include(x => x.Destination).Where(x => x.Id == editId).ToListAsync();
            if (tour != null && tour.Count != 0)
            {
                addEditFormModel.TourId = tour.ElementAt(0).Id;
                addEditFormModel.DestinationId = tour.ElementAt(0).Destination.Id;
                addEditFormModel.Name = tour.ElementAt(0).Name;
                addEditFormModel.Description = tour.ElementAt(0).Description;
                addEditFormModel.Schedule = tour.ElementAt(0).Schedule;
                addEditFormModel.Duration = tour.ElementAt(0).Duration;
                addEditFormModel.Price = tour.ElementAt(0).Price;
                addEditFormModel.Iframe = tour.ElementAt(0).Iframe;
            }
            else
            {
                return RedirectToAction("Home", "Dashboard");
            }
        }
        List<Destination> destinations = await dbContext.Destinations.ToListAsync();
        addEditFormModel.Destinations = destinations;
        addEditFormModel.Action = action;

        return View(addEditFormModel);
    }

    [HttpGet]
    public async Task<IActionResult> BookingMng()
    {
        string currentUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}{Request.QueryString}";
        Uri uri = new Uri(currentUrl);
        String param = HttpUtility.ParseQueryString(uri.Query).Get("search");
        List<Booking> bookings;
        if (param == null)
        {
            bookings = await dbContext.Bookings.Include(x => x.User).Include(x => x.Tour).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }
        else
        {
            bookings = await dbContext.Bookings.Include(x => x.User).Include(x => x.Tour).Where(x => x.Tour.Name.ToLower().Contains(param.ToLower())).OrderByDescending(x => x.CreatedDate).ToListAsync();
        }
        return View(bookings);
    }

    [HttpPost]
    public async Task<IActionResult> AddEditTour(AddEditFormModel formModel)
    {
        Destination destination = await dbContext.Destinations.FindAsync(formModel.DestinationId);
        if (destination == null)
        {
            return RedirectToAction("AddEditTour");
        }
        List<Asset> assetList = new List<Asset>();

        if (formModel.Files != null && formModel.Files.Count > 0)
        {
            foreach (var file in formModel.Files)
            {
                string folder = "assets/img/";
                var tempFileName = Guid.NewGuid().ToString() + file.FileName;
                folder += tempFileName;
                string severFoler = Path.Combine(webHostEnvironment.WebRootPath, folder);
                await file.CopyToAsync(new FileStream(severFoler, FileMode.Create));
                Asset asset = new Asset()
                {
                    Id = Guid.NewGuid(),
                    Url = folder,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                };
                assetList.Add(asset);
            }
        }



        if (formModel.Action == "add")
        {
            Tour tour = new Tour()
            {
                Id = Guid.NewGuid(),
                Name = formModel.Name,
                Description = formModel.Description,
                Schedule = formModel.Schedule,
                Duration = formModel.Duration,
                LastestDate = DateTime.Now,
                Price = formModel.Price,
                BookingNumber = 0,
                Iframe = formModel.Iframe,
                IsBusiness = true,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                Destination = destination,
                Assets = assetList
            };
            Tour tempTour = dbContext.Tours.FirstOrDefault(x => x.Id == tour.Id);
            if (tempTour == null)
            {
                dbContext.Tours.Add(tour);
                await dbContext.SaveChangesAsync();
            }


        }
        else
        {
            Tour tour = await dbContext.Tours.Include(x => x.Destination).Include(x => x.Assets).Where(x => x.Id == formModel.TourId).FirstOrDefaultAsync();
            if (tour != null)
            {
                tour.Name = formModel.Name;
                tour.Description = formModel.Description;
                tour.Schedule = formModel.Schedule;
                tour.Duration = formModel.Duration;
                tour.Price = formModel.Price;
                tour.Iframe = formModel.Iframe;
                tour.UpdatedDate = DateTime.Now;
                tour.Destination = destination;
                tour.Assets = assetList;
                await dbContext.SaveChangesAsync();
            }

        }




        return RedirectToAction("TourMng");
    }

    public async Task<IActionResult> StopBusiness(Guid id)
    {
        var tour = await dbContext.Tours.Include(x => x.Destination).Include(x => x.Assets).Where(x => x.Id == id).FirstOrDefaultAsync();
        if (tour != null)
        {
            tour.IsBusiness = !tour.IsBusiness;
            tour.UpdatedDate = DateTime.Now;
            await dbContext.SaveChangesAsync();
        }
        return RedirectToAction("TourMng");
    }

}