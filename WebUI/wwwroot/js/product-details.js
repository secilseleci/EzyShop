document.addEventListener('DOMContentLoaded', () => {
    const btn = document.getElementById("addToCartBtn");
    if (!btn) {
        return;
    }

    btn.addEventListener("click", async (e) => {
        e.preventDefault();

        const productId = btn.dataset.productId;
        await addToCart(productId);
    });
});

async function addToCart(productId) {
    const formData = new FormData();
    formData.append("productId", productId);

    const result = await fetchSafe("/api/cart/addtocart", {
        method: "POST",
        body: formData
    });

    if (!result) return;

    $("#cart-icon").load("/Cart/CartIcon");
    toastr.success(result.message);
}
