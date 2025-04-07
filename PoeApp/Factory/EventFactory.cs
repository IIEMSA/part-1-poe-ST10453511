using PoeApp.Entities;
using PoeApp.Models;

namespace PoeApp.Factory;

public class EventFactory
{
	public static Event CreateDatabaseEvent(EventViewModel viewModel)
	{
		return new Event
		{
			EventId = viewModel.EventId,
			EventDate = viewModel.EventDate,
			EventName = viewModel.EventName,
			Description = viewModel.Description,
			VenueId = viewModel.Venue?.VenueId ?? 0
		};
	}

	public static EventViewModel CreateViewModelEvent(Event entity)
	{
		return new EventViewModel
		{
			EventId = entity.EventId,
			EventDate = entity.EventDate,
			EventName = entity.EventName,
			Description = entity.Description,
			Venue = VenueFactory.CreateViewModelVenue(entity.Venue)
		};
	}
}