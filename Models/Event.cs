using System;
using System.ComponentModel.DataAnnotations;

namespace EventEaseProject.Models
{
    public class Event
    {
        public int EventId { get; set; }

        [Required]
        [StringLength(255)]
        public required string EventName { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public required DateTime EventDate { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public int? VenueId { get; set; }
        public Venue? Venue { get; set; }
    }
}
