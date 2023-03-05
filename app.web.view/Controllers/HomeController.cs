using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using app.web.view.Models;
using BusinesDAL.Abstract;
using app.common.DTO;

namespace app.web.view.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IFacultyBusinessService _facultyBusinessService;

    public HomeController(ILogger<HomeController> logger, IFacultyBusinessService facultyBusinessService)
    {
        _logger = logger;
        _facultyBusinessService = facultyBusinessService;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<FacultyDTO> faculties = await _facultyBusinessService.GetFacultiesAsync();
        return View(faculties);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    
}
