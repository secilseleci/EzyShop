function addToCart(productId) {
    let count = $("#productCount").val();  

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
