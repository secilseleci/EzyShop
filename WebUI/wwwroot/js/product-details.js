console.log("product-details.js yüklendi");

$(document).ready(function () {
 
    $("#addToCartBtn").on("click", function (e) {
        e.preventDefault();

        const productId = $(this).data("product-id"); 
        addToCart(productId); 
    });
});
function addToCart(productId) {

    $.ajax({
        url: "/api/cart/addtocart",
        type: "POST",
        data: { productId },
        success: function (response) {
            if (!response.success) {
                toastr.error(response.message);
                return;
            }

            toastr.success(response.message);

        },
        error: function () {
            toastr.error("An unexpected error occurred.");
        }
    });
}

