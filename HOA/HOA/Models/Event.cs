namespace HOA.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public string Color { get; set; }
    }
}
