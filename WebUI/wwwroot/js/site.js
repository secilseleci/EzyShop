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
        const res = await fetch(url, options);

        // 401: Oturum yok veya zaman aşımı
        if (res.status === 401) {
            window.location.href = "/Auth/Login";
            return null;
        }

        // 403: Yetki yok
        if (res.status === 403) {
            alert("You are not authorized to perform this operation.");
            return null;
        }

        // 500+: Sunucu hatası
        if (res.status >= 500) {
            alert("An error occurred on the server side. Try again later.");
            return null;
        }

        // Diğer 4xx hataları (mesela 400)
        if (!res.ok) {
            let message = "An unexpected error occurred.";
            try {
                const data = await res.json();
                message = data.message || message;
            } catch (e) {
                console.warn("The error message is not JSON.");
            }
            alert(message);
            return null;
        }

        // Her şey yolundaysa JSON'u dön
        return await res.json();

    } catch (err) {
        alert("Connection error: " + err.message);
        return null;
    }
}
