using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Web.Models.Entities.Enums
{
    public enum TrangThaiThanhToan
    {
        // Thành công: Giao dịch tiền tệ đã được xác nhận.
        ThanhCong = 1,

        // Thất bại: Giao dịch không thành công (ví dụ: thẻ hết tiền, lỗi cổng thanh toán).
        ThatBai = 2,

        // Chờ xác nhận: Đang chờ ngân hàng hoặc hệ thống xác nhận (thường dùng cho Chuyển khoản).
        ChoXacNhan = 3,

        // Đã hoàn tiền: Số tiền đã được trả lại cho khách hàng.
        DaHoanTien = 4
    }
}