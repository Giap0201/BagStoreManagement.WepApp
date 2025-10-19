using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Web.Models.Entities.Enums
{
    public enum TrangThaiDonHang
    {
        // Chờ xử lý: Đơn hàng mới được đặt, đang chờ xác nhận và đóng gói.
        ChoXuLy = 1,

        // Đang giao hàng: Đơn hàng đã được vận chuyển.
        DangGiaoHang = 2,

        // Hoàn thành: Đơn hàng đã được giao thành công và thanh toán xong.
        HoanThanh = 3,

        // Đã hủy: Đơn hàng bị hủy bởi khách hàng hoặc quản trị viên.
        DaHuy = 4
    }
}