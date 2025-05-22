using Microsoft.AspNet.Identity.EntityFramework;

namespace HOA.Models;

public class EventParticipant
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string UserId { get; set; }
    public Event Event { get; set; }
}
