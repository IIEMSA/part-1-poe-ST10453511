using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PoeApp.DbContext;
using PoeApp.Entities;
using PoeApp.Factory;
using PoeApp.Models;

namespace PoeApp.Controllers
{
	public class BookingController : Controller
	{
		private readonly ApplicationDbContext _context;

		public BookingController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var bookingEntityList = await _context.Booking
			                                      .Include(b => b.Event)
			                                      .Include(b => b.Venue)
			                                      .ToListAsync();

			List<BookingViewModel> viewModelList = new List<BookingViewModel>();
			foreach (Booking dbBooking in bookingEntityList)
			{
				viewModelList.Add(BookingFactory.CreateViewModelBooking(dbBooking));
			}

			return View(viewModelList);
		}

		private async Task<List<VenueViewModel>> GetAllVenues()
		{
			List<Venue>          venueEntityList = await _context.Venue.ToListAsync();
			List<VenueViewModel> viewModelList   = new List<VenueViewModel>();
			foreach (Venue dbVenue in venueEntityList)
			{
				viewModelList.Add(VenueFactory.CreateViewModelVenue(dbVenue));
			}

			return viewModelList;
		}


		private async Task<List<EventViewModel>> GetAllEvents()
		{
			List<Event>          venueEntityList = await _context.Event.ToListAsync();
			List<EventViewModel> viewModelList   = new List<EventViewModel>();
			foreach (Event dbVenue in venueEntityList)
			{
				viewModelList.Add(EventFactory.CreateViewModelEvent(dbVenue));
			}

			return viewModelList;
		}

		public async Task<IActionResult> Create()
		{
			List<VenueViewModel> venueViewModelList = await GetAllVenues();
			ViewBag.VenueList = new SelectList(venueViewModelList, nameof(VenueViewModel.VenueId),
				nameof(VenueViewModel.VenueName));

			List<EventViewModel> eventViewModelList = await GetAllEvents();
			ViewBag.EventList = new SelectList(eventViewModelList, nameof(EventViewModel.EventId),
				nameof(EventViewModel.EventName));
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(BookingViewModel booking)
		{
			//if (ModelState.IsValid)
			//{
				Booking bookingEntity = BookingFactory.CreateDatabaseBooking(booking);
				_context.Add(bookingEntity);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			//}

			return View(booking);
		}

		public async Task<IActionResult> Details(int id)
		{
			Booking? bookingEntity = await _context.Booking
			                                       .Include(b => b.Event)
			                                       .Include(b => b.Venue)
			                                       .FirstOrDefaultAsync(m => m.BookingId == id);
			if (bookingEntity == null)
			{
				return NotFound();
			}

			BookingViewModel bookingViewModel = BookingFactory.CreateViewModelBooking(bookingEntity);
			return View(bookingViewModel);
		}

		public async Task<IActionResult> Delete(int id)
		{
			var booking = await _context.Booking
										.Include(b => b.Event)
										.Include(b => b.Venue)
										.FirstOrDefaultAsync(m => m.BookingId == id);
			if (booking == null)
			{
				return NotFound();
			}
			BookingViewModel bookingViewModel = BookingFactory.CreateViewModelBooking(booking);
			return View(bookingViewModel);
		}

            [HttpPost]
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var booking = await _context.Booking.FindAsync(id);
			_context.Booking.Remove(booking);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool BookingExists(int id)
		{
			return _context.Booking.Any(e => e.BookingId == id);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			Booking? booking = await _context.Booking
				.Include(b=> b.Event).Include(b => b.Venue).FirstOrDefaultAsync(b=> b.BookingId == id);

			if (booking == null)
			{
				return NotFound();
			}
            List<VenueViewModel> venueViewModelList = await GetAllVenues();
            ViewBag.VenueList = new SelectList(venueViewModelList, nameof(VenueViewModel.VenueId),
                nameof(VenueViewModel.VenueName));

            List<EventViewModel> eventViewModelList = await GetAllEvents();
            ViewBag.EventList = new SelectList(eventViewModelList, nameof(EventViewModel.EventId),
                nameof(EventViewModel.EventName));

            BookingViewModel bookingViewModel = BookingFactory.CreateViewModelBooking(booking);
			return View(bookingViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(BookingViewModel bookingViewModel)
		{
			//if (ModelState.IsValid)
			//{
				try
				{
					Booking bookingEntity = BookingFactory.CreateDatabaseBooking(bookingViewModel);
					_context.Update(bookingEntity);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!BookingExists(bookingViewModel.BookingId))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}

				return RedirectToAction(nameof(Index));
			//}

			//return View(bookingViewModel);
		}
	}
}