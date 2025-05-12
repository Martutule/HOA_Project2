using HOA.Models;

namespace HOA.Services.Interfaces
{
    public interface IEventsService
    {
        Event GetEventById(int id);
        void AddEvent(Event @event);
        void UpdateEvent(Event @event);
        void DeleteEvent(int id);
        IEnumerable<Event> GetAllEvents();
        IEnumerable<Event> SearchEventsByEventName(string name);
    }
}
