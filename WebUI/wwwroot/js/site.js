document.addEventListener("DOMContentLoaded", function () {
    let priceInput = document.querySelector("input[name='Price']");

    if (priceInput) {
        priceInput.addEventListener("input", function () {
            this.value = this.value.replace(",", ".");
        });
    }
});

$(document).ready(function () {
    if (isCustomer === "true") {
        $.ajax({
            url: "/Cart/GetCartLineCount",
            type: "GET",
            success: function (data) {
                if (data.success) {
                    $("#cartLineCount").text(data.count);
                }
            },
            error: function () {
                console.log("Cart count çekilemedi.");
            }
        });
    }
});
