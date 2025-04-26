$(document).ready(function () {
    productsTable = $('#tblProduct').DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: '/Seller/Product/GetProducts?status=' + productStatus, 
            type: 'GET',
            data: function (d) {
                return d;
            },
            error: function (xhr, error, thrown) {
                toastr.error("An error occurred while loading products.");
            }
        },
        columns: [
            {
                title: "Image", data: 'imageUrl', width: "10%",
                render: function (data) {
                    if (!data) return "-";
                    return `<img src="${data}" class="img-thumbnail" style="max-height:60px;" />`;
                }
            },
            { title: "Name", data: 'productName', width: "15%" },

            { title: "Category", data: 'categoryName', width: "10%" },
            {
                title: "Details", data: "productId",
                width: "15%",
                render: function (data) {
                    return `<a onClick="viewDetails('${data}')" class="btn btn-info btn-sm rounded d-block text-center">
                        <i class="bi bi-eye"></i> Details
                    </a>`;
                } },
            {
                title: "Actions", data: 'productId', width: "10%",
                render: function (data) {
                    return `<div class="btn-group">
                        <a href="/Seller/Product/Edit?id=${data}" class="btn btn-sm btn-warning mx-2">
                            <i class="bi bi-pencil-square"></i>
                        </a>               
                        <button onClick="deleteProduct('${data}')" class="btn btn-sm btn-danger mx-2">
                            <i class="bi bi-trash-fill"></i>
                        </button>
                    </div>`;
                }
            }
        ]
    });
});

function viewDetails(id) {
   
    $.ajax({
        url: `/Seller/Product/Details/${id}?status=${productStatus}`,
        type: 'GET',
        success: function (html) {
            $("#productDetailModalContent").html(html);
            $("#productDetailModal").modal('show');
        }
    });
}
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



