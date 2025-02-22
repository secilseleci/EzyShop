function addToCart(productId) {
    let count = parseInt($("#productCount").val());
    if (count < 1) {
        toastr.error("Adet en az 1 olmalýdýr.");
        return;
    }
    if (count > 100) {
        toastr.error("Adet 100'den fazla olamaz.");
        return;
    }
    $.ajax({
        url: "/ShoppingCart/AddToCart",
        type: "POST",
        data: { productId: productId, count: count },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                updateCartCount(response.cartItemCount);
            } else {
                toastr.error(response.message);
            }
        },
        error: function () {
            toastr.error("An error occurred while adding the product to the cart.");
        }
    });
}

function updateCartCount(count) {
    $("#cartItemCount").text(`(${count})`);
}
