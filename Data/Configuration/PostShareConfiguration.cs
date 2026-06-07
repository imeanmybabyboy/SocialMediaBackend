using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaBackend.Data.Entities;

namespace SocialMediaBackend.Data.Configuration
{
    public class PostShareConfiguration : IEntityTypeConfiguration<PostShare>
    {
        public void Configure(EntityTypeBuilder<PostShare> builder)
        {
            builder.HasKey(s => new { s.UserId, s.PostId });

            builder.HasOne(s => s.User)
                .WithMany(u => u.PostShares)
                .HasForeignKey(s => s.UserId);

            builder.HasOne(s => s.Post)
              .WithMany(p => p.Shares)
              .HasForeignKey(s => s.PostId);
        }
    }
}
