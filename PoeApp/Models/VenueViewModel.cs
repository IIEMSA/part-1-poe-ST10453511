using System.ComponentModel.DataAnnotations;

namespace PoeApp.Models
{
    public class VenueViewModel
    {
        [Required]
        public int VenueId { get; set; }
        public string VenueName { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public string ImageUrl { get; set; }

    }
}