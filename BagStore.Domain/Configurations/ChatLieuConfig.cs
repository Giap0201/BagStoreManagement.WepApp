using BagStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ChatLieuConfig : IEntityTypeConfiguration<ChatLieu>
{
    public void Configure(EntityTypeBuilder<ChatLieu> builder)
    {
        builder.HasKey(e => e.MaChatLieu);
        builder.Property(e => e.TenChatLieu).IsRequired().HasMaxLength(100);
    }
}