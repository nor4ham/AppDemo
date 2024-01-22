using AppDemo.Data;
using AppDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace AppDemo.Controllers;
[ApiController]
[Route("[controller]")]
public class DriversController : ControllerBase
{
    private readonly ILogger<DriversController> _logger;
    private readonly ApiDbContext _context;
    public DriversController(
        ILogger<DriversController> logger,
        ApiDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    [HttpGet(Name = "GetAllDrivers")]
    public async Task<IActionResult> Get()
    {
        var driver =  new Driver()
        {
            DriverNb = 44,
            Name = "Lewis Hamilton"
        };
        await _context.Drivers.AddAsync(driver);
        await _context.SaveChangesAsync();
        var drivers = await _context.Drivers.ToListAsync();
       return Ok(drivers);
    }
}
