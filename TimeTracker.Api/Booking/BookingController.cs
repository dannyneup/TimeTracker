using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Api.Context;
using TimeTracker.Api.Booking.ViewModels;

namespace TimeTracker.Api.Booking;

[ApiController]
[Route("/bookings")]
public class BookingController : ControllerBase
{
    private readonly TimeTrackerContext _context;
    private readonly IMapper _mapper;

    public BookingController(TimeTrackerContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    [HttpPost]
    public async Task<ActionResult> Create(BookingWriteViewModel bookingWriteViewModel)
    {
        var booking = _mapper.Map<Booking>(bookingWriteViewModel);
        
        var bookingEntry = await _context.AddAsync(booking);
        await _context.SaveChangesAsync();

        var bookingReadViewModel = _mapper.Map<BookingReadViewModel>(bookingEntry.Entity);
    
        return CreatedAtAction("GetById", new { id = bookingReadViewModel.Id}, bookingReadViewModel);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var bookings = await _context.Bookings
            .ToListAsync();

        var bookingReadViewModels = _mapper.Map<List<BookingReadViewModel>>(bookings);
    
        return Ok(bookingReadViewModels);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);

        var bookingReadViewModel = _mapper.Map<BookingReadViewModel>(booking);
        
        return bookingReadViewModel != null
            ? Ok(bookingReadViewModel)
            : NotFound();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Edit(int id, BookingWriteViewModel inputBookingWriteViewModel)
    {
        if (!await EntityExists(id)) return NotFound();

        var booking = _mapper.Map<Booking>(inputBookingWriteViewModel);
        booking.Id = id;
        
        _context.Entry(booking).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);

        if (booking is null) return NotFound();
        _context.Bookings.Remove(booking);

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    private Task<bool> EntityExists(int id)
    {
        return _context.Bookings.AnyAsync(e => e.Id == id);
    }
}