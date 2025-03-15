$(document).ready(function () {
    var orderId = getOrderIdFromUrl();
    if (orderId) {
        loadOrderDetail(orderId);
    }

    setupEventListeners(); 
});

// ✅ Sipariş ID'yi URL'den al
function getOrderIdFromUrl() {
    return window.location.pathname.split('/').pop();
}

// ✅ Sipariş Detaylarını Getir
function loadOrderDetail(orderId) {
    $.ajax({
        url: `/Order/GetOrderDetail/${orderId}`,
        type: "GET",
        success: function (response) {
            if (!response || !response.data) return;
           
            var order = response.data;

            console.log("DEBUG: Order Status = ", order.status);
            // 📌 Sayfanın en üstündeki sipariş durumu   
            var statusBadge = "";
            if (order.status === "Pending" || order.status === 1) {
                statusBadge = `<span class="badge bg-warning p-2">Pending</span>`;
            }
            else if (order.status === "Shipped" || order.status === 2) {
                statusBadge = `<span class="badge bg-success p-2">Shipped</span>`;
            }
            else if (order.status === "Delivered" || order.status === 3) {
                statusBadge = `<span class="badge bg-info p-2">Delivered</span>`;
            }
            else if (order.status === "Cancelled" || order.status === 4) {
                statusBadge = `<span class="badge bg-danger p-2">Cancelled</span>`;
            }
            else {
                statusBadge = `<span class="badge bg-secondary p-2">${order.status}</span>`;
            }

            $("#orderStatus").html(statusBadge);  
            // Sipariş Bilgileri   
            $("#orderCode").text(order.orderCode || "-");
            $("#paymentMethod").text(order.paymentMethod || "-");
            $("#createdDate").text(order.createdDate ? new Date(order.createdDate).toLocaleDateString() : "-");
            $("#totalAmount").text(order.totalAmount ? `$${order.totalAmount.toFixed(2)}` : "$0.00");

            // Müşteri Bilgileri   
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

// ✅ Tüm Click Eventler
function setupEventListeners() {
    
    $(document).on("click", ".product-image", function () {
        $("#modalImage").attr("src", $(this).attr("src"));
        $("#imageModal").modal("show");
    });

     $(document).on("click", "#btnShipped", function () {
        markOrderAsShipped();
    });
}

 function markOrderAsShipped() {
    var orderId = getOrderIdFromUrl();

    if (!orderId) {
        toastr.error("Order ID not found!");
        return;
    }

    Swal.fire({
        title: 'Are you sure?',
        text: "Do you want to mark this order as shipped?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, mark as shipped!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/Order/MarkAsShipped?orderId=${orderId}`,
                type: "POST",
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message);
                        $("#btnShipped")
                            .removeClass("btn-primary")   
                            .addClass("btn-success")  
                            .text("Shipped")   
                            .prop("disabled", true);   

                        $("#orderStatus").html(`<span class="badge bg-success p-2">Shipped</span>`);
                    }
                    else {
                        toastr.error(response.message);
                    }
                    
                },
                error: function (jqXHR) {
                    toastr.error(jqXHR.responseJSON?.message || "An error occurred!");
                }
            });
        }
    });
}
