using BagStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BagStore.Web.Areas.Client.ViewComponents
{
    public class DanhMucLoaiTuiViewComponent : ViewComponent
    {
        private readonly IDanhMucLoaiTuiService _danhMucLoaiTuiService;

        public DanhMucLoaiTuiViewComponent(IDanhMucLoaiTuiService danhMucLoaiTuiService)
        {
            _danhMucLoaiTuiService = danhMucLoaiTuiService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = await _danhMucLoaiTuiService.GetAllAsync();

            return View("DanhMucLoaiTui", response.Data);
        }
    }
}