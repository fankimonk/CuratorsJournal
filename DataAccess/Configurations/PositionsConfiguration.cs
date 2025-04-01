using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class PositionsConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name).HasColumnType("nvarchar(max)");

            //builder
            //    .HasData([
            //        new Position { Id = 1, Name = "Декан", IsDefaultPosition = true },
            //        new Position { Id = 2, Name = "Зам. декана", IsDefaultPosition = true },
            //        new Position { Id = 3, Name = "Зав. кафедры", IsDefaultPosition = true },
            //        new Position { Id = 4, Name = "Преподаватель", IsDefaultPosition = true },
            //    ]);
        }
    }
}
