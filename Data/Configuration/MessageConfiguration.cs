using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaBackend.Data.Entities;

namespace SocialMediaBackend.Data.Configuration
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(m => m.Text)
                .IsRequired()
                .HasMaxLength(2000);

            builder.HasIndex(m => new { m.ChatId, m.CreatedAt });

            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
