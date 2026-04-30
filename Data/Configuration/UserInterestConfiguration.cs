using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMediaBackend.Data.Entities;
using System.Reflection.Emit;

namespace SocialMediaBackend.Data.Configuration
{
    public class UserInterestConfiguration : IEntityTypeConfiguration<UserInterest>
    {
        public void Configure(EntityTypeBuilder<UserInterest> builder)
        {
            builder
                .HasKey(ui => new { ui.UserId, ui.InterestId });

            builder
                .HasOne(ui => ui.User)
                .WithMany(u => u.UserInterests)
                .HasForeignKey(ui => ui.UserId);

            builder
                .HasOne(ui => ui.Interest)
                .WithMany(i => i.UsersInterests)
                .HasForeignKey(ui => ui.InterestId);
        }
    }
}
