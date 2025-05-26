using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEaseProject.Models
{
    public class Venue
    {
        public int VenueId { get; set; }

        [Required]
        public required string VenueName { get; set; }

        [Required]
        public required string Location { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0.")]
        public required int Capacity { get; set; }

        //This says - to srore the URL of the image uploaded
        public string? ImageUrl { get; set; }

        //Add this new one - only for uploading from Create/Edit form
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

    }
}

