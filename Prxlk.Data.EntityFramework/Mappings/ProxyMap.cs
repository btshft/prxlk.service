using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Prxlk.Domain.Models;

namespace Prxlk.Data.EntityFramework.Mappings
{
    public class ProxyMap : IEntityTypeConfiguration<Proxy>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Proxy> builder)
        {
            builder.Property(p => p.Id).IsRequired()
                .ValueGeneratedOnAdd();

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Protocol)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}