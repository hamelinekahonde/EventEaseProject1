using EventEaseProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventEaseProject.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string searchType, int? venueId, DateTime? startDate, DateTime? endDate)
        {
            var events = _context.Event
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchType))
                events = events.Where(e => e.EventType != null && e.EventType.Name == searchType);

            if (venueId.HasValue)
                events = events.Where(e => e.VenueId == venueId);

            if (startDate.HasValue && endDate.HasValue)
                events = events.Where(e => e.EventDate >= startDate && e.EventDate <= endDate);


            ViewData["EventTypes"] = _context.EventType.ToList();
            ViewData["Venues"] = _context.Venue.ToList();

            return View(await events.ToListAsync());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Event created successfully.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["Venues"] = _context.Venue.ToList();

            ViewData["EventTypes"] = _context.EventType.ToList();

            return View(@event);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
                return NotFound();

            ViewData["Venues"] = _context.Venue.ToList();

            ViewData["EventTypes"] = _context.EventType.ToList();

            return View(@event);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event @event)
        {
            if (id != @event.EventId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Event updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Event.Any(e => e.EventId == id))
                        return NotFound();
                    else
                        throw;
                }
            }
            // Repopulate dropdown if model state is invalid
            ViewData["EventTypes"] = _context.EventType.ToList();
            return View(@event);
        }

        // STEP 1: Confirm Deletion (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Event
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (@event == null) return NotFound();

            return View(@event);
        }

        // STEP 2: Perform Deletion (POST) - Logic to restrict the deletion of events associated with active bookings.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Event.FindAsync(id);
            if (@event == null) return NotFound();

            var isBooked = await _context.Booking.AnyAsync(b => b.EventId == id);
            if (isBooked)
            {
                TempData["ErrorMessage"] = "Cannot delete event because it has existing bookings.";
                return RedirectToAction(nameof(Index));
            }

            _context.Event.Remove(@event);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Event deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .Include(e => e.Venue) // Include related venue if applicable
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }
    }
}