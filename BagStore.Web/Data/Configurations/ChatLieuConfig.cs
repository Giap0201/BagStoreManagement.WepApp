using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BagStore.Data.Configurations
{
    public class ChatLieuConfig : IEntityTypeConfiguration<ChatLieu>
    {
        public void Configure(EntityTypeBuilder<ChatLieu> builder)
        {
            builder.ToTable("ChatLieu");

            builder.HasKey(x => x.MaChatLieu);

            builder.Property(x => x.TenChatLieu)
                   .IsRequired()
                   .HasMaxLength(100);
            builder.HasIndex(x => x.TenChatLieu).IsUnique();

            builder.Property(x => x.MoTa)
                   .HasMaxLength(500);

            builder.HasMany(x => x.SanPhams)
                   .WithOne(x => x.ChatLieu)
                   .HasForeignKey(x => x.MaChatLieu)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}