namespace HOA.Models
{
    public class Dashboard
    {
        public int TotalResidents { get; set; }
        public float TotalDuePayments { get; set; }
        public int TotalEvents { get; set; }
        public List<Announcement> RecentAnnouncements { get; set; }

        public Dashboard(int totalResidents, float totalDuePayments, int totalEvents, List<Announcement> recentAnnouncements)
        {
            TotalResidents = totalResidents;
            TotalDuePayments = totalDuePayments;
            TotalEvents = totalEvents;
            RecentAnnouncements = recentAnnouncements;
        }
    }
}
