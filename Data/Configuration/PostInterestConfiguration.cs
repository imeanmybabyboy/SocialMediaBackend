using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaBackend.Data.Entities;

namespace SocialMediaBackend.Data.Configuration
{
    public class PostInterestConfiguration : IEntityTypeConfiguration<PostInterest>
    {
        public void Configure(EntityTypeBuilder<PostInterest> builder)
        {
            builder
                .HasKey(pi => new { pi.PostId, pi.InterestId });

            builder
                .HasOne(pi => pi.Post)
                .WithMany(p => p.PostsInterests)
                .HasForeignKey(pi => pi.PostId);

            builder
                .HasOne(pi => pi.Interest)
                .WithMany(i => i.PostsInterests)
                .HasForeignKey(pi => pi.InterestId);
        }

    }
}
