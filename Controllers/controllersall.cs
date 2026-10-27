using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.DTOs;
using MyApi.Models;

namespace MyApi.Controllers;

[ApiController]
[Route("api/donations")]
public class OperationsAllDonations : ControllerBase
{
    private readonly AppDbContext _context;

    public OperationsAllDonations(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
public async Task<IActionResult> AllDonations(
    [FromQuery] int pageSize = 20,
    [FromQuery] DateTime? cursorCreatedAt = null,
    [FromQuery] int? cursorId = null,
    CancellationToken cancellationToken = default)
{
    if (pageSize < 1 || pageSize > 100)
        return BadRequest("PageSize must be between 1 and 100.");

    if (cursorCreatedAt.HasValue != cursorId.HasValue)
        return BadRequest("Both cursorCreatedAt and cursorId are required.");

    if (cursorId.HasValue && cursorId.Value <= 0)
        return BadRequest("Invalid cursorId.");

    var query = _context.Donations.AsNoTracking();

    if (cursorCreatedAt.HasValue && cursorId.HasValue)
    {
        var lastDate = cursorCreatedAt.Value;
        var lastId = cursorId.Value;

        query = query.Where(d =>
            d.CreatedAt < lastDate ||
            (d.CreatedAt == lastDate && d.Id < lastId));
    }

    var rows = await query
        .OrderByDescending(d => d.CreatedAt)
        .ThenByDescending(d => d.Id)
        .Select(d => new
        {
            d.Id,
            d.Amount,
            DonorName = d.IsAnonymous
                ? "Anonymous"
                : d.DonorName,
            d.IsAnonymous,
            d.CreatedAt
        })
        .Take(pageSize + 1)
        .ToListAsync(cancellationToken);

    bool hasMore = rows.Count > pageSize;

    var donations = rows.Take(pageSize).ToList();
    var last = donations.LastOrDefault();

    return Ok(new
    {
        Data = donations,
        HasMore = hasMore,
        NextCursor = hasMore
            ? new
            {
                CreatedAt = last!.CreatedAt,
                Id = last.Id
            }
            : null
    });
}

   
   [HttpGet]
   [Route("Id")]
    public async Task <ActionResult> gitId( int Id )
    {
        var Dontion =  _context .Donations.FindAsync(Id) ;
       return Ok(Dontion);

    }
      
      [HttpPost]
      public async Task <ActionResult> donationspost( [FromBody ] CreateDonationDto dto )
    {
        
        var donation = new Donation
        {
            Amount = dto.Amount,
            DonorName = dto.DonorName,
            IsAnonymous = dto.IsAnonymous,
            CreatedAt = DateTime.UtcNow
        };

            _context.Donations.Add(donation);

           await _context.SaveChangesAsync();
             
             if (donation.IsAnonymous == true)
            {
               donation.DonorName ="Anonymous";

                return Ok(donation);
            }
            
          return Ok(donation);
             
             
       

    } 

   [HttpPut] 
   [Route("Id")]
   public async Task <ActionResult> UpdateID (int Id,CreateDonationDto donation)
    {
         
           if (donation.Amount <= 0)
    {
        return BadRequest("Donation amount must be greater than zero.");
    }

    
    var don = await _context.Donations.FindAsync(Id);

    if (don == null)
    {
        return NotFound("Donation not found.");
    }

   
    don.Amount = donation.Amount;
    don.DonorName = donation.DonorName;
    don.IsAnonymous = donation.IsAnonymous;

   
    await _context.SaveChangesAsync();

          return Ok();
             



    }

 [HttpDelete]
 [Route("Id")]
     public async Task<ActionResult> deleteid (int Id)
    {

        var donation = await _context.Donations.FindAsync(Id);
          if (donation == null)
          {
           return NotFound("Donation not found.");
          }
          
         _context.Donations.Remove(donation);
         
          await _context.SaveChangesAsync();
          return Ok();

   
   }





}


