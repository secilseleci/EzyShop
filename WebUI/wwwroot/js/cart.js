function addToCart(productId) {
    let count = parseInt($("#productCount").val());
    if (count < 1) {
        toastr.error(Messages.CartItemCountError);
        return;
    }
    if (count > 100) {
        toastr.error(Messages.CartItemCountError);
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
            toastr.error(Messages.AddShoppingCartItemError);
        }
    });
}

function updateCartCount(count) {
    $("#cartItemCount").text(`(${count})`);
}
