using CachingWebApi.Data;
using CachingWebApi.Models;
using CachingWebApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CachingWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DriversController : ControllerBase
{
    private readonly ILogger<DriversController> _logger;
    private readonly ICacheService _cacheService;
    private readonly AppDbContext _context;

    public DriversController(ILogger<DriversController> logger,
        ICacheService cacheService,
        AppDbContext context)
    {
        _logger = logger;
        _cacheService = cacheService;
        _context = context;
    }

    [HttpGet("drivers")]
    public async Task<IActionResult> Get()
    {
        var cacheData = _cacheService.GetData<IEnumerable<Driver>>("drivers");

        if (cacheData != null && cacheData.Count() > 0)
        {
            return Ok(cacheData);
        }

        cacheData = await _context.Drivers.ToListAsync();
        var expiryTime = DateTimeOffset.Now.AddSeconds(30);
        _cacheService.SetData<IEnumerable<Driver>>("drivers", cacheData, expiryTime);

        return Ok(cacheData);
    }

    [HttpPost("AddDriver")]
    public async Task<IActionResult> Post(Driver value)
    {
        var addObj = await _context.Drivers.AddAsync(value);

        var expiryTime = DateTimeOffset.Now.AddSeconds(30);
        _cacheService.SetData<Driver>($"driver{value.Id}", addObj.Entity, expiryTime);
        await _context.SaveChangesAsync();

        return Ok(addObj.Entity);
    }

    [HttpDelete("DeleteDriver")]
    public async Task<IActionResult> Delete(int id)
    {
        var exist = await _context.Drivers.FirstOrDefaultAsync(x => x.Id == id);
        if (exist != null)
        {
            _context.Remove(exist);
            _cacheService.RemoveData($"driver{id}");
            await _context.SaveChangesAsync();
            return NoContent();
        }
        return NotFound();
    }
}
