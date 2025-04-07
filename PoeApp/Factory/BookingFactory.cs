using PoeApp.Entities;
using PoeApp.Models;

namespace PoeApp.Factory;

public class BookingFactory
{
	public static Booking CreateDatabaseBooking(BookingViewModel viewModel)
	{
		return new Booking
		{
			BookingId = viewModel.BookingId,
			EventId = viewModel.Event.EventId,
			VenueId = viewModel.Venue.VenueId,
			BookingDate = viewModel.BookingDate
		};
	}

	public static BookingViewModel CreateViewModelBooking(Booking entity)
	{
		return new BookingViewModel
		{
			BookingId = entity.BookingId,
			BookingDate = entity.BookingDate,
			Venue = VenueFactory.CreateViewModelVenue(entity.Venue),
			Event = EventFactory.CreateViewModelEvent(entity.Event)
		};
	}
}