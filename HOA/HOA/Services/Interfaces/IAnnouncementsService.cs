using HOA.Models;

namespace HOA.Services.Interfaces
{
    public interface IAnnouncementsService
    {
        Announcement GetAnnouncementsById(int id);
        void AddAnnouncement(Announcement announcement);
        void UpdateAnnouncement(Announcement announcement);
        void DeleteAnnouncement(int id);
        IEnumerable<Announcement> GetAllAnnouncements();
        IEnumerable<Announcement> SearchAnnouncementsByTitle(string title);
    }
}
