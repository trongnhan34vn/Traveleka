using System.Web;
using Azure;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tour_Booking.Data;
using Tour_Booking.Models;
using Newtonsoft.Json;


namespace Tour_Booking.Controllers;

public class UserController : Controller
{

    private ApplicationDbContext dbContext;

    public UserController(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Member()
    {
        string currentUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}{Request.Path}{Request.QueryString}";
        string currentPath = $"{Request.Path}";

        Uri uri = new Uri(currentUrl);
        string param = HttpUtility.ParseQueryString(uri.Query).Get("action");
        if (currentPath == "/User/Member")
        {
            if (param == "SignUp" || param == "SignIn")
            {
                ViewData["Param"] = param;
                return View();
            }
        }

        return RedirectToAction("Index", "Home");




    }

    [HttpGet]
    public async Task<IActionResult> BookingList(Guid id)
    {
        List<Booking> bookings = await dbContext.Bookings
        .Include(x => x.User)
        .Include(x => x.Tour).ThenInclude(to => to.Assets)
        .Where(x => x.User.Id == id)
        .OrderByDescending(x => x.CreatedDate)
        .ToListAsync();
        return View(bookings);
    }

    [HttpPost]
    public async Task<IActionResult> DoSignUp(SignInUpViewModel viewModel)
    {
        var roleCustomer = await dbContext.Roles.FirstOrDefaultAsync(x => x.RoleName == "CUSTOMER");
        // var roleAdmin = await dbContext.Roles.FirstOrDefaultAsync(x => x.RoleName == "ADMIN");
        var existUserName = await dbContext.Users.FirstOrDefaultAsync(x => x.UserName == viewModel.UserName);
        var existPhoneNumber = await dbContext.Users.FirstOrDefaultAsync(x => x.PhoneNumber == viewModel.PhoneNumber);

        if (existUserName != null)
        {
            TempData["ErrorMessage"] = "OOP! Existed Username";
            return Redirect("/User/Member?action=SignUp");
        }
        else if (existPhoneNumber != null)
        {
            TempData["ErrorMessage"] = "OOP! Existed Phone number";
            return Redirect("/User/Member?action=SignUp");
        }

        // if (roleAdmin == null) {
        //    return RedirectToAction("Index", "Home");
        // }

        if (roleCustomer == null)
        {
            return RedirectToAction("Index", "Home");
        }

        List<Role> roles = new List<Role>();
        roles.Add(roleCustomer);
        // roles.Add(roleAdmin);


        var user = new User
        {
            UserName = viewModel.UserName,
            Password = viewModel.Password,
            FullName = viewModel.FullName,
            PhoneNumber = viewModel.PhoneNumber,
            IsBlocked = false,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now,
            Roles = roles,
            Bookings = new List<Booking>(),
        };

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Sign Up successfully!";
        return Redirect("/User/Member?action=SignIn");
    }

    [HttpPost]
    public async Task<IActionResult> DoSignIn(SignInUpViewModel viewModel)
    {

        var userList = dbContext.Users.Include(x => x.Roles).Where(x => x.UserName == viewModel.UserName).ToList();

        if (userList.Count == 0 || userList.ElementAt(0).Password != viewModel.Password)
        {
            TempData["ErrorMessage"] = "OOP! Wrong Username or Password";
            return Redirect("/User/Member?action=SignIn");
        }

        if (userList.ElementAt(0).IsBlocked)
        {
            TempData["ErrorMessage"] = "OOP! User Blocked";
            return Redirect("/User/Member?action=SignIn");
        }

        TempData["SuccessMessage"] = "Welcome";

        var userResponse = new UserResponseViewModel()
        {
            Id = userList.ElementAt(0).Id,
            FullName = userList.ElementAt(0).FullName,
            PhoneNumber = userList.ElementAt(0).PhoneNumber,
            Roles = userList.ElementAt(0).Roles,
        };

        var userJson = JsonConvert.SerializeObject(userResponse, Formatting.None, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        TempData["CurrentUserJson"] = userJson;

        foreach (var role in userList.ElementAt(0).Roles)
        {
            if (role.RoleName == "ADMIN")
            {
                return RedirectToAction("Home", "Dashboard");
            }
        }

        return Redirect("/Home");

    }

    [HttpGet]
    public async Task<IActionResult> BlockUser(Guid id)
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user != null)
        {
            user.IsBlocked = !user.IsBlocked;
            user.UpdatedDate = DateTime.Now;
            await dbContext.SaveChangesAsync();
        }
        return RedirectToAction("UserMng", "Dashboard");
    }


    [HttpGet]
    public async Task<IActionResult> Profile(Guid id)
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user != null)
        {
            return View(user);
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Profile(SignInUpViewModel viewModel)
    {
        var user = await dbContext.Users.FindAsync(viewModel.Id);
        if (user != null)
        {
            if (viewModel.FullName != null && user.FullName != viewModel.FullName)
            {
                user.FullName = viewModel.FullName;
            }

            if (viewModel.PhoneNumber != null && user.PhoneNumber != viewModel.PhoneNumber)
            {
                var tempUser = await dbContext.Users.FirstOrDefaultAsync(x => x.PhoneNumber == viewModel.PhoneNumber);
                if (tempUser == null)
                {
                    user.PhoneNumber = viewModel.PhoneNumber;
                }
                else
                {
                    TempData["Errormessage"] = "OOP! Existed phone number!";
                    return RedirectToAction("Profile");
                }
            }

            if (viewModel.Password != null && user.Password != viewModel.Password)
            {
                user.Password = viewModel.Password;
            }

            user.UpdatedDate = DateTime.Now;
            await dbContext.SaveChangesAsync();
            TempData["SuccessMessage"] = "Update successfully!";
            return RedirectToAction("Profile");


        }

        return View(viewModel);

    }

}