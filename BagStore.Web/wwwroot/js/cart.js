const checkAll = document.querySelectorAll(".check-all");
const checkItems = document.querySelectorAll(".check-item");
const totalPrice = document.querySelector(".total-price");
const countSpan = document.querySelector(".count");

checkAll.forEach((ca) =>
    ca.addEventListener("change", (e) => {
        checkItems.forEach((ci) => (ci.checked = e.target.checked));
        updateTotal();
    })
);

checkItems.forEach((ci) => ci.addEventListener("change", updateTotal));

function updateTotal() {
    let total = 0;
    let count = 0;
    document.querySelectorAll(".cart-item").forEach((item) => {
        const cb = item.querySelector(".check-item");
        if (cb.checked) {
            const price = parseInt(
                item
                    .querySelector(".total-col")
                    .textContent.replace(/\D/g, "")
            );
            total += price;
            count++;
        }
    });
    totalPrice.textContent = total.toLocaleString() + "₫";
    countSpan.textContent = count;
}