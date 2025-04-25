

document.addEventListener("DOMContentLoaded", function () {
    loadCartLines();
    refreshCartIconCount();
});

function loadCartLines() {
 
    fetch("/Cart/GetCartLines")
        .then(res => res.json())
        .then(data => {
            if (!data.success) {
                toastr.error(data.message || "Cart could not be loaded.");
                return;
            }

            const cartBody = document.getElementById("cartBody");
            const totalPrice = document.getElementById("totalPrice");
            const emptyMsg = document.getElementById("emptyCartMsg");
            const cartTable = document.getElementById("cartTableContainer");

            cartBody.innerHTML = "";
 
            if (data.cartLines.length === 0) {
                emptyMsg.classList.remove("d-none");
                cartTable.classList.add("d-none");
                totalPrice.textContent = "0 ₺";
                return;
            }

            emptyMsg.classList.add("d-none");
            cartTable.classList.remove("d-none");

            let total = 0;

            data.cartLines.forEach(item => {
                total += item.lineTotal;

                const row = `
                    <tr>
                        <td>
                            <div class="d-flex align-items-center">
                                <img src="${item.imageUrl}" alt="${item.productName}" width="60" class="rounded shadow-sm me-3" />
                                <div><strong>${item.productName}</strong></div>
                            </div>
                        </td>
                        <td>${item.shopName}</td>
                        <td>${item.color ?? ""}</td>
                        <td>${item.price.toFixed(2)} ₺</td>
                        <td class="text-center">
                            <div class="input-group input-group-sm justify-content-center" style="width: 140px; margin: auto;">
                                <button class="btn btn-outline-secondary btn-sm" onclick="updateQuantity('${item.productId}', 'decrease')">-</button>
                                <input type="text" class="form-control text-center" value="${item.count}" readonly style="max-width: 40px;" />
                                <button class="btn btn-outline-secondary btn-sm" onclick="updateQuantity('${item.productId}', 'increase')">+</button>
                            </div>
                        </td>
                        <td>${item.lineTotal.toFixed(2)} ₺</td>
                        <td class="text-end">
                            <button class="btn btn-outline-danger btn-sm" onclick="removeItem('${item.productId}')">
                                <i class="bi bi-trash"></i>
                            </button>
                        </td>
                    </tr>
                `;

                cartBody.insertAdjacentHTML("beforeend", row);
            });

            totalPrice.textContent = `${total.toFixed(2)} ₺`;
        })
        .catch(error => {
             toastr.error("Cart could not be loaded.");
        });
}


function clearCart() {
    fetch("/Cart/ClearCart", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({})
    })
        .then(res => res.json())
        .then(data => {
             if (!data.success) {
                toastr.error(data.message);
                return;
            }

            toastr.success(data.message);

            loadCartLines();
            refreshCartIconCount();
        })
        .catch(err => {
             toastr.error("Cart could not be cleared.");
        });
}
