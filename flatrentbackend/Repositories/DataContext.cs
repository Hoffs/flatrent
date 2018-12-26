using System;
using System.Collections.Generic;
using System.Linq;
using FlatRent.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlatRent.Repositories
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ClientInformation> ClientInformations { get; set; }
        public DbSet<EmployeeInformation> EmployeeInformations { get; set; }
        public DbSet<Fault> Faults { get; set; }
        public DbSet<Flat> Flats { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<RentAgreement> RentAgreements { get; set; }
        public DbSet<UserType> UserTypes { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var adminUserType = new UserType
            {
                Id = new Guid("ee3d96b6-4243-4235-8231-9a9fced615fe"),
                Name = "Administrator",
                Users = new List<User>()
            };
            modelBuilder.Entity<UserType>().HasData(new UserType
                {
                    Id = new Guid("ed42ea4b-9900-4477-af32-0336ca61eab1"),
                    Name = "Client",
                    Users = new List<User>()
                },
                new UserType
                {
                    Id = new Guid("268c6597-15cb-4ab1-9d39-8a7d7c85b3d1"),
                    Name = "Employee",
                    Users = new List<User>()
                },
                adminUserType);

            modelBuilder.Entity<User>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Address>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<ClientInformation>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<EmployeeInformation>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Fault>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Flat>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Invoice>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Owner>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<Photo>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<RentAgreement>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<UserType>().Property(x => x.Id).HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = new Guid("b2c9ecb2-eda6-4b0f-9236-ef0583f11bb4"),
                FirstName = "Test",
                LastName = "Test",
                Email = "admin@admin.com",
                Password = "UhYWUG3vDiTZZt04YTqkBxL/RUxhyEvqpzCXJlRDMas=",
                TypeId = adminUserType.Id,
                PhoneNumber = "+37060286000"
            });
            
//            modelBuilder.Entity<Flat>().HasOne(x => x.Address).WithOne(x => x.Flat);
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var added = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
            added.ForEach(e => e.Property("CreatedDate").CurrentValue = DateTime.UtcNow);

            var changed = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
            changed.ForEach(e => e.Property("ModifiedDate").CurrentValue = DateTime.UtcNow);
            
            return base.SaveChanges();
        }
    }
}