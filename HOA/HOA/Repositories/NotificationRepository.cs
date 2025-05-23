using HOA.Models;
using HOA.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HOA.Repositories
{
    public class NotificationRepository : RepositoryBase<Notification> , INotificationRepository
    {
        public NotificationRepository(HOADbContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
