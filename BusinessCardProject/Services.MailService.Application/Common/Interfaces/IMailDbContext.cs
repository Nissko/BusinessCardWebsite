using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Services.MailService.Domain.Entities;

namespace Services.MailService.Application.Common.Interfaces
{
    public interface IMailDbContext
    {
        DatabaseFacade Database { get; }

        public DbSet<EmailContentTemplateEntity> EmailContentTemplate { get; set; }
    
        void Migrate();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}