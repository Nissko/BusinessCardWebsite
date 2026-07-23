using Microsoft.EntityFrameworkCore;
using Services.MailService.Application.Common.Interfaces;
using Services.MailService.Domain.Entities;
using Services.MailService.Infrastructure.Configurations;

namespace Services.MailService.Infrastructure
{
    public class MailDbContext : DbContext, IMailDbContext
    {
        private readonly string _defaultSchema = "bytecode_email_notifications";

        public MailDbContext(DbContextOptions<MailDbContext> options)
            : base(options)
        {
        }
    
        public DbSet<EmailContentTemplateEntity> EmailContentTemplate { get; set; }
    
        public void Migrate()
        {
            Database.Migrate();
        }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_defaultSchema);
        
            modelBuilder.ApplyConfiguration(new EmailContentTemplateConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MailDbContext).Assembly);
        }
    }
}