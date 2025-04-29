document.addEventListener("DOMContentLoaded", function () {
    let priceInput = document.querySelector("input[name='Price']");

    if (priceInput) {
        priceInput.addEventListener("input", function () {
            this.value = this.value.replace(",", ".");
        });
    }
});

$(document).ajaxStart(function () {
    // Tüm ajaxlar başladığında loader göster
    $('#globalLoader').show();
}).ajaxStop(function () {
    // Tüm ajaxlar bitince loader gizle
    $('#globalLoader').hide();
});
//document.addEventListener("DOMContentLoaded", function () {
//    if (isUserAuthenticated === "true") {
//        refreshCartIconCount();
//    }
//});

//function refreshCartIconCount() {
//    fetch("/Cart/GetCartLineCount")
//        .then(res => res.json())
//        .then(data => {
//            if (data.success) {
//                document.getElementById("cartLineCount").textContent = data.count;
//            }
//        })
//        .catch(() => {
//            console.warn("Cart count çekilemedi.");
//        });
//}