using BagStore.Web.AppConfig.Interface;
using BagStore.Web.Models.Entities.Enums;

namespace BagStore.Web.AppConfig.Implementations
{
    public class EnumMapper : IEnumMapper
    {
        private readonly Dictionary<Type, Dictionary<string, object>> _enumMaps;

        public EnumMapper()
        {
            _enumMaps = new Dictionary<Type, Dictionary<string, object>>
            {
                // 🟢 TrangThaiDonHang
                {
                    typeof(TrangThaiDonHang),
                    new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "Chờ xử lý", TrangThaiDonHang.ChoXuLy },
                        { "Đang giao hàng", TrangThaiDonHang.DangGiaoHang },
                        { "Hoàn thành", TrangThaiDonHang.HoanThanh },
                        { "Đã huỷ", TrangThaiDonHang.DaHuy }
                    }
                },

                // 🟢 TrangThaiPhuongThucThanhToan
                {
                    typeof(TrangThaiPhuongThucThanhToan),
                    new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "COD", TrangThaiPhuongThucThanhToan.COD },
                        { "Chuyển khoản", TrangThaiPhuongThucThanhToan.ChuyenKhoan },
                        { "Ví điện tử", TrangThaiPhuongThucThanhToan.ViDienTu }
                    }
                },

                // 🟢 TrangThaiThanhToan
                {
                    typeof(TrangThaiThanhToan),
                    new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "Thành công", TrangThaiThanhToan.ThanhCong },
                        { "Thất bại", TrangThaiThanhToan.ThatBai },
                        { "Chờ xác nhận", TrangThaiThanhToan.ChoXacNhan },
                        { "Đã hoàn tiền", TrangThaiThanhToan.DaHoanTien }
                    }
                },

                // 🟢 TrangThaiGioHang
                {
                    typeof(TrangThaiGioHang),
                    new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                    {
                        { "Hoạt động", TrangThaiGioHang.HoatDong },
                        { "Đã chuyển đơn", TrangThaiGioHang.DaChuyenDon },
                        { "Đã xóa", TrangThaiGioHang.DaXoa }
                    }
                }
            };
        }

        public TEnum MapToEnum<TEnum>(string value) where TEnum : struct, Enum
        {
            var type = typeof(TEnum);

            if (_enumMaps.TryGetValue(type, out var map) && map.TryGetValue(value, out var result))
                return (TEnum)result;

            throw new ArgumentException($"Giá trị '{value}' không hợp lệ cho enum {type.Name}");
        }

        public string MapToString<TEnum>(TEnum enumValue) where TEnum : struct, Enum
        {
            var type = typeof(TEnum);

            if (!_enumMaps.TryGetValue(type, out var map))
                throw new ArgumentException($"Enum {type.Name} chưa được cấu hình trong EnumMapper");

            var entry = map.FirstOrDefault(x => x.Value.Equals(enumValue));
            return entry.Key ?? enumValue.ToString();
        }
    }
}
