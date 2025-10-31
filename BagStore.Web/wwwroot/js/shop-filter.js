function applyFilter(page = 1) {
    let filters = { page: page };
    $(".filter-input:checked").each(function () {
        filters[$(this).data("type")] = $(this).val();
    });

    $.get("/Client/Shop/Filter", filters, function (result) {
        $("#product-list").html(result);
    });

    $(".filter-input:checked").each(function () {
        let type = $(this).data("type");
        let value = $(this).val();
        filters[type] = value;
    });

    $.ajax({
        url: "/Client/Shop/Filter",
        type: "GET",
        data: filters,
        success: function (result) {
            $("#product-list").html(result);
        }
    });
}

$(".filter-input").on("change", function () {
    applyFilter();
});
$(document).on("click", ".pagination a", function (e) {
    e.preventDefault();
    let page = $(this).data("page");
    applyFilter(page);
});
