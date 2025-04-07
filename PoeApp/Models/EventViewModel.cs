using System.ComponentModel.DataAnnotations;
using PoeApp.Models;

namespace PoeApp.Models
{
    public class EventViewModel
    {
        [Required]
        public int EventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }

        public VenueViewModel? Venue { get; set; }
    }
}