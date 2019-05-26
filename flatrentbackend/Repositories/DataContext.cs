using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlatRent.Entities;
using Microsoft.EntityFrameworkCore;
using File = System.IO.File;

namespace FlatRent.Repositories
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Flat> Flats { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Agreement> Agreements { get; set; }

        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Avatar> Avatars { get; set; }

        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<AgreementStatus> AgreementStatuses { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        private const string ArraySeparator = "///";

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flat>().Property(f => f.Features).HasConversion(arr => string.Join(ArraySeparator, arr),
                joined => joined.Split(ArraySeparator, StringSplitOptions.RemoveEmptyEntries));

            modelBuilder.Entity<Flat>().HasOne(e => e.Author).WithMany(e => e.Flats).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Flat>().HasOne(e => e.Address).WithOne(e => e.Flat).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Image>().HasOne(e => e.Flat).WithMany(e => e.Images).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Agreement>().HasOne(e => e.Flat).WithMany(e => e.Agreements).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Agreement>().HasOne(e => e.Author).WithMany(e => e.TenantAgreements).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Agreement>().HasOne(e => e.Conversation).WithOne(c => c.Agreement).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Incident>().HasOne(e => e.Author).WithMany(e => e.RegisteredIncidents).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Incident>().HasOne(e => e.Conversation).WithOne(c => c.Incident).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Conversation>().HasOne(e => e.Author).WithMany(e => e.StartedConversations).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Conversation>().HasOne(e => e.Recipient).WithMany(e => e.RecipientConversations).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Attachment>().HasOne(e => e.Message).WithMany(e => e.Attachments).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>().HasOne(e => e.Avatar).WithMany().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>().Property(e => e.AvatarId).HasDefaultValue(Guid.Parse("00000000-0000-0000-0000-000000000001"));

            modelBuilder.Entity<UserType>().HasData(UserType.ExistingTypes);
            modelBuilder.Entity<AgreementStatus>().HasData(AgreementStatus.ExistingAgreementStatuses);

//            modelBuilder.Entity<User>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
//            modelBuilder.Entity<FlatDetailsAddress>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
//            modelBuilder.Entity<Incident>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
//            modelBuilder.Entity<Flat>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
//            modelBuilder.Entity<Invoice>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
//            modelBuilder.Entity<Image>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
//            modelBuilder.Entity<Agreement>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = new Guid("b2c9ecb2-eda6-4b0f-9236-ef0583f11bb4"),
                FirstName = "Test",
                LastName = "Test",
                Email = "admin@admin.com",
                Password = "UhYWUG3vDiTZZt04YTqkBxL/RUxhyEvqpzCXJlRDMas=",
                TypeId = UserType.Administrator.Id,
                PhoneNumber = "+37060286000",
            },
            new User
            {
                Id = new Guid("b2c9ecb2-eda6-4b0f-9236-ef0583f11bc8"),
                FirstName = "Test",
                LastName = "Test",
                Email = "client@client.com",
                Password = "UhYWUG3vDiTZZt04YTqkBxL/RUxhyEvqpzCXJlRDMas=",
                TypeId = UserType.User.Id,
                PhoneNumber = "+37060286001",
            });

            modelBuilder.Entity<Avatar>().HasData(new Avatar()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "avatar.png",
                Bytes = File.ReadAllBytes("Files/defaultuser.png"),
                MimeType = "image/png",
            });



            modelBuilder.Entity<Flat>(builder =>
            {
//                builder.Property(f => f.Id).Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;
            });

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            UpdateChangeTrackerEntityDates();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken token = default(CancellationToken))
        {
            UpdateChangeTrackerEntityDates();
            return base.SaveChangesAsync(token);
        }

        private void UpdateChangeTrackerEntityDates()
        {
            var added = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
            added.ForEach(e => e.Property("CreatedDate").CurrentValue = DateTime.UtcNow);

            var changed = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
            changed.ForEach(e => e.Property("ModifiedDate").CurrentValue = DateTime.UtcNow);
        }
    }
}