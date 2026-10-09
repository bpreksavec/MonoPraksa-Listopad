using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace WebApplication3.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Athlete> Athletes { get; set; }

    public virtual DbSet<Workout> Workouts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Athlete>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("athletes_pkey");

            entity.ToTable("athletes");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");

            entity.Property(e => e.Age)
                .HasColumnName("age");

            entity.Property(e => e.Country)
                .HasMaxLength(30)
                .HasColumnName("country");

            entity.Property(e => e.Height)
                .HasColumnName("height");

            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");

            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .HasColumnName("surname");

            entity.Property(e => e.Weight)
                .HasColumnName("weight");
        });

        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("Workouts_pkey");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");

            entity.Property(e => e.AthleteId)
                .HasColumnName("athlete_id");

            entity.Property(e => e.AvgHeartrate)
                .HasColumnName("avg_heartrate");

            entity.Property(e => e.Distance)
                .HasColumnName("distance");

            entity.Property(e => e.Duration)
                .HasColumnName("duration");

            entity.Property(e => e.Sport)
                .HasMaxLength(50)
                .HasColumnName("sport");

            entity.HasOne(d => d.Athlete)
                .WithMany(p => p.Workouts)
                .HasForeignKey(d => d.AthleteId)
                .HasConstraintName("fk_workout_athlete");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}