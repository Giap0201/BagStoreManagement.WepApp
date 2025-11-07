using BagStore.Models.Common;
using BagStore.Web.Areas.Admin.Models;

namespace BagStore.Web.Repositories.Interfaces
{
    public interface IDashboardService
    {
        Task<BaseResponse<DashBoardPhanHoiDTO>> LayDuLieuDashboardAsync(int namHienTai);
    }
}