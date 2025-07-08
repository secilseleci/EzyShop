let shopsTable;
let columns = [
    { title: "Shop", data: 'name', width: "10%" },
    { title: "Seller", data: 'sellerName', width: "10%" },
    {
        title: "Details", data: "id",
        width: "15%",
        render: function (data) {
            return `<a onClick="viewDetails('${data}')" class="btn btn-info btn-sm rounded d-block text-center">
                        <i class="bi bi-eye"></i> Details
                    </a>`;
        }
    }
];

$(document).ready(function () {
    shopsTable = $('#tblShops').DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            url: '/admin/api/shops?status=' + shopStatus,
            type: 'GET',
            error: function(xhr) {
                let errorMessage = "data could not be retrieved";
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    errorMessage = xhr.responseJSON.message;
                }
            },
        },
        columns: columns
    });
});

if (showActionColumn) {
    columns.push({
        title: "Action",
        data: "id",
        width: "30%",
        render: function (data, type, row) {
            let html = "";

            if (shopStatus === "Pending") {
                html += `<a onClick="approveShop('${row.id}', '${row.sellerId}')" class="btn btn-success btn-sm rounded">Approve</a>
                 <a onClick="rejectShop('${row.id}', '${row.sellerId}')" class="btn btn-danger btn-sm rounded">Reject</a>`;
            }
            else if (shopStatus === "Active") {
                html += `<a onClick="deactivateShop('${row.id}', '${row.sellerId}')" class="btn btn-danger btn-sm rounded">Deactivate</a>`;
            }
            else if (shopStatus === "Inactive") {
                html += `<a onClick="reactivateShop('${row.id}', '${row.sellerId}')" class="btn btn-success btn-sm rounded">Activate</a>
                 <a onClick="deleteShop('${row.id}', '${row.sellerId}')" class="btn btn-danger btn-sm rounded">Delete</a>`;
            }

            return `<div class="btn-group d-flex gap-2 justify-content-center" role="group">${html}</div>`;
        }
    });
}

function viewDetails(id) {
    $.ajax({
        url: `/Admin/Shop/Details/${id}?status=${shopStatus}`,
        type: 'GET',
        success: function (html) {
            $("#shopDetailModalContent").html(html);
            $("#shopDetailModal").modal('show');
        }
    });
}
function approveShop(shopId, sellerId) {
    $.ajax({
        url: `/admin/api/shops/approve`,
        type: 'POST',
        data: {
            shopId: shopId,
            sellerId: sellerId
        },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                shopsTable.ajax.reload(null, false);
                $('#shopDetailModal').modal('hide');
            } else {
                toastr.error(response.message);
            }
        },
        error: function () {
            toastr.error("An error occurred while approving the shop.");
        }
    });
}
function rejectShop(shopId, sellerId) {
    $.ajax({
        url: `/admin/api/shops/reject`,
        type: 'POST',
        data: {
            shopId: shopId,
            sellerId: sellerId
        },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                shopsTable.ajax.reload(null, false);
                $('#shopDetailModal').modal('hide');
            } else {
                toastr.error(response.message);
            }
        },
        error: function () {
            toastr.error("An error occurred while rejecting the shop.");
        }
    });
}
function deleteShop(shopId, sellerId) {
    $.ajax({
        url: `/admin/api/shops/delete`,
        type: 'POST',
        data: {
            shopId: shopId,
            sellerId: sellerId
        },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                shopsTable.ajax.reload(null, false);
                $('#shopDetailModal').modal('hide');
            } else {
                toastr.error(response.message);
            }
        },
        error: function () {
            toastr.error("An error occurred. Unable to delete the shop.");
        }
    });
}
function reactivateShop(shopId, sellerId) {
    $.ajax({
        url: `/admin/api/shops/reactivate`,
        type: 'POST',
        data: {
            shopId: shopId,
            sellerId: sellerId
        },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                shopsTable.ajax.reload(null, false);
                $('#shopDetailModal').modal('hide');
            } else {
                toastr.error(response.message);
            }
        },
        error: function () {
            toastr.error("An error occurred while reactiving the shop.");
        }
    });
}
function deactivateShop(shopId, sellerId) {
    $.ajax({
        url: `/admin/api/shops/deactivate`,
        type: 'POST',
        data: {
            shopId: shopId,
            sellerId: sellerId
        },
        success: function (response) {
            if (response.success) {
                toastr.success(response.message);
                shopsTable.ajax.reload(null, false);
                $('#shopDetailModal').modal('hide');
            } else {
                toastr.error(response.message);
            }
        },
        error: function () {
            toastr.error("An error occurred while deactiving the shop.");
        }
    });
}