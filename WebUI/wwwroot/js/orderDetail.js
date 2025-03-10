$(document).ready(function () {
    var orderId = window.location.pathname.split('/').pop();
    if (orderId) {
        loadOrderDetail(orderId);
    }

    // Resme tıklanınca modal aç
    $(document).on("click", ".product-image", function () {
        $("#modalImage").attr("src", $(this).attr("src"));
        $("#imageModal").modal("show");
    });
});

function loadOrderDetail(orderId) {
    $.ajax({
        url: `/Order/GetOrderDetail/${orderId}`,
        type: "GET",
        success: function (response) {
            if (!response || !response.data) return;

            var order = response.data;

            // Sipariş Bilgilerini Güncelle
            $("#orderCode").text(order.orderCode || "-");
            $("#paymentMethod").text(order.paymentMethod || "-");
            $("#createdDate").text(order.createdDate ? new Date(order.createdDate).toLocaleDateString() : "-");
            $("#totalAmount").text(order.totalAmount ? `$${order.totalAmount.toFixed(2)}` : "$0.00");

            // Müşteri Bilgilerini Güncelle
            $("#customerName").text(order.customerName || "-");
            $("#customerEmail").text(order.customerEmail || "-");
            $("#customerAddress").text(order.customerAddress || "-");
            $("#customerPhone").text(order.customerPhone || "-");

            // Ürünleri Listele
            var orderItemsHtml = order.orderItems?.map(item => `
                <tr>
                    <td>
                        <img src="${item.imageUrl ? '/' + item.imageUrl : '/images/no-image.png'}" 
                             class="product-image" 
                             style="width: 50px; height: 50px; border-radius:5px; border:1px solid #ddd; cursor: pointer;">
                    </td>
                    <td>${item.productName || "-"}</td>
                    <td>${item.color || "-"}</td>
                    <td>${item.count || 0}</td>
                    <td>$${item.productPrice?.toFixed(2) || "0.00"}</td>
                    <td>$${(item.count * (item.productPrice || 0)).toFixed(2)}</td>
                </tr>
            `).join("") || '<tr><td colspan="6" class="text-center">No products found</td></tr>';

            $("#orderItemsTable").html(orderItemsHtml);
        },
        error: function (jqXHR) {
            console.error("❌ AJAX Hatası:", jqXHR.responseText);
        }
    });
}
