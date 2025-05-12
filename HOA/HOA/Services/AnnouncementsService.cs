using HOA.Models;
using HOA.Repositories.Interfaces;
using HOA.Services.Interfaces;

namespace HOA.Services
{
    public class AnnouncementsService: IAnnouncementsService
    {
        private IRepositoryWrapper _repositoryWrapper;

        public AnnouncementsService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        public Announcement GetAnnouncementsById(int id)
        {
            return _repositoryWrapper.AnnouncementsRepository.FindByCondition(a => a.Id == id).FirstOrDefault();

        }
        public void AddAnnouncement(Announcement announcement)
        {
            _repositoryWrapper.AnnouncementsRepository.Create(announcement);
            _repositoryWrapper.Save();
        }
        public void UpdateAnnouncement(Announcement announcement)
        {
            _repositoryWrapper.AnnouncementsRepository.Update(announcement);
            _repositoryWrapper.Save();
        }
        public void DeleteAnnouncement(int id)
        {
            var announcement = _repositoryWrapper.AnnouncementsRepository.FindByCondition(p => p.Id == id).FirstOrDefault();

            if (announcement != null)
            {
                _repositoryWrapper.AnnouncementsRepository.Delete(announcement);
                _repositoryWrapper.Save();
            }
        }
        public IEnumerable<Announcement> GetAllAnnouncements()
        {
            return _repositoryWrapper.AnnouncementsRepository.FindAll();
        }
        public IEnumerable<Announcement> SearchAnnouncementsByTitle(string name)
        {
            return _repositoryWrapper.AnnouncementsRepository.FindByCondition(p => p.Title.ToLower().Contains(name.ToLower().Trim()));
        }
    }
}
