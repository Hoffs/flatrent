using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FlatRent.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlatRent.Repositories
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Fault> Faults { get; set; }
        public DbSet<Flat> Flats { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Agreement> RentAgreements { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<AgreementStatus> AgreementStatuses { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flat>().HasOne(f => f.Owner).WithMany(o => o.Flats).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Flat>().HasOne(f => f.Address).WithOne(a => a.Flat).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserType>().HasData(UserType.ExistingTypes);
            modelBuilder.Entity<AgreementStatus>().HasData(AgreementStatus.ExistingAgreementStatuses);

//            modelBuilder.Entity<User>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
//            modelBuilder.Entity<Address>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
//            modelBuilder.Entity<Fault>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
//            modelBuilder.Entity<Flat>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
//            modelBuilder.Entity<Invoice>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
//            modelBuilder.Entity<Photo>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
//            modelBuilder.Entity<Agreement>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = new Guid("b2c9ecb2-eda6-4b0f-9236-ef0583f11bb4"),
                FirstName = "Test",
                LastName = "Test",
                Email = "admin@admin.com",
                Password = "UhYWUG3vDiTZZt04YTqkBxL/RUxhyEvqpzCXJlRDMas=",
                TypeId = UserType.Types.Administrator,
                PhoneNumber = "+37060286000",
            },
            new User
            {
                Id = new Guid("b2c9ecb2-eda6-4b0f-9236-ef0583f11bc8"),
                FirstName = "Test",
                LastName = "Test",
                Email = "client@client.com",
                Password = "UhYWUG3vDiTZZt04YTqkBxL/RUxhyEvqpzCXJlRDMas=",
                TypeId = UserType.Types.User,
                PhoneNumber = "+37060286001",
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