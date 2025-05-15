document.addEventListener('DOMContentLoaded', loadCart);

function loadCart() {
    fetch('/api/cart/getcart')
        .then(res => res.json())
        .then(res => {
            if (!res.success) return;

            const data = res.data;
            const container = document.getElementById('cart-content');
            const template = document.getElementById('cart-item-template');

            container.innerHTML = '';
            let totalCount = 0;

            data.orderItems.forEach(item => {
                const clone = template.content.cloneNode(true);
                const root = clone.querySelector('.cart-item');

                // Görsel alanlar
                root.querySelector('.cart-img').src = item.imageUrl;
                root.querySelector('.cart-img').alt = item.productName;
                root.querySelector('.cart-name').textContent = item.productName;
                root.querySelector('.cart-color').textContent = item.color ?? '-';

                // Adet input'u
                const input = root.querySelector('.cart-count');
                input.value = item.count;
                input.setAttribute('data-id', item.id);
                input.setAttribute('data-unit-price', item.productPrice);

                // Fiyat alanı
                const priceElem = root.querySelector('.cart-price');
                priceElem.textContent = item.totalPrice.toLocaleString('tr-TR') + ' ₺';

                // Butonlar
                root.querySelector('.btn-increase').addEventListener('click', () => {
                    updateItemCount(item.id, 1);
                });

                root.querySelector('.btn-decrease').addEventListener('click', () => {
                    updateItemCount(item.id, -1);
                });

                root.querySelector('.cart-remove').addEventListener('click', () => {
                    removeItem(item.id, root);
                });

                container.appendChild(clone);
                totalCount += item.count;
            });

            recalculateSummary();
        });
}

function updateItemCount(orderItemId, delta) {
    fetch('/api/cart/updatecount', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            orderItemId: orderItemId,
            delta: delta
        })
    })
        .then(res => res.json())
        .then(res => {
            if (!res.success) {
                alert("Count could not update: " + res.message);
                return;
            }

            // UI'deki input ve fiyatı güncelle
            const input = document.querySelector(`.cart-count[data-id='${orderItemId}']`);
            const newCount = parseInt(input.value) + delta;

            if (newCount < 1 || newCount > 100) return;

            input.value = newCount;

            const unitPrice = parseFloat(input.getAttribute('data-unit-price'));
            const priceElem = input.closest('.cart-item').querySelector('.cart-price');
            priceElem.textContent = (unitPrice * newCount).toLocaleString('tr-TR') + ' ₺';

            recalculateSummary();
        });
}

function removeItem(orderItemId, element) {
    fetch('/api/cart/remove', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(orderItemId)
    })
        .then(res => res.json())
        .then(res => {
            if (!res.success) {
                alert("Ürün silinemedi: " + res.message);
                return;
            }

            element.remove();
            recalculateSummary();
        });
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
