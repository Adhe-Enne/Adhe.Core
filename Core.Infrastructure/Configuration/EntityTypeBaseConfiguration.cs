using Core.Contracts.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public abstract class EntityTypeBaseConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            // Propiedades de auditoría (si existen)
            builder.Property(u => u.DateAdded).IsRequired();
            builder.Property(u => u.DateUpdated);
            builder.Property(u => u.UserAdded).HasMaxLength(100);
            builder.Property(u => u.UserUpdated).HasMaxLength(100);

            builder.Property(u => u.DateAdded)
                 .HasColumnType("timestamp")
                       .HasDefaultValueSql("CURRENT_TIMESTAMP");
            builder.Property(u => u.DateUpdated)
                 .HasColumnType("timestamp")
                       .HasDefaultValueSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

            builder.Property(u => u.UserAdded)
                .HasColumnType("varchar(100)");
            builder.Property(u => u.UserUpdated)
                .HasColumnType("varchar(100)");

            ConfigurateProperties(builder);
            ConfigurateConstraints(builder);
            ConfigurateTableName(builder);
        }

        protected abstract void ConfigurateProperties(EntityTypeBuilder<T> builder);
        protected abstract void ConfigurateConstraints(EntityTypeBuilder<T> builder);
        protected abstract void ConfigurateTableName(EntityTypeBuilder<T> builder);
    }
}
