using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Munna.Concentrix.MovieGram.Core;
namespace Munna.Concentrix.MovieGram.Data.Mapper
{
    /// <summary>
    /// Represents a movie mapping configuration
    /// </summary>
    public partial class MovieMapper : MovieGramEntityTypeConfiguration<Movie>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Movie> builder)
        {
           
            builder.ToTable(nameof(Movie));
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(1000).IsRequired();
            builder.Property(x => x.ImageUrl).HasMaxLength(300).IsRequired();
            builder.HasMany(x => x.Showtimes).WithOne(x => x.Movie);
            base.Configure(builder);
        }

        #endregion
    }
}