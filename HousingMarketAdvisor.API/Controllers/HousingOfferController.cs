using Bogus;
using Microsoft.EntityFrameworkCore;
using HousingMarketAdvisor.DAL.SqlServer.EfModels;
using HousingMarketAdvisor.DAL.SqlServer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace HousingMarketAdvisor.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HousingOfferController : ControllerBase
{
    private readonly HousingRepository _context;
    private readonly ILogger<HousingOfferController> _logger;

    public HousingOfferController(
        ILogger<HousingOfferController> logger,
        HousingRepository context
    )
    {
        _logger = logger;
        _context = context;

        _context.Database.EnsureCreated();

        // if empty add records
        if (_context.HousingOffers.Any()) return;
        var housingOffersGenerator = new Faker<HousingOffer>()
            .RuleFor(ho => ho.Title, f => f.Lorem.Sentence())
            .RuleFor(ho => ho.Price, f => decimal.Parse(f.Commerce.Price(100000, 400000, 2)))
            .RuleFor(ho => ho.Description, f => f.Lorem.Paragraph())
            .RuleFor(ho => ho.Address, f => f.Address.FullAddress())
            .RuleFor(ho => ho.City, f => f.Address.City())
            .RuleFor(ho => ho.ZipCode, f => f.Address.ZipCode())
            .RuleFor(ho => ho.Country, f => f.Address.Country())
            .RuleFor(ho => ho.Contact, f => f.Name.FullName())
            .RuleFor(ho => ho.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(ho => ho.Email, f => f.Internet.Email())
            .RuleFor(ho => ho.Website, f => f.Internet.Url())
            .RuleFor(ho => ho.ImageUrl, f => f.Image.PicsumUrl())
            .RuleFor(ho => ho.ImageUrl2, f => f.Image.PicsumUrl())
            .RuleFor(ho => ho.ImageUrl3, f => f.Image.PicsumUrl())
            .Generate(10);

        _context.HousingOffers
            .AddRange(housingOffersGenerator);
        _context.SaveChanges();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HousingOffer>>> Get()
    {
        if (!_context.HousingOffers.Any())
        {
            return NotFound();
        }

        var housingOffers = await _context.HousingOffers.ToListAsync();
        return Ok(housingOffers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HousingOffer>> GetHousingOffer(int id)
    {
        var housingOffer = await _context.HousingOffers.FindAsync(id);

        if (housingOffer == null)
        {
            return NotFound();
        }

        return Ok(housingOffer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutHousingOffer(int id, HousingOffer housingOffer)
    {
        try
        {
            var housingOfferToUpdate = await _context.HousingOffers.FindAsync(id);
            if (housingOfferToUpdate != null)
            {
                housingOfferToUpdate.Title = housingOffer.Title;
                housingOfferToUpdate.Description = housingOffer.Description;
                housingOfferToUpdate.Address = housingOffer.Address;
                housingOfferToUpdate.City = housingOffer.City;
                housingOfferToUpdate.ZipCode = housingOffer.ZipCode;
                housingOfferToUpdate.Country = housingOffer.Country;
                housingOfferToUpdate.Contact = housingOffer.Contact;
                housingOfferToUpdate.Phone = housingOffer.Phone;
                housingOfferToUpdate.Email = housingOffer.Email;
                housingOfferToUpdate.Website = housingOffer.Website;
                housingOfferToUpdate.ImageUrl = housingOffer.ImageUrl;
                housingOfferToUpdate.ImageUrl2 = housingOffer.ImageUrl2;
                housingOfferToUpdate.ImageUrl3 = housingOffer.ImageUrl3;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating housing offer");
            return BadRequest();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<HousingOffer>> DeleteHousingOffer(int id)
    {
        var housingOffer = await _context.HousingOffers.FindAsync(id);
        if (housingOffer == null)
        {
            return NotFound();
        }

        _context.HousingOffers.Remove(housingOffer);
        await _context.SaveChangesAsync();

        return Ok(housingOffer);
    }

    [HttpPost]
    public async Task<ActionResult<HousingOffer>> PostHousingOffer(HousingOffer housingOffer)
    {
        try
        {
            _context.HousingOffers.Add(housingOffer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHousingOffer", new {id = housingOffer.Id}, housingOffer);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating housing offer");
            return BadRequest();
        }
    }

    private bool HousingOfferExists(int id)
    {
        return _context.HousingOffers.Any(e => e.Id == id);
    }
}