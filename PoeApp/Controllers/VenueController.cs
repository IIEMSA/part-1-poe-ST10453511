using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoeApp.DbContext;
using PoeApp.Entities;
using PoeApp.Factory;
using PoeApp.Models;

namespace PoeApp.Controllers
{
	public class VenueController : Controller
	{
		private readonly ApplicationDbContext _context;

		public VenueController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var venue = await _context.Venue.ToListAsync();

			List<VenueViewModel> venueList = new List<VenueViewModel>();
			foreach (Venue dbVenue in venue)
			{
				venueList.Add(VenueFactory.CreateViewModelVenue(dbVenue));
			}

			return View(venueList);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(VenueViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				Venue venue = VenueFactory.CreateDatabaseVenue(viewModel);
				_context.Add(venue);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return View(viewModel);
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
				return NotFound();

			var venue = await _context.Venue
			                          .FirstOrDefaultAsync(m => m.VenueId == id);

			if (venue == null)
				return NotFound();

			VenueViewModel viewModel = VenueFactory.CreateViewModelVenue(venue);
			return View(viewModel);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
				return NotFound();

			var venue = await _context.Venue.FindAsync(id);
			if (venue == null)
				return NotFound();

			VenueViewModel viewModel = VenueFactory.CreateViewModelVenue(venue);
			return View(viewModel);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(VenueViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				Venue venue = VenueFactory.CreateDatabaseVenue(viewModel);
				try
				{
					_context.Update(venue);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!_context.Venue.Any(e => e.VenueId == venue.VenueId))
						return NotFound();
					else
						throw;
				}

				return RedirectToAction(nameof(Index));
			}

			return View(viewModel);
		}


		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var venue = await _context.Venue.FirstOrDefaultAsync(m => m.VenueId == id);

			if (venue == null)
			{
				return NotFound();
			}

            VenueViewModel viewModel = VenueFactory.CreateViewModelVenue(venue);
            return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			var venue = await _context.Venue.FindAsync(id);
			if (venue == null)
			{
				return NotFound();
			}

			_context.Venue.Remove(venue);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
	}
}