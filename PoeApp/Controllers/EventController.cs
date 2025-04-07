using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PoeApp.DbContext;
using PoeApp.Entities;
using PoeApp.Factory;
using PoeApp.Models;

namespace PoeApp.Controllers
{
	public class EventController : Controller
	{
		private readonly ApplicationDbContext _context;

		public EventController(ApplicationDbContext context)
		{
			_context = context;
		}

        public async Task<IActionResult> Index()
        {
            var eventsEntityList = await _context.Event.ToListAsync();
            List<EventViewModel> viewModelList = new List<EventViewModel>();
            foreach (Event dbEvent in eventsEntityList)
            {
                viewModelList.Add(EventFactory.CreateViewModelEvent(dbEvent));
            }

            return View(viewModelList);
        }

        private async Task<List<VenueViewModel>> GetAllVenues()
        {
            List<Venue> venueEntityList = await _context.Venue.ToListAsync();
            List<VenueViewModel> viewModelList = new List<VenueViewModel>();
            foreach (Venue dbVenue in venueEntityList)
            {
                viewModelList.Add(VenueFactory.CreateViewModelVenue(dbVenue));
            }

            return viewModelList;
        }

        public async Task<IActionResult> Create()
        {
            List<VenueViewModel> venueViewModelList = await GetAllVenues();
            ViewBag.VenueList = new SelectList(venueViewModelList, nameof(VenueViewModel.VenueId),
                nameof(VenueViewModel.VenueName));

            return View();
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(EventViewModel eventModel)
		{
			if (ModelState.IsValid)
			{
                Event eventEntity = EventFactory.CreateDatabaseEvent(eventModel);
                _context.Add(eventEntity);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			List<VenueViewModel> venueViewModelList = await GetAllVenues();
            ViewBag.VenueList = new SelectList(venueViewModelList, nameof(VenueViewModel.VenueId),
                nameof(VenueViewModel.VenueName));

            return View(eventModel);
		}

		public async Task<IActionResult> Details(int id)
		{
			var eventEntity = await _context.Event
			                               .Include(e => e.Venue)
			                               .FirstOrDefaultAsync(e => e.EventId == id);
			if (eventEntity == null)
			{
				return NotFound();
			}

            var eventModel = EventFactory.CreateViewModelEvent(eventEntity);
            return View(eventModel);
		}

		public async Task<IActionResult> Delete(int id)
		{
			var eventEntity = await _context.Event
			                               .Include(e => e.Venue)
			                               .FirstOrDefaultAsync(e => e.EventId == id);
			if (eventEntity == null)
			{
				return NotFound();
			}

            var eventModel = EventFactory.CreateViewModelEvent(eventEntity);
            return View(eventModel);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var eventModel = await _context.Event.FindAsync(id);
			_context.Event.Remove(eventModel);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool EventExists(int id)
		{
			return _context.Event.Any(e => e.EventId == id);
		}

		public async Task<IActionResult> Edit(int id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var eventEntity = await _context.Event.FindAsync(id);
			if (eventEntity == null)
			{
				return NotFound();
			}

            var eventModel = EventFactory.CreateViewModelEvent(eventEntity);
            List<VenueViewModel> venueViewModelList = await GetAllVenues();
            ViewBag.VenueList = new SelectList(venueViewModelList, nameof(VenueViewModel.VenueId),
                nameof(VenueViewModel.VenueName));	

            return View(eventModel);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventViewModel eventModel)
        {
            if (id != eventModel.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Event eventEntity = EventFactory.CreateDatabaseEvent(eventModel);
                    _context.Update(eventEntity);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index"); // Explicit action name
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(eventModel.EventId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Reload venues if validation fails
            List<VenueViewModel> venueViewModelList = await GetAllVenues();
            ViewBag.VenueList = new SelectList(venueViewModelList,
                nameof(VenueViewModel.VenueId),
                nameof(VenueViewModel.VenueName),
                eventModel.Venue?.VenueId);

            return View(eventModel);
        }
    }
}