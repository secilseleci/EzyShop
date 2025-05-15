document.addEventListener('DOMContentLoaded', () => {
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

                clone.querySelector('.cart-img').src = item.imageUrl;
                clone.querySelector('.cart-img').alt = item.productName;
                clone.querySelector('.cart-name').textContent = item.productName;
                clone.querySelector('.cart-color').textContent = item.color ?? '-';
                clone.querySelector('.cart-count').value = item.count;
                clone.querySelector('.cart-price').textContent = item.totalPrice.toLocaleString('tr-TR') + ' ₺';

                clone.querySelector('.cart-remove').addEventListener('click', () => {
                    removeItem(item.id);
                });

                container.appendChild(clone);
                totalCount += item.count;
            });

             document.getElementById('summary-count').textContent = `${totalCount} item(s)`;
            document.getElementById('summary-items-count').textContent = totalCount;
            document.getElementById('summary-total-price').textContent = data.totalAmount.toLocaleString('tr-TR') + ' ₺';
        });
});

function removeItem(id) {
    console.log("Silinecek ürün ID:", id);
    // İlgili API çağrısı buraya eklenebilir
}
