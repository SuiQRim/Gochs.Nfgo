using Gochs.Nfgo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gochs.Nfgo.Data.Configurations;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.ToTable("Equipment", table =>
        {
            table.HasCheckConstraint(
                "CK_Equipment_Quantity_Positive",
                "\"Quantity\" > 0");
        });

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.InventoryNumber)
            .HasMaxLength(100);

        builder.HasIndex(x => x.InventoryNumber)
            .IsUnique();

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.Condition)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(x => x.Unit)
            .WithMany(x => x.Equipment)
            .HasForeignKey(x => x.UnitId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
