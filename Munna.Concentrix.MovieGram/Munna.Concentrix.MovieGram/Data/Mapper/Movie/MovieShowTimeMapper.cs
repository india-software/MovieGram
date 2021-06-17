using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Munna.Concentrix.MovieGram.Core;
namespace Munna.Concentrix.MovieGram.Data.Mapper
{
    /// <summary>
    /// Represents a movie mapping configuration
    /// </summary>
    public partial class MovieShowTimeMapper : MovieGramEntityTypeConfiguration<MovieShowTime>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<MovieShowTime> builder)
        {           
            builder.ToTable(nameof(MovieShowTime));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.StartShowTimeUTC).IsRequired();
            builder.Property(x => x.EndShowTimeUTC).IsRequired();
            builder.HasOne(x => x.Movie)
                .WithMany(y => y.Showtimes).HasForeignKey(x => x.MovieId);
            base.Configure(builder);
        }

        #endregion
    }
}