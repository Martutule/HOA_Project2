using System.ComponentModel.DataAnnotations;


namespace HOA.Models
{
    public class Incident
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [Required]
        [MinLength(100, ErrorMessage = "The description must be at least 100 characters long.")]
        public string Description { get; set; }
        public DateOnly Date { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public string? ImagePath { get; set; }
    }
}
