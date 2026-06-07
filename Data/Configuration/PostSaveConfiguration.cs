using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaBackend.Data.Entities;

namespace SocialMediaBackend.Data.Configuration
{
    public class PostSaveConfiguration : IEntityTypeConfiguration<PostSave>
    {
        public void Configure(EntityTypeBuilder<PostSave> builder)
        {
            builder.HasKey(s => new { s.UserId, s.PostId });

            builder.HasOne(s => s.User)
                .WithMany(u => u.PostSaves)
                .HasForeignKey(s => s.UserId);

            builder.HasOne(s => s.Post)
                  .WithMany(p => p.Saves)
                  .HasForeignKey(s => s.PostId);
        }
    }
}
