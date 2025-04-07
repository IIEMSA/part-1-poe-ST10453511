using PoeApp.Entities;
using PoeApp.Models;

namespace PoeApp.Factory;

public class VenueFactory
{
	public static Venue CreateDatabaseVenue(VenueViewModel viewModel)
	{
		return new Venue
		{
			VenueName = viewModel.VenueName,
			Location = viewModel.Location,
			Capacity = viewModel.Capacity,
			ImageUrl = viewModel.ImageUrl,
			VenueId = viewModel.VenueId,
			Bookings = new List<Booking>(),
			Events = new List<Event>()
		};
	}

	public static VenueViewModel CreateViewModelVenue(Venue entity)
	{
		if (entity == null)
			return null;
		return new VenueViewModel
		{
			VenueName = entity.VenueName,
			Location  = entity.Location,
			Capacity  = entity.Capacity,
			ImageUrl  = entity.ImageUrl,
			VenueId   = entity.VenueId
		};
	}
}