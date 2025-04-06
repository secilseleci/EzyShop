$(document).ready(function () {
    loadOrderTable();
});

function loadOrderTable() {
    $("#orderTable").DataTable({
        "ajax": {url: '/order/getsellerorders' }, 
            
           
        
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
                    return `<a href="/Order/OrderDetail/${data}" class="btn btn-info btn-sm">
                                <i class="bi bi-eye"></i> View
                            </a>`;
                }
            }
        ],
        "order": [[5, "desc"]],  
        "pageLength": 10
    });
}

 
