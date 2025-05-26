using System.ComponentModel.DataAnnotations;
using EventEaseProject.Models;

namespace EventEaseProject.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        public required int EventId { get; set; }

        [Required]
        public required int VenueId { get; set; }

        [DataType(DataType.DateTime)]
        public required DateTime BookingDate { get; set; } = DateTime.Now;

        // Navigation properties
        public required Event Event { get; set; }
        public required Venue Venue { get; set; }
    }
}
