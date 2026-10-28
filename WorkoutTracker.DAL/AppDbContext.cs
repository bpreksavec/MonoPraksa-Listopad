using Microsoft.EntityFrameworkCore;
using WorkoutTracker.DAL.Models;

namespace WorkoutTracker.DAL;

// Table and column names come from UseSnakeCaseNamingConvention() in Program.cs
public partial class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<BodyMeasurement> BodyMeasurements { get; set; }

    public virtual DbSet<Sport> Sports { get; set; }

    public virtual DbSet<SportSegment> SportSegments { get; set; }

    public virtual DbSet<Workout> Workouts { get; set; }

    public virtual DbSet<WorkoutSegment> WorkoutSegments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.FirstName)
                .HasMaxLength(50);

            entity.Property(e => e.LastName)
                .HasMaxLength(50);

            entity.Property(e => e.Email)
                .HasMaxLength(256);

            entity.Property(e => e.Gender)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.Property(e => e.Bio)
                .HasMaxLength(1000);

            entity.Property(e => e.ProfileImageUrl)
                .HasMaxLength(500);

            entity.HasOne(e => e.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<UserProfile>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Lookup table with fixed ids, so seeded rows don't clash with an identity sequence
            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.Property(e => e.Name)
                .HasMaxLength(20);

            entity.HasIndex(e => e.Name).IsUnique();

            entity.HasData(
                new Role { Id = 1, Name = Role.Admin },
                new Role { Id = 2, Name = Role.User });
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.HasOne(e => e.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<BodyMeasurement>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(e => e.User)
                .WithMany(u => u.BodyMeasurements)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Sport>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.Property(e => e.Name)
                .HasMaxLength(50);

            entity.HasIndex(e => e.Name).IsUnique();

            entity.HasData(
                new Sport { Id = 1, Name = "Running" },
                new Sport { Id = 2, Name = "Cycling" },
                new Sport { Id = 3, Name = "Swimming" },
                new Sport { Id = 4, Name = "Walking" },
                new Sport { Id = 5, Name = "Hiking" },
                new Sport { Id = 6, Name = "Rowing" },
                new Sport { Id = 7, Name = "Strength" },
                new Sport { Id = 8, Name = "Other" },
                new Sport { Id = 9, Name = "Triathlon", IsComposite = true },
                new Sport { Id = 10, Name = "Duathlon", IsComposite = true });
        });

        modelBuilder.Entity<SportSegment>(entity =>
        {
            entity.HasKey(e => new { e.CompositeSportId, e.SegmentOrder });

            entity.HasOne(e => e.CompositeSport)
                .WithMany(s => s.Segments)
                .HasForeignKey(e => e.CompositeSportId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Discipline)
                .WithMany()
                .HasForeignKey(e => e.DisciplineId)
                .OnDelete(DeleteBehavior.Restrict);

            // Standard (olympic) distances
            entity.HasData(
                new SportSegment { CompositeSportId = 9, SegmentOrder = 1, DisciplineId = 3, DistanceKm = 1.5 },
                new SportSegment { CompositeSportId = 9, SegmentOrder = 2, DisciplineId = 2, DistanceKm = 40 },
                new SportSegment { CompositeSportId = 9, SegmentOrder = 3, DisciplineId = 1, DistanceKm = 10 },
                new SportSegment { CompositeSportId = 10, SegmentOrder = 1, DisciplineId = 1, DistanceKm = 10 },
                new SportSegment { CompositeSportId = 10, SegmentOrder = 2, DisciplineId = 2, DistanceKm = 40 },
                new SportSegment { CompositeSportId = 10, SegmentOrder = 3, DisciplineId = 1, DistanceKm = 5 });
        });

        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.Notes)
                .HasMaxLength(1000);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Workouts)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Sport)
                .WithMany()
                .HasForeignKey(e => e.SportId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<WorkoutSegment>(entity =>
        {
            entity.HasKey(e => new { e.WorkoutId, e.SegmentOrder });

            entity.HasOne(e => e.Workout)
                .WithMany(w => w.Segments)
                .HasForeignKey(e => e.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Discipline)
                .WithMany()
                .HasForeignKey(e => e.DisciplineId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
