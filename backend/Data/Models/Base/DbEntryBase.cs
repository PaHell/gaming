using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Data.Models.Base
{
      public abstract class DbEntryBase<Model, Key> : IEntityTypeConfiguration<Model>
            where Model : DbEntryBase<Model, Key>
            where Key : struct
      {
            public Key Id { get; set; }

            public virtual void Configure(EntityTypeBuilder<Model> builder)
            {
                  builder.HasKey(e => e.Id);
            }
      }
}