using Gochs.Nfgo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gochs.Nfgo.Data.Configurations;

public class FormationConfiguration : IEntityTypeConfiguration<Formation>
{
    public void Configure(EntityTypeBuilder<Formation> builder)
    {
        builder.ToTable("Formations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Purpose)
            .HasMaxLength(500);

        builder.Property(x => x.Location)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.LeaderName)
            .HasMaxLength(200);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasMany(x => x.Units)
            .WithOne(x => x.Formation)
            .HasForeignKey(x => x.FormationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Notifications)
            .WithOne(x => x.Formation)
            .HasForeignKey(x => x.FormationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
