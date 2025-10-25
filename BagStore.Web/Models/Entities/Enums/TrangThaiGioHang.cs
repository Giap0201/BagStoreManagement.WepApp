using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Web.Models.Entities.Enums
{
    public enum TrangThaiGioHang
    {
        // Hoạt động: Mục đang ở trong giỏ hàng và có thể được mua.
        HoatDong = 1,

        // Đã chuyển đơn: Mục đã được đưa vào một đơn hàng và không còn ở trong giỏ.
        DaChuyenDon = 2,

        // Đã xóa: Mục đã bị khách hàng xóa khỏi giỏ hàng.
        DaXoa = 3
    }
}