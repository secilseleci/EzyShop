$(document).ready(function () {
    productsTable = $('#tblProduct').DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: '/seller/api/products?status=' + productStatus,
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
                title: "Details", data: "productId", width: "15%",
                render: function (data) {
                    return `<a onClick="viewDetails('${data}')" class="btn btn-info btn-sm rounded d-block text-center">
                                <i class="bi bi-eye"></i> Details
                            </a>`;
                }
            },
            {
                title: "Actions", data: 'productId', width: "10%",
                render: function (data) {
                    let html = "";
                    if (productStatus === "Available") {
                        html += `<a href="/Seller/Product/Update?id=${data}" class="btn btn-sm btn-warning mx-2">
                                    Edit
                                 </a>`;
                        html += `<button onClick="deactivateProduct('${data}')" class="btn btn-sm btn-danger mx-2">
                                    Deactivate
                                 </button>`;
                    }
                    else if (productStatus === "SoldOut") {
                        html += `<button onClick="reactivateProduct('${data}')" class="btn btn-sm btn-success mx-2">
                                    Reactivate
                                 </button>`;
                        html += `<button onClick="deleteProduct('${data}')" class="btn btn-sm btn-danger mx-2">
                                    Delete
                                 </button>`;
                    }

                    return `<div class="btn-group">${html}</div>`;
                }
            }
        ]
    });

    $('#productReactivateModal').on('hidden.bs.modal', function () {
        $("#reactivateProductId").val('');
        $("#stockInput").val('');
    });
});

 
function viewDetails(id) {

    $.ajax({
        url: `/Seller/Product/Details?id=${id}&status=${productStatus}`,
        type: 'GET',
        success: function (html) {
            $("#productDetailModalContent").html(html);
            $("#productDetailModal").modal('show');
        }
    });
}
function deactivateProduct(productId) {
    $.ajax({
        url: `/seller/api/products/deactivate`,
        type: 'POST',
        data: { productId: productId },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                productsTable.ajax.reload(null, false);
                $('#productDetailModal').modal('hide');

            } else {
                toastr.error(response.message);
            }
        },
        error: function (response) {
            toastr.error("An error occurred. Unable to deactivate the product.");
        }
    });
}
function reactivateProduct(id) {
    $('#productDetailModal').modal('hide');
    $("#reactivateProductId").val(id);
    $("#productReactivateModal").modal('show');
}
function submitReactivate() {
    let productId = $("#reactivateProductId").val();
    let stock = parseInt($("#stockInput").val());
    if (!stock || stock <= 0) {
        toastr.error("Please enter a valid stock amount.");
        return;
    }
    $.ajax({
        url: '/seller/api/products/reactivate',
        type: 'POST',
        data: {
            productId: productId,
            stock: stock
        },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                $("#productReactivateModal").modal('hide');
                productsTable.ajax.reload(null, false);

            } else {
                toastr.error(response.message);
            }
        },
        error: function () {
            toastr.error("An error occurred while reactivating the product.");
        }
    });
}
function deleteProduct(id) {
    $.ajax({
        url: `/seller/api/products/delete`,
        type: 'POST',
        data: {
            productId: id
        },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                productsTable.ajax.reload(null, false);
                $('#productDetailModal').modal('hide');
            } else {
                toastr.error(response.message);
            }
        },
        error: function () {
            toastr.error("An error occurred. Unable to delete the product.");
        }
    });
}
