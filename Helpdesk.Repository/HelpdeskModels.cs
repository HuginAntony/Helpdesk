namespace Helpdesk.Repository
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class HelpdeskModels : DbContext
    {
        public HelpdeskModels()
            : base("name=HelpdeskModels")
        {
        }

        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<AppSpecialist> AppSpecialists { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Developer> Developers { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<RequestMessage> RequestMessages { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserAppSpecialist> UserAppSpecialists { get; set; }
        public virtual DbSet<UserBrand> UserBrands { get; set; }
        public virtual DbSet<UserDeveloper> UserDevelopers { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<RequestType> RequestTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Application>()
                .HasMany(e => e.Requests)
                .WithRequired(e => e.Application)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AppSpecialist>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<AppSpecialist>()
                .Property(e => e.Surname)
                .IsUnicode(false);

            modelBuilder.Entity<AppSpecialist>()
                .Property(e => e.EmailAddress)
                .IsUnicode(false);

            modelBuilder.Entity<AppSpecialist>()
                .HasMany(e => e.UserAppSpecialists)
                .WithRequired(e => e.AppSpecialist)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Brand>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Brand>()
                .HasMany(e => e.UserBrands)
                .WithRequired(e => e.Brand)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Brand>()
                .HasMany(e => e.Requests)
                .WithRequired(e => e.Brand)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Developer>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Developer>()
                .Property(e => e.Surname)
                .IsUnicode(false);

            modelBuilder.Entity<Developer>()
                .Property(e => e.EmailAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Developer>()
                .HasMany(e => e.UserDevelopers)
                .WithRequired(e => e.Developer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Request>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Request>()
                .Property(e => e.BookingReference)
                .IsUnicode(false);

            modelBuilder.Entity<Request>()
                .Property(e => e.Priority)
                .IsUnicode(false);

            modelBuilder.Entity<Request>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Request>()
                .Property(e => e.RequestType)
                .IsUnicode(false);

            modelBuilder.Entity<Request>()
                .HasMany(e => e.RequestMessages)
                .WithRequired(e => e.Request)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RequestMessage>()
                .Property(e => e.Message)
                .IsUnicode(false);

            modelBuilder.Entity<RequestMessage>()
                .Property(e => e.AttachmentFilename)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.UserRoles)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Surname)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.EmailAddress)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Requests)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.RequestMessages)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserBrands)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserAppSpecialists)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserDevelopers)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserRoles)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RequestType>()
                .Property(e => e.Name)
                .IsUnicode(false);
        }
    }
}
