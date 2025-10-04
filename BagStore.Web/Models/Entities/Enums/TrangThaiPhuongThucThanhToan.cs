using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagStore.Domain.Enums
{
    public enum TrangThaiPhuongThucThanhToan
    {
        // COD (Cash On Delivery): Thanh toán khi nhận hàng.
        COD = 1,

        // Chuyển khoản: Thanh toán qua ngân hàng.
        ChuyenKhoan = 2,

        // Ví điện tử: Thanh toán qua các cổng như Momo, ZaloPay, v.v.
        ViDienTu = 3
    }
}