$(document).ready(function () {
    $("#addToCartBtn").on("click", function (e) {
        e.preventDefault();

        if (!isUserAuthenticated) {
            window.location.href = "/Account/Login";
            return;
        }

        let productId = $(this).data("product-id");

        $.ajax({
            url: "/Cart/AddToCart",
            type: "POST",
            data: { productId },
            success: function (response) {
                if (!response.success) {
                    toastr.error(response.message);
                    return;
                }

                toastr.success(response.message);

                 setTimeout(() => {
                    refreshCartIconCount();
                }, 300); 
            },
            error: function () {
                toastr.error("An unexpected error occurred.");
            }
        });
    });
});
