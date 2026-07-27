using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaBackend.Data.Entities;

namespace SocialMediaBackend.Data.Configuration
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasOne(c => c.User1)
                .WithMany()
                .HasForeignKey(c => c.User1Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.User2)
                .WithMany()
                .HasForeignKey(c => c.User2Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => new { c.User1Id, c.User2Id })
                .IsUnique();

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
