let productsTable;

$(document).ready(function () {
    productsTable = $('#tblProduct').DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: '/Seller/Product/GetPaginatedProducts',
            type: 'GET',
        },

        columns: [
            {
                title: "Image", data: 'imageUrl', "width": "10%",

                "render": function (data) {
                    if (!data) return "-";
                    return `<img src="${data}" class="img-thumbnail" style="max-height:60px;" />`;
                }
            },
            { title: "Category", data: 'categoryName', "width": "10%" },
            { title: "Name", data: 'name', "width": "15%" },
            { title: "Color", data: 'color', "width": "10%" },
            { title: "Price", data: 'price', "width": "5%" },
            {
                title: "Stock", data: 'stockStatus', "width": "10%",
                render: function (data) {
                    let badgeClass = data === "In Stock" ? "success" : "danger";
                    return `<span class="badge bg-${badgeClass}">${data}</span>`;
                }
            },
            {
                title: "Status", data: 'isActive', "width": "10%",

                "render": function (data, type, row) {
                    let btnClass = data ? "btn-success" : "btn-danger";
                    let btnText = data ? "Active" : "Inactive";
                    return `<button onClick="toggleProductStatus('${row.id}')" class="btn btn-sm ${btnClass}">
                                ${btnText}
                            </button>`;
                }
            },
            {
                title: "Actions", data: 'id', "width": "10%",

                "render": function (data) {
                    return `<div class="btn-group">
                     <a href="/Seller/Product/Edit?id=${data}" class="btn btn-sm btn-warning mx-2"> <i class="bi bi-pencil-square"></i></a>               
                     <button onClick="deleteProduct('${data}')" class="btn btn-sm btn-danger mx-2"> <i class="bi bi-trash-fill"></i></button>
                    </div>`;
                }
            }
        ]
    });
});

//Delete Function
function deleteProduct(productId) {
    $.ajax({
        url: `/Seller/Product/Delete`,
        type: 'POST',
        data: { productId },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                productsTable.ajax.reload(null, false);
            } else {
                toastr.error(response.message);
            }
        },
        error: function (response) {
            toastr.error("An error occurred. Unable to delete the product.");
        }
    });
}
//Delete Function
 

function toggleProductStatus(productId) {
    $.ajax({
        url: `/Seller/Product/ToggleStatus`,  
        type: "POST",
        data: { productId },  
        success: function (response) {
            toastr.success(response.message);
            productsTable.ajax.reload();
        },
        error: function (xhr) {
            const err = xhr.responseJSON?.message || "Status could not be changed.";
            toastr.error(err);
        }
    });
}

