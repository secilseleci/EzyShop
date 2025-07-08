document.addEventListener("DOMContentLoaded", function () {
    let priceInput = document.querySelector("input[name='Price']");

    if (priceInput) {
        priceInput.addEventListener("input", function () {
            this.value = this.value.replace(",", ".");
        });
    }
});

async function fetchSafe(url, options = {}) {
    try {
        const res = await fetch(url, {
            credentials: "include",
            ...options
        });

        if (res.status === 401) {
            window.location.href = "/Auth/Login";
            return null;
        }

        if (res.status === 403) {
            toastr.error("You are not authorized to perform this operation.");
            return null;
        }

        if (res.status >= 500) {
            toastr.error("An error occurred on the server side. Try again later.");
            return null;
        }
        const data = await res.json();
        return data;

    } catch (err) {
        toastr.error("Bağlantı hatası: " + err.message);
        return null;
    }
}

window.refreshCartIcon = async function () {
    const result = await fetchSafe("/api/cart/count");
    if (!result || typeof result.count === "undefined") return;

    const badge = document.getElementById("cart-count-badge");
    if (badge) {
        badge.textContent = result.count;
    }
}

document.addEventListener("DOMContentLoaded", function () {
    if (!window.toastrConfigured) {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-bottom-right",
            "preventDuplicates": true,
            "timeOut": "5000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };

        window.toastrConfigured = true;
    }
});


