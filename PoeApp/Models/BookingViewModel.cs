using PoeApp.Entities;
using PoeApp.Models;

namespace PoeApp.Models
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }

        public EventViewModel Event { get; set; }

        public VenueViewModel Venue { get; set; }

        public DateTime BookingDate { get; set; }
    }
}