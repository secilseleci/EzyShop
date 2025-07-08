
document.addEventListener("DOMContentLoaded", () => {
    loadCart();
    
});
async function loadCart() {
    const res = await fetchSafe('/api/cart/getcart');
    if (!res || !res.data) return;

    const data = res.data;

    const container = document.getElementById('cart-content');
    const template = document.getElementById('cart-item-template');

    container.innerHTML = '';
    let totalCount = 0;

    data.orderItems.forEach(item => {
        const clone = template.content.cloneNode(true);
        const root = clone.querySelector('.cart-item');

        root.querySelector('.cart-img').src = item.imageUrl;
        root.querySelector('.cart-img').alt = item.productName;

        // Ürün adı ve rengi
        root.querySelector('.cart-name').textContent = item.productName;
        root.querySelector('.cart-color').textContent = item.color ?? '-';

        // Adet input'u - veriyi ata ve veri-id ekle
        const input = root.querySelector('.cart-count');
        input.value = item.count;
        input.setAttribute('data-id', item.id);
        input.setAttribute('data-unit-price', item.productPrice);
        input.setAttribute('data-stock', item.stock);
        // Fiyat gösterimi
        const priceElem = root.querySelector('.cart-price');
        priceElem.textContent = item.totalPrice.toLocaleString('tr-TR') + ' ₺';

        // Artır / azalt butonları
        root.querySelector('.btn-increase').addEventListener('click', () => {
            updateItemCount(item.id, 1);
        });

        root.querySelector('.btn-decrease').addEventListener('click', () => {
            updateItemCount(item.id, -1);
        });

        // Ürünü sil butonu
        root.querySelector('.cart-remove').addEventListener('click', () => {
            removeItem(item.id, root);
        });

        // HTML içine ekle
        container.appendChild(clone);
        totalCount += item.count;
    });

    // Özet verileri güncelle
    recalculateSummary();
}

function recalculateSummary() {
    let totalCount = 0;
    let totalPrice = 0;

    document.querySelectorAll('.cart-count').forEach(input => {
        const count = parseInt(input.value);
        const unitPrice = parseFloat(input.getAttribute('data-unit-price'));

        totalCount += count;
        totalPrice += count * unitPrice;
    });

    document.getElementById('summary-count').textContent = `${totalCount} item(s)`;
    document.getElementById('summary-items-count').textContent = totalCount;
    document.getElementById('summary-total-price').textContent = totalPrice.toLocaleString('tr-TR') + ' ₺';
}

async function updateItemCount(orderItemId, delta) {
    const input = document.querySelector(`.cart-count[data-id='${orderItemId}']`);
    const currentCount = parseInt(input.value);
    const stock = parseInt(input.getAttribute('data-stock'));

    const newCount = currentCount + delta;

    if (newCount < 1) return;

    if (newCount > stock) {
        toastr.warning(`Stokta sadece ${stock} adet var`);
        return;
    }

    const result = await fetchSafe('/api/cart/updatecount', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            orderItemId: orderItemId,
            delta: delta
        })
    });

    if (!result) return;

    if (!result.success) {
        toastr.warning(result.message || "Update failed.");
        return;
    }

    input.value = newCount;

    const unitPrice = parseFloat(input.getAttribute('data-unit-price'));
    const priceElem = input.closest('.cart-item').querySelector('.cart-price');
    priceElem.textContent = (unitPrice * newCount).toLocaleString('tr-TR') + ' ₺';

    recalculateSummary();
    refreshCartIcon();
}



async function removeItem(orderItemId, element) {
    const res = await fetchSafe('/api/cart/removeitem', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(orderItemId)
    });

    if (!res || !res.success) {
        toastr.error("The product could not be deleted: " + (res?.message ?? ""));
        return;
    }

    element.remove();
    recalculateSummary();
    refreshCartIcon();

    if (res.isCartEmpty) {
        window.location.href = '/Cart/Index';
    }
}

