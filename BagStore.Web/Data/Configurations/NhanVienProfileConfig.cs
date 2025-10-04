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
    public class NhanVienProfileConfig : IEntityTypeConfiguration<NhanVienProfile>
    {
        public void Configure(EntityTypeBuilder<NhanVienProfile> builder)
        {
            builder.HasKey(x => x.UserId);

            //quan he 1-1 voi application user
            builder.HasOne(x => x.ApplicationUser)
                .WithOne(x => x.NhanVienProfile)
                .HasForeignKey<NhanVienProfile>(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); // xoa user thi xoa profile

            builder.Property(x => x.ChucVu)
                .HasMaxLength(100);
        }
    }
}