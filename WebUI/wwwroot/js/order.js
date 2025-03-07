$(document).ready(function () {
    loadOrderTable();
});

function loadOrderTable() {
    $("#orderTable").DataTable({
        "ajax": {
            url: "/Order/GetSellerOrders",  // ✅ API çağrısı
            type: "GET",
            datatype: "json"
        },
        "columns": [
            { data: "orderCode", "width": "15%" },
            { data: "customerName", "width": "15%" },
            { data: "totalAmount", "width": "10%", "render": $.fn.dataTable.render.number(',', '.', 2, '$') },
            { data: "paymentMethod", "width": "10%" },
            {
                data: "status",
                "width": "10%",
                "render": function (data) {
                    let badgeClass = data === "Pending" ? "bg-warning"
                        : data === "Shipped" ? "bg-info"
                            : data === "Delivered" ? "bg-success"
                                : "bg-danger";
                    return `<span class="badge ${badgeClass}">${data}</span>`;
                }
            },
            {
                data: "createdDate",
                "width": "10%",
                "render": function (data) {
                    return new Date(data).toLocaleDateString();
                }
            },
            {
                data: "id",
                "width": "10%",
                "render": function (data) {
                    return `<button class="btn btn-info btn-sm" onclick="viewOrderDetails('${data}')">View</button>`;
                }
            }
        ],
        "order": [[5, "desc"]], // 📅 Tarihe göre sıralama (en yeni siparişler en üstte)
        "pageLength": 10
    });
}

function viewOrderDetails(orderId) {
    // Burada sipariş detaylarını açan bir modal veya sayfa yönlendirme olabilir
    alert("Order ID: " + orderId);
}
