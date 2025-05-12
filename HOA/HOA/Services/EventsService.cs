using HOA.Models;
using HOA.Repositories.Interfaces;
using HOA.Services.Interfaces;

namespace HOA.Services
{
    public class EventsService: IEventsService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public EventsService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public Event GetEventById(int id)
        {
            return _repositoryWrapper.EventsRepository.FindByCondition(a => a.Id == id).FirstOrDefault();

        }
        public void AddEvent(Event @event)
        {
            _repositoryWrapper.EventsRepository.Create(@event);
            _repositoryWrapper.Save();
        }
        public void UpdateEvent(Event @event)
        {
            _repositoryWrapper.EventsRepository.Update(@event);
            _repositoryWrapper.Save();
        }
        public void DeleteEvent(int id)
        {
            var @event = _repositoryWrapper.EventsRepository.FindByCondition(p => p.Id == id).FirstOrDefault();

            if (@event != null)
            {
                _repositoryWrapper.EventsRepository.Delete(@event);
                _repositoryWrapper.Save();
            }
        }
        public IEnumerable<Event> GetAllEvents()
        {
            return _repositoryWrapper.EventsRepository.FindAll();
        }
        public IEnumerable<Event> SearchEventsByEventName(string name)
        {
            return _repositoryWrapper.EventsRepository.FindByCondition(p => p.EventName.ToLower().Contains(name.ToLower().Trim()));
        }
    }
}
