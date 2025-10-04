using BagStore.Domain.Entities.IdentityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Configurations
{
    public class KhachHangProfileConfig : IEntityTypeConfiguration<KhachHangProfile>
    {
        public void Configure(EntityTypeBuilder<KhachHangProfile> builder)
        {
            builder.HasKey(x => x.UserId);

            //quan he 1-1 voi user
            builder.HasOne(x => x.User).WithOne(x => x.KhachHangProfile)
                .HasForeignKey<KhachHangProfile>(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); // xoa user thi xoa profile
        }
    }
}