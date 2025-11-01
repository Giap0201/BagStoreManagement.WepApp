function applyFilter(page = 1) {
    const form = document.getElementById('filterForm');
    const formData = new FormData(form);
    formData.append('page', page);

    const params = new URLSearchParams(formData).toString();

    fetch(`/Client/Shop/Filter?${params}`)
        .then(res => res.text())
        .then(html => {
            document.getElementById('product-list').innerHTML = html;
            window.scrollTo({ top: 0, behavior: 'smooth' });
        })
        .catch(() => {
            document.getElementById('product-list').innerHTML =
                '<div class="alert alert-danger text-center">Lỗi tải sản phẩm.</div>';
        });
}

// Khi người dùng đổi filter
document.addEventListener("change", function (e) {
    if (e.target.classList.contains("filter-input")) {
        applyFilter();
    }
});

// Khi người dùng bấm trang
document.addEventListener("click", function (e) {
    if (e.target.matches(".pagination a")) {
        e.preventDefault();
        const page = e.target.getAttribute("data-page");
        applyFilter(page);
    }
});
