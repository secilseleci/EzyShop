document.addEventListener("DOMContentLoaded", function () {
    let priceInput = document.querySelector("input[name='Price']");

    if (priceInput) {
        priceInput.addEventListener("input", function () {
            this.value = this.value.replace(",", ".");  
        });
    }
});

 
    $(document).ready(function () {
        $.ajax({
            url: "/ShoppingCart/GetCartItemCount",
            type: "GET",
            success: function (data) {
                if (data.success) {
                    $("#cartItemCount").text(data.count);
                }
            },
            error: function () {
                console.log("Cart count çekilemedi.");
            }
        });
    });
 
