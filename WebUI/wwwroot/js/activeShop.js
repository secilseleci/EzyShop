let shopsTable;

$(document).ready(function () {
    shopsTable = $('#tblShops').DataTable({
        processing: true,
        serverSide: true,
        ajax: { url: '/Admin/Shop/GetActiveShops', type: 'GET' },
        columns: [
            { title: "Shop", data: 'name', width: "10%" },
            { title: "Seller", data: 'sellerName', width: "10%" },
            {
                title: "Details", data: "id",
                width: "15%",
                render: function (data) {
                    return `<a onClick="viewDetails('${data}')" class="btn btn-outline-info btn-sm rounded d-block text-center">
                    <i class="bi bi-eye"></i> Details
                </a>`;
                }
            },
            {
                title: "Action",
                data: "id",
                width: "30%",
                "render": function (data) {
                    return `<div class="btn-group d-flex gap-2 justify-content-center" role="group">
                                <a onClick=approveShop('${data}') class="btn btn-outline-success btn-sm rounded">
                                    Approve
                                </a>
                                <a onClick=rejectShop('${data}') class="btn btn-outline-danger btn-sm rounded">
                                    Reject
                                </a>
                            </div>`;
                }

            }
        ]
    });
});
function viewDetails(id) {
    $.ajax({
        url: `/Admin/Shop/Details/${id}`,
        type: 'GET',
        success: function (html) {
            $("#shopDetailModalContent").html(html);
            $("#shopDetailModal").modal('show');
        }
    });
}

//function deleteShop(ShopId) {
//    $.ajax({
//        url: `/Admin/Shop/Delete`,
//        type: 'POST',
//        data: { applicationId },
//        success: function (response) {
//            if (response.success) {
//                toastr.success(response.message);
//                applicationsTable.ajax.reload(null, false);
//            } else {
//                toastr.error(response.message);
//            }
//        },
//        error: function () {
//            toastr.error("An error occurred. Unable to delete the application.");
//        }
//    });
//}

//function approveApp(applicationId) {
//    $.ajax({
//        url: `/Admin/Shop/Approve`,
//        type: 'POST',
//        data: { applicationId },
//        success: function (response) {
//            if (response.success) {
//                toastr.success(response.message);
//                applicationsTable.ajax.reload(null, false);
//            } else {
//                toastr.error(response.message);
//            }
//        },
//        error: function () {
//            toastr.error("An error occurred while approving the application.");
//        }
//    });
//}

//function rejectApp(applicationId) {
//    $.ajax({
//        url: `/Admin/Application/Reject`,
//        type: 'POST',
//        data: { applicationId },
//        success: function (response) {
//            if (response.success) {
//                toastr.success(response.message);
//                applicationsTable.ajax.reload(null, false);
//            } else {
//                toastr.error(response.message);
//            }
//        },
//        error: function () {
//            toastr.error("An error occurred while rejecting the application.");
//        }
//    });
//}
