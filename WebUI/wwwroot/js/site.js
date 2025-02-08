document.addEventListener("DOMContentLoaded", function () {
    let priceInput = document.querySelector("input[name='Price']");

    if (priceInput) {
        priceInput.addEventListener("input", function () {
            this.value = this.value.replace(",", "."); // Virgülü noktaya çevir
        });
    }
});
