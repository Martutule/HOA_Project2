namespace HOA.Models
{
    public class Notification
    {
        public int Id { get; set; }              // Unique identifier for the notification
        public string Message { get; set; }        
        public string Link { get; set; }           
        public DateTime Timestamp { get; set; }    
    }
}
